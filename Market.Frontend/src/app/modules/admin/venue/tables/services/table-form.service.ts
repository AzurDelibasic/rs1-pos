import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetTableByIdQueryDto } from '../../../../../api-services/tables/tables-api.model';

export interface TableForm {
  label: FormControl<string>;
  seats: FormControl<number>;
  regionId: FormControl<number | null>;
  isActive: FormControl<boolean>;
}

@Injectable()
export class TableFormService {
  readonly labelMaxLength = 20;
  readonly seatsMin = 1;
  readonly seatsMax = 50;

  createTableForm(table?: GetTableByIdQueryDto): FormGroup<TableForm> {
    return new FormGroup<TableForm>({
      label: new FormControl(table?.label ?? '', {
        nonNullable: true,
        validators: [Validators.required, Validators.maxLength(this.labelMaxLength)]
      }),
      seats: new FormControl(table?.seats ?? 4, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(this.seatsMin), Validators.max(this.seatsMax)]
      }),
      regionId: new FormControl<number | null>(table?.regionId ?? null, [
        Validators.required
      ]),
      isActive: new FormControl(table?.isActive ?? true, { nonNullable: true })
    });
  }
}
