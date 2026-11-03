import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { RegionsApiService } from '../../../../api-services/regions/regions-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListRegionsRequest,
  ListRegionsQueryDto,
} from '../../../../api-services/regions/regions-api.model';
import { RegionUpsertComponent } from './region-upsert/region-upsert.component';

@Component({
  selector: 'app-regions',
  standalone: false,
  templateUrl: './regions.component.html',
  styleUrl: './regions.component.scss',
})
export class RegionsComponent
  extends BaseListPagedComponent<ListRegionsQueryDto, ListRegionsRequest>
  implements OnInit
{
  private api = inject(RegionsApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private dialogHelper = inject(DialogHelperService);
  private translate = inject(TranslateService);

  displayedColumns: string[] = ['sortOrder', 'name', 'isActive', 'actions'];
  showOnlyActive = true;

  constructor() {
    super();
    this.request = new ListRegionsRequest();
    this.request.onlyActive = true;
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('REGIONS.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load regions error:', err);
      },
    });
  }

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
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
    const dialogRef = this.dialog.open(RegionUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'region-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.region.showCreateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onEdit(region: ListRegionsQueryDto): void {
    const dialogRef = this.dialog.open(RegionUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'region-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'edit',
        regionId: region.id,
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.region.showUpdateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onDelete(region: ListRegionsQueryDto): void {
    this.dialogHelper.region.confirmDelete(region.name).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(region);
      }
    });
  }

  private performDelete(region: ListRegionsQueryDto): void {
    this.startLoading();

    this.api.delete(region.id).subscribe({
      next: () => {
        this.dialogHelper.region.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper.showError(
          'DIALOGS.TITLES.ERROR',
          'REGIONS.DIALOGS.ERROR_DELETE'
        ).subscribe();

        console.error('Delete region error:', err);
      },
    });
  }

  onToggleStatus(region: ListRegionsQueryDto): void {
    this.startLoading();

    const apiAction = region.isActive
      ? this.api.deactivate(region.id)
      : this.api.activate(region.id);

    apiAction.subscribe({
      next: () => {
        const messageKey = region.isActive
          ? 'REGIONS.DIALOGS.SUCCESS_DEACTIVATE'
          : 'REGIONS.DIALOGS.SUCCESS_ACTIVATE';
        this.toaster.success(this.translate.instant(messageKey, { name: region.name }));
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        const errorMessage = this.extractErrorMessage(err);

        if (err.status === 409) {
          this.dialogHelper.showWarning(
            'DIALOGS.TITLES.WARNING',
            errorMessage || 'REGIONS.DIALOGS.ERROR_TOGGLE',
            { name: region.name }
          ).subscribe();
        } else {
          this.toaster.error(
            errorMessage ||
              this.translate.instant('REGIONS.DIALOGS.ERROR_TOGGLE', { name: region.name })
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
