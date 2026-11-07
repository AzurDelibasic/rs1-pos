import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

export interface OpenShiftForm {
  userId: FormControl<number | null>;
  cashRegisterId: FormControl<number | null>;
  openingCash: FormControl<number>;
}

export interface CloseShiftForm {
  countedCash: FormControl<number | null>;
}

@Injectable()
export class ShiftFormService {
  readonly cashMax = 1000000;

  createOpenShiftForm(requireWaiter: boolean): FormGroup<OpenShiftForm> {
    return new FormGroup<OpenShiftForm>({
      userId: new FormControl<number | null>(null, {
        validators: requireWaiter ? [Validators.required] : []
      }),
      cashRegisterId: new FormControl<number | null>(null, {
        validators: [Validators.required]
      }),
      openingCash: new FormControl(0, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(0), Validators.max(this.cashMax)]
      })
    });
  }

  createCloseShiftForm(): FormGroup<CloseShiftForm> {
    return new FormGroup<CloseShiftForm>({
      countedCash: new FormControl<number | null>(null, {
        validators: [Validators.required, Validators.min(0), Validators.max(this.cashMax)]
      })
    });
  }
}
