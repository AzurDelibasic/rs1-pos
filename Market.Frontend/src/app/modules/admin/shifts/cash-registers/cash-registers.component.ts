import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { CashRegistersApiService } from '../../../../api-services/cash-registers/cash-registers-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListCashRegistersRequest,
  ListCashRegistersQueryDto,
} from '../../../../api-services/cash-registers/cash-registers-api.model';
import { CashRegisterUpsertComponent } from './cash-register-upsert/cash-register-upsert.component';

@Component({
  selector: 'app-cash-registers',
  standalone: false,
  templateUrl: './cash-registers.component.html',
  styleUrl: './cash-registers.component.scss',
})
export class CashRegistersComponent
  extends BaseListPagedComponent<ListCashRegistersQueryDto, ListCashRegistersRequest>
  implements OnInit
{
  private api = inject(CashRegistersApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private dialogHelper = inject(DialogHelperService);
  private translate = inject(TranslateService);

  displayedColumns: string[] = ['name', 'isActive', 'actions'];
  showOnlyActive = true;

  constructor() {
    super();
    this.request = new ListCashRegistersRequest();
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
        this.stopLoading(this.translate.instant('CASH_REGISTERS.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load cash registers error:', err);
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
    const dialogRef = this.dialog.open(CashRegisterUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'cash-register-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.cashRegister.showCreateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onEdit(cashRegister: ListCashRegistersQueryDto): void {
    const dialogRef = this.dialog.open(CashRegisterUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'cash-register-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'edit',
        cashRegisterId: cashRegister.id,
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.cashRegister.showUpdateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onDelete(cashRegister: ListCashRegistersQueryDto): void {
    this.dialogHelper.cashRegister.confirmDelete(cashRegister.name).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(cashRegister);
      }
    });
  }

  private performDelete(cashRegister: ListCashRegistersQueryDto): void {
    this.startLoading();

    this.api.delete(cashRegister.id).subscribe({
      next: () => {
        this.dialogHelper.cashRegister.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper.showError(
          'DIALOGS.TITLES.ERROR',
          'CASH_REGISTERS.DIALOGS.ERROR_DELETE'
        ).subscribe();

        console.error('Delete cash register error:', err);
      },
    });
  }

  onToggleStatus(cashRegister: ListCashRegistersQueryDto): void {
    this.startLoading();

    const apiAction = cashRegister.isActive
      ? this.api.deactivate(cashRegister.id)
      : this.api.activate(cashRegister.id);

    apiAction.subscribe({
      next: () => {
        const messageKey = cashRegister.isActive
          ? 'CASH_REGISTERS.DIALOGS.SUCCESS_DEACTIVATE'
          : 'CASH_REGISTERS.DIALOGS.SUCCESS_ACTIVATE';
        this.toaster.success(this.translate.instant(messageKey, { name: cashRegister.name }));
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        const errorMessage = this.extractErrorMessage(err);

        if (err.status === 409) {
          this.dialogHelper.showWarning(
            'DIALOGS.TITLES.WARNING',
            errorMessage || 'CASH_REGISTERS.DIALOGS.ERROR_TOGGLE',
            { name: cashRegister.name }
          ).subscribe();
        } else {
          this.toaster.error(
            errorMessage ||
              this.translate.instant('CASH_REGISTERS.DIALOGS.ERROR_TOGGLE', { name: cashRegister.name })
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
