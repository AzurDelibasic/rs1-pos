import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { CountriesApiService } from '../../../../api-services/countries/countries-api.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListCountriesRequest,
  ListCountriesQueryDto,
} from '../../../../api-services/countries/countries-api.model';
import { CountryUpsertComponent } from './country-upsert/country-upsert.component';

@Component({
  selector: 'app-countries',
  standalone: false,
  templateUrl: './countries.component.html',
  styleUrl: './countries.component.scss',
})
export class CountriesComponent
  extends BaseListPagedComponent<ListCountriesQueryDto, ListCountriesRequest>
  implements OnInit
{
  private api = inject(CountriesApiService);
  private dialog = inject(MatDialog);
  private dialogHelper = inject(DialogHelperService);
  private translate = inject(TranslateService);

  displayedColumns: string[] = ['code', 'name', 'vatRate', 'actions'];

  constructor() {
    super();
    this.request = new ListCountriesRequest();
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('COUNTRIES.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load countries error:', err);
      },
    });
  }

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onCreate(): void {
    const dialogRef = this.dialog.open(CountryUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'country-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.country.showCreateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onEdit(country: ListCountriesQueryDto): void {
    const dialogRef = this.dialog.open(CountryUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'country-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'edit',
        countryId: country.id,
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.country.showUpdateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onDelete(country: ListCountriesQueryDto): void {
    this.dialogHelper.country.confirmDelete(country.name).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(country);
      }
    });
  }

  private performDelete(country: ListCountriesQueryDto): void {
    this.startLoading();

    this.api.delete(country.id).subscribe({
      next: () => {
        this.dialogHelper.country.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper.showError(
          'DIALOGS.TITLES.ERROR',
          'COUNTRIES.DIALOGS.ERROR_DELETE'
        ).subscribe();

        console.error('Delete country error:', err);
      },
    });
  }
}
