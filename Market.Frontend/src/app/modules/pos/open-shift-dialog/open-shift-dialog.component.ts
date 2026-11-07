import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ShiftsApiService } from '../../../api-services/shifts/shifts-api.service';
import { CashRegistersApiService } from '../../../api-services/cash-registers/cash-registers-api.service';
import { ListCashRegistersQueryDto } from '../../../api-services/cash-registers/cash-registers-api.model';
import { ListWaitersQueryDto, OpenShiftCommand } from '../../../api-services/shifts/shifts-api.model';
import { ToasterService } from '../../../core/services/toaster.service';
import { allItemsPaging } from '../../../core/models/paging/paging-utils';
import { applyServerErrors } from '../../../core/forms/server-errors';
import { OpenShiftForm, ShiftFormService } from '../../shared/services/shift-form.service';

export interface OpenShiftDialogData {
  selectWaiter: boolean;
}

@Component({
  selector: 'app-open-shift-dialog',
  standalone: false,
  templateUrl: './open-shift-dialog.component.html',
  styleUrl: './open-shift-dialog.component.scss',
  providers: [ShiftFormService]
})
export class OpenShiftDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<OpenShiftDialogComponent>);
  protected data = inject<OpenShiftDialogData>(MAT_DIALOG_DATA);
  private api = inject(ShiftsApiService);
  private cashRegistersApi = inject(CashRegistersApiService);
  protected formService = inject(ShiftFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form!: FormGroup<OpenShiftForm>;
  cashRegisters: ListCashRegistersQueryDto[] = [];
  waiters: ListWaitersQueryDto[] = [];
  isLoading = false;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.form = this.formService.createOpenShiftForm(this.data.selectWaiter);
    this.loadCashRegisters();

    if (this.data.selectWaiter) {
      this.loadWaiters();
    }
  }

  private loadWaiters(): void {
    this.api
      .listWaiters({ paging: allItemsPaging })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.waiters = response.items;
        },
        error: (err) => {
          this.toaster.error(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD_WAITERS'));
          console.error('Load waiters error:', err);
        }
      });
  }

  private loadCashRegisters(): void {
    this.cashRegistersApi
      .list({ paging: allItemsPaging, onlyActive: true })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.cashRegisters = response.items;
        },
        error: (err) => {
          this.toaster.error(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD_CASH_REGISTERS'));
          console.error('Load cash registers error:', err);
        }
      });
  }

  onSubmit(): void {
    if (this.isLoading) {
      return;
    }
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const command: OpenShiftCommand = {
      cashRegisterId: this.form.controls.cashRegisterId.value!,
      openingCash: this.form.controls.openingCash.value,
      userId: this.data.selectWaiter ? this.form.controls.userId.value : null
    };

    this.api.open(command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(this.form, err);
        console.error('Open shift error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
