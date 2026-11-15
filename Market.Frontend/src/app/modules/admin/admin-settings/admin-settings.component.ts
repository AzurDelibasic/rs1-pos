import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AppSettingsApiService } from '../../../api-services/app-settings/app-settings-api.service';
import {
  RoundingModeType,
  UpdateAppSettingsCommand
} from '../../../api-services/app-settings/app-settings-api.model';
import { CountriesApiService } from '../../../api-services/countries/countries-api.service';
import { ListCountriesQueryDto } from '../../../api-services/countries/countries-api.model';
import { CurrenciesApiService } from '../../../api-services/currencies/currencies-api.service';
import { ListCurrenciesQueryDto } from '../../../api-services/currencies/currencies-api.model';
import { ToasterService } from '../../../core/services/toaster.service';
import { allItemsPaging } from '../../../core/models/paging/paging-utils';
import { applyServerErrors } from '../../../core/forms/server-errors';
import { AppSettingsForm, AppSettingsFormService } from './services/app-settings-form.service';

@Component({
  selector: 'app-admin-settings',
  standalone: false,
  templateUrl: './admin-settings.component.html',
  styleUrl: './admin-settings.component.scss',
  providers: [AppSettingsFormService]
})
export class AdminSettingsComponent extends BaseComponent implements OnInit {
  private api = inject(AppSettingsApiService);
  private countriesApi = inject(CountriesApiService);
  private currenciesApi = inject(CurrenciesApiService);
  protected formService = inject(AppSettingsFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form?: FormGroup<AppSettingsForm>;
  countries: ListCountriesQueryDto[] = [];
  currencies: ListCurrenciesQueryDto[] = [];
  isSaving = false;
  saveError: string | null = null;

  readonly roundingModes = [
    { value: RoundingModeType.None, key: 'ADMIN_SETTINGS.ROUNDING.NONE' },
    { value: RoundingModeType.NearestFiveCents, key: 'ADMIN_SETTINGS.ROUNDING.FIVE_CENTS' },
    { value: RoundingModeType.NearestTenCents, key: 'ADMIN_SETTINGS.ROUNDING.TEN_CENTS' },
    { value: RoundingModeType.NearestWhole, key: 'ADMIN_SETTINGS.ROUNDING.WHOLE' },
  ];

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.startLoading();

    forkJoin({
      settings: this.api.get(),
      countries: this.countriesApi.list({ paging: allItemsPaging }),
      currencies: this.currenciesApi.list(),
    })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: ({ settings, countries, currencies }) => {
          this.countries = countries.items;
          this.currencies = currencies;
          this.form = this.formService.createAppSettingsForm(settings);
          this.stopLoading();
        },
        error: (err) => {
          this.stopLoading(this.translate.instant('ADMIN_SETTINGS.ERROR_LOAD'));
          console.error('Load app settings error:', err);
        }
      });
  }

  get selectedVatRate(): number | null {
    const countryId = this.form?.controls.countryId.value;
    return this.countries.find(c => c.id === countryId)?.vatRate ?? null;
  }

  onSubmit(): void {
    if (!this.form || this.isSaving) {
      return;
    }
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const form = this.form;
    const value = form.getRawValue();

    const command: UpdateAppSettingsCommand = {
      businessName: value.businessName.trim(),
      address: value.address.trim() || null,
      taxNumber: value.taxNumber.trim() || null,
      countryId: value.countryId!,
      defaultCurrencyCode: value.defaultCurrencyCode,
      roundingMode: value.roundingMode,
      reservationHoldMinutes: value.reservationHoldMinutes,
    };

    this.isSaving = true;
    this.saveError = null;

    this.api.update(command).subscribe({
      next: () => {
        this.isSaving = false;
        form.markAsPristine();
        this.toaster.success(this.translate.instant('ADMIN_SETTINGS.SUCCESS_SAVE'));
      },
      error: (err) => {
        this.isSaving = false;
        this.saveError = applyServerErrors(form, err);
        console.error('Update app settings error:', err);
      }
    });
  }
}
