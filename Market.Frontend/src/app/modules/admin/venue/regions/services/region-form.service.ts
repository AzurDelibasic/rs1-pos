import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetRegionByIdQueryDto } from '../../../../../api-services/regions/regions-api.model';

export interface RegionForm {
  name: FormControl<string>;
  sortOrder: FormControl<number>;
  isActive: FormControl<boolean>;
}

@Injectable()
export class RegionFormService {
  readonly nameMaxLength = 50;

  createRegionForm(region?: GetRegionByIdQueryDto): FormGroup<RegionForm> {
    return new FormGroup<RegionForm>({
      name: new FormControl(region?.name ?? '', {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(this.nameMaxLength)
        ]
      }),
      sortOrder: new FormControl(region?.sortOrder ?? 0, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(0)]
      }),
      isActive: new FormControl(region?.isActive ?? true, { nonNullable: true })
    });
  }
}