import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  GetAppSettingsQueryDto,
  RoundingModeType
} from '../../../../api-services/app-settings/app-settings-api.model';

export interface AppSettingsForm {
  businessName: FormControl<string>;
  address: FormControl<string>;
  taxNumber: FormControl<string>;
  countryId: FormControl<number | null>;
  defaultCurrencyCode: FormControl<string>;
  roundingMode: FormControl<RoundingModeType>;
  reservationHoldMinutes: FormControl<number>;
}

@Injectable()
export class AppSettingsFormService {
  readonly businessNameMaxLength = 100;
  readonly addressMaxLength = 200;
  readonly taxNumberMaxLength = 20;
  readonly reservationHoldMinutesMax = 240;

  createAppSettingsForm(settings?: GetAppSettingsQueryDto): FormGroup<AppSettingsForm> {
    return new FormGroup<AppSettingsForm>({
      businessName: new FormControl(settings?.businessName ?? '', {
        nonNullable: true,
        validators: [Validators.required, Validators.maxLength(this.businessNameMaxLength)]
      }),
      address: new FormControl(settings?.address ?? '', {
        nonNullable: true,
        validators: [Validators.maxLength(this.addressMaxLength)]
      }),
      taxNumber: new FormControl(settings?.taxNumber ?? '', {
        nonNullable: true,
        validators: [Validators.maxLength(this.taxNumberMaxLength)]
      }),
      countryId: new FormControl<number | null>(settings?.countryId ?? null, {
        validators: [Validators.required]
      }),
      defaultCurrencyCode: new FormControl(settings?.defaultCurrencyCode ?? 'BAM', {
        nonNullable: true,
        validators: [Validators.required]
      }),
      roundingMode: new FormControl(settings?.roundingMode ?? RoundingModeType.None, {
        nonNullable: true,
        validators: [Validators.required]
      }),
      reservationHoldMinutes: new FormControl(settings?.reservationHoldMinutes ?? 15, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(0), Validators.max(this.reservationHoldMinutesMax)]
      })
    });
  }
}
