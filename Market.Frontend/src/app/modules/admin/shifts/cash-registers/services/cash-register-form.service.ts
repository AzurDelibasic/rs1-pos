import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetCashRegisterByIdQueryDto } from '../../../../../api-services/cash-registers/cash-registers-api.model';

export interface CashRegisterForm {
  name: FormControl<string>;
  isActive: FormControl<boolean>;
}

@Injectable()
export class CashRegisterFormService {
  readonly nameMaxLength = 50;

  createCashRegisterForm(cashRegister?: GetCashRegisterByIdQueryDto): FormGroup<CashRegisterForm> {
    return new FormGroup<CashRegisterForm>({
      name: new FormControl(cashRegister?.name ?? '', {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(this.nameMaxLength)
        ]
      }),
      isActive: new FormControl(cashRegister?.isActive ?? true, { nonNullable: true })
    });
  }
}