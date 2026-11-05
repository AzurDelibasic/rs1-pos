import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { CashRegistersApiService } from '../../../../../api-services/cash-registers/cash-registers-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  CreateCashRegisterCommand,
  UpdateCashRegisterCommand
} from '../../../../../api-services/cash-registers/cash-registers-api.model';
import { CashRegisterForm, CashRegisterFormService } from '../services/cash-register-form.service';
import { applyServerErrors } from '../../../../../core/forms/server-errors';

export interface CashRegisterDialogData {
  mode: 'create' | 'edit';
  cashRegisterId?: number;
}

@Component({
  selector: 'app-cash-register-upsert',
  standalone: false,
  templateUrl: './cash-register-upsert.component.html',
  styleUrl: './cash-register-upsert.component.scss',
  providers: [CashRegisterFormService]
})
export class CashRegisterUpsertComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<CashRegisterUpsertComponent>);
  private data = inject<CashRegisterDialogData>(MAT_DIALOG_DATA);
  private api = inject(CashRegistersApiService);
  protected formService = inject(CashRegisterFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form?: FormGroup<CashRegisterForm>;
  isLoading = false;
  errorMessage: string | null = null;
  isEditMode = false;

  ngOnInit(): void {
    this.isEditMode = this.data.mode === 'edit';

    if (this.isEditMode && this.data.cashRegisterId) {
      this.loadCashRegister(this.data.cashRegisterId);
    } else {
      this.form = this.formService.createCashRegisterForm();
    }
  }

  private loadCashRegister(id: number): void {
    this.isLoading = true;

    this.api.getById(id).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (cashRegister) => {
        this.form = this.formService.createCashRegisterForm(cashRegister);
        this.isLoading = false;
      },
      error: (err) => {
        this.toaster.error(this.translate.instant('CASH_REGISTERS.DIALOGS.ERROR_LOAD'));
        console.error('Load cash register error:', err);
        this.dialogRef.close(false);
      }
    });
  }

  onSubmit(): void {
    if (!this.form || this.isLoading) {
      return;
    }
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    if (this.isEditMode && this.data.cashRegisterId) {
      this.updateCashRegister(this.form);
    } else {
      this.createCashRegister(this.form);
    }
  }

  private createCashRegister(form: FormGroup<CashRegisterForm>): void {
    const command: CreateCashRegisterCommand = {
      name: form.controls.name.value.trim(),
      isActive: form.controls.isActive.value
    };

    this.api.create(command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Create cash register error:', err);
      }
    });
  }

  private updateCashRegister(form: FormGroup<CashRegisterForm>): void {
    const command: UpdateCashRegisterCommand = {
      name: form.controls.name.value.trim(),
      isActive: form.controls.isActive.value
    };

    this.api.update(this.data.cashRegisterId!, command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Update cash register error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
