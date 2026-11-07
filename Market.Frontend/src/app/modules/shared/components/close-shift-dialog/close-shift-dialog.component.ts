import { Component, inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { ShiftsApiService } from '../../../../api-services/shifts/shifts-api.service';
import { CloseShiftCommand } from '../../../../api-services/shifts/shifts-api.model';
import { CloseShiftForm, ShiftFormService } from '../../services/shift-form.service';
import { applyServerErrors } from '../../../../core/forms/server-errors';

export interface CloseShiftDialogData {
  shiftId: number;
  cashRegisterName: string;
  userFullName?: string;
  openedAtUtc: string;
  openingCash: number;
  expectedCash: number;
}

@Component({
  selector: 'app-close-shift-dialog',
  standalone: false,
  templateUrl: './close-shift-dialog.component.html',
  styleUrl: './close-shift-dialog.component.scss',
  providers: [ShiftFormService]
})
export class CloseShiftDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<CloseShiftDialogComponent>);
  protected data = inject<CloseShiftDialogData>(MAT_DIALOG_DATA);
  private api = inject(ShiftsApiService);
  protected formService = inject(ShiftFormService);

  form!: FormGroup<CloseShiftForm>;
  isLoading = false;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.form = this.formService.createCloseShiftForm();
  }

  get difference(): number | null {
    const counted = this.form.controls.countedCash.value;
    if (counted === null || counted === undefined) {
      return null;
    }
    return counted - this.data.expectedCash;
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

    const command: CloseShiftCommand = {
      countedCash: this.form.controls.countedCash.value!
    };

    this.api.close(this.data.shiftId, command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(this.form, err);
        console.error('Close shift error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
