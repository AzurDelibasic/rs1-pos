import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetCountryByIdQueryDto } from '../../../../../api-services/countries/countries-api.model';

export interface CountryForm {
  name: FormControl<string>;
  code: FormControl<string>;
  vatRate: FormControl<number>;
}

@Injectable()
export class CountryFormService {
  readonly nameMaxLength = 100;
  readonly codeLength = 2;

  createCountryForm(country?: GetCountryByIdQueryDto): FormGroup<CountryForm> {
    return new FormGroup<CountryForm>({
      name: new FormControl(country?.name ?? '', {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(this.nameMaxLength)
        ]
      }),
      code: new FormControl(country?.code ?? '', {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(this.codeLength),
          Validators.maxLength(this.codeLength),
          Validators.pattern('^[A-Za-z]{2}$')
        ]
      }),
      vatRate: new FormControl(country?.vatRate ?? 0, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(0), Validators.max(100)]
      })
    });
  }
}
