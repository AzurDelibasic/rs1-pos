import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { TablesApiService } from '../../../../api-services/tables/tables-api.service';
import { RegionsApiService } from '../../../../api-services/regions/regions-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListTablesRequest,
  ListTablesQueryDto,
} from '../../../../api-services/tables/tables-api.model';
import { ListRegionsQueryDto } from '../../../../api-services/regions/regions-api.model';
import { allItemsPaging } from '../../../../core/models/paging/paging-utils';
import { TableUpsertComponent } from './table-upsert/table-upsert.component';

@Component({
  selector: 'app-tables',
  standalone: false,
  templateUrl: './tables.component.html',
  styleUrl: './tables.component.scss',
})
export class TablesComponent
  extends BaseListPagedComponent<ListTablesQueryDto, ListTablesRequest>
  implements OnInit
{
  private api = inject(TablesApiService);
  private regionsApi = inject(RegionsApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private dialogHelper = inject(DialogHelperService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  displayedColumns: string[] = ['regionName', 'label', 'seats', 'isActive', 'actions'];
  showOnlyActive = true;
  regions: ListRegionsQueryDto[] = [];

  constructor() {
    super();
    this.request = new ListTablesRequest();
    this.request.onlyActive = true;
  }

  ngOnInit(): void {
    this.loadRegions();
    this.initList();
  }

  private loadRegions(): void {
    this.regionsApi
      .list({ paging: allItemsPaging })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.regions = response.items;
        },
        error: (err) => {
          this.toaster.error(this.translate.instant('TABLES.DIALOGS.ERROR_LOAD_REGIONS'));
          console.error('Load regions error:', err);
        }
      });
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('TABLES.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load tables error:', err);
      },
    });
  }

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onRegionFilterChange(regionId: number | null): void {
    this.request.regionId = regionId;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onToggleActiveFilter(checked: boolean): void {
    this.showOnlyActive = checked;
    this.request.onlyActive = checked ? true : null;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onCreate(): void {
    const dialogRef = this.dialog.open(TableUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'table-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.table.showCreateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onEdit(table: ListTablesQueryDto): void {
    const dialogRef = this.dialog.open(TableUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'table-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'edit',
        tableId: table.id,
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.table.showUpdateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onDelete(table: ListTablesQueryDto): void {
    this.dialogHelper.table.confirmDelete(table.label).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(table);
      }
    });
  }

  private performDelete(table: ListTablesQueryDto): void {
    this.startLoading();

    this.api.delete(table.id).subscribe({
      next: () => {
        this.dialogHelper.table.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper.showError(
          'DIALOGS.TITLES.ERROR',
          'TABLES.DIALOGS.ERROR_DELETE'
        ).subscribe();

        console.error('Delete table error:', err);
      },
    });
  }

  onToggleStatus(table: ListTablesQueryDto): void {
    this.startLoading();

    const apiAction = table.isActive
      ? this.api.deactivate(table.id)
      : this.api.activate(table.id);

    apiAction.subscribe({
      next: () => {
        const messageKey = table.isActive
          ? 'TABLES.DIALOGS.SUCCESS_DEACTIVATE'
          : 'TABLES.DIALOGS.SUCCESS_ACTIVATE';
        this.toaster.success(this.translate.instant(messageKey, { name: table.label }));
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        const errorMessage = this.extractErrorMessage(err);

        if (err.status === 409) {
          this.dialogHelper.showWarning(
            'DIALOGS.TITLES.WARNING',
            errorMessage || 'TABLES.DIALOGS.ERROR_TOGGLE',
            { name: table.label }
          ).subscribe();
        } else {
          this.toaster.error(
            errorMessage ||
              this.translate.instant('TABLES.DIALOGS.ERROR_TOGGLE', { name: table.label })
          );
        }

        console.error('Toggle status error:', err);
        this.loadPagedData();
      },
    });
  }

  private extractErrorMessage(err: any): string | null {
    if (err?.error) {
      if (typeof err.error === 'string') {
        return err.error;
      }

      if (err.error.message) {
        return err.error.message;
      }

      if (err.error.title) {
        return err.error.title;
      }

      if (err.error.errors && typeof err.error.errors === 'object') {
        const errors = Object.values(err.error.errors).flat();
        if (errors.length > 0) {
          return errors.join(', ');
        }
      }
    }

    if (err?.message) {
      return err.message;
    }

    if (err?.statusText && err.statusText !== 'Unknown Error') {
      return err.statusText;
    }

    return null;
  }
}
