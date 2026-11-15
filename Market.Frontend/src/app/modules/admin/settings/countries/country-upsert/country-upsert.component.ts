import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { CountriesApiService } from '../../../../../api-services/countries/countries-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  CreateCountryCommand,
  UpdateCountryCommand
} from '../../../../../api-services/countries/countries-api.model';
import { CountryForm, CountryFormService } from '../services/country-form.service';
import { applyServerErrors } from '../../../../../core/forms/server-errors';

export interface CountryDialogData {
  mode: 'create' | 'edit';
  countryId?: number;
}

@Component({
  selector: 'app-country-upsert',
  standalone: false,
  templateUrl: './country-upsert.component.html',
  styleUrl: './country-upsert.component.scss',
  providers: [CountryFormService]
})
export class CountryUpsertComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<CountryUpsertComponent>);
  private data = inject<CountryDialogData>(MAT_DIALOG_DATA);
  private api = inject(CountriesApiService);
  protected formService = inject(CountryFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form?: FormGroup<CountryForm>;
  isLoading = false;
  errorMessage: string | null = null;
  isEditMode = false;

  ngOnInit(): void {
    this.isEditMode = this.data.mode === 'edit';

    if (this.isEditMode && this.data.countryId) {
      this.loadCountry(this.data.countryId);
    } else {
      this.form = this.formService.createCountryForm();
    }
  }

  private loadCountry(id: number): void {
    this.isLoading = true;

    this.api.getById(id).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (country) => {
        this.form = this.formService.createCountryForm(country);
        this.isLoading = false;
      },
      error: (err) => {
        this.toaster.error(this.translate.instant('COUNTRIES.DIALOGS.ERROR_LOAD'));
        console.error('Load country error:', err);
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

    if (this.isEditMode && this.data.countryId) {
      this.updateCountry(this.form);
    } else {
      this.createCountry(this.form);
    }
  }

  private createCountry(form: FormGroup<CountryForm>): void {
    const command: CreateCountryCommand = {
      name: form.controls.name.value.trim(),
      code: form.controls.code.value.trim().toUpperCase(),
      vatRate: form.controls.vatRate.value
    };

    this.api.create(command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Create country error:', err);
      }
    });
  }

  private updateCountry(form: FormGroup<CountryForm>): void {
    const command: UpdateCountryCommand = {
      name: form.controls.name.value.trim(),
      code: form.controls.code.value.trim().toUpperCase(),
      vatRate: form.controls.vatRate.value
    };

    this.api.update(this.data.countryId!, command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Update country error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
