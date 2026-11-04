import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { TablesApiService } from '../../../../../api-services/tables/tables-api.service';
import { RegionsApiService } from '../../../../../api-services/regions/regions-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  CreateTableCommand,
  UpdateTableCommand
} from '../../../../../api-services/tables/tables-api.model';
import { ListRegionsQueryDto } from '../../../../../api-services/regions/regions-api.model';
import { TableForm, TableFormService } from '../services/table-form.service';
import { applyServerErrors } from '../../../../../core/forms/server-errors';
import { allItemsPaging } from '../../../../../core/models/paging/paging-utils';

export interface TableDialogData {
  mode: 'create' | 'edit';
  tableId?: number;
}

@Component({
  selector: 'app-table-upsert',
  standalone: false,
  templateUrl: './table-upsert.component.html',
  styleUrl: './table-upsert.component.scss',
  providers: [TableFormService]
})
export class TableUpsertComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<TableUpsertComponent>);
  private data = inject<TableDialogData>(MAT_DIALOG_DATA);
  private api = inject(TablesApiService);
  private regionsApi = inject(RegionsApiService);
  protected formService = inject(TableFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form?: FormGroup<TableForm>;
  regions: ListRegionsQueryDto[] = [];
  isLoading = false;
  errorMessage: string | null = null;
  isEditMode = false;

  ngOnInit(): void {
    this.isEditMode = this.data.mode === 'edit';
    this.loadRegions();

    if (this.isEditMode && this.data.tableId) {
      this.loadTable(this.data.tableId);
    } else {
      this.form = this.formService.createTableForm();
    }
  }

  private loadRegions(): void {
    this.regionsApi
      .list({ onlyActive: true, paging: allItemsPaging })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.regions = response.items;
        },
        error: (err) => {
          this.toaster.error(this.translate.instant('TABLES.DIALOGS.ERROR_LOAD_REGIONS'));
          console.error('Load regions error:', err);
        }
      });
  }

  private loadTable(id: number): void {
    this.isLoading = true;

    this.api.getById(id).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (table) => {
        this.form = this.formService.createTableForm(table);
        this.isLoading = false;
      },
      error: (err) => {
        this.toaster.error(this.translate.instant('TABLES.DIALOGS.ERROR_LOAD'));
        console.error('Load table error:', err);
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

    if (this.isEditMode && this.data.tableId) {
      this.updateTable(this.form);
    } else {
      this.createTable(this.form);
    }
  }

  private createTable(form: FormGroup<TableForm>): void {
    const command: CreateTableCommand = {
      label: form.controls.label.value.trim(),
      seats: form.controls.seats.value,
      regionId: form.controls.regionId.value ?? 0,
      isActive: form.controls.isActive.value
    };

    this.api.create(command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Create table error:', err);
      }
    });
  }

  private updateTable(form: FormGroup<TableForm>): void {
    const command: UpdateTableCommand = {
      label: form.controls.label.value.trim(),
      seats: form.controls.seats.value,
      regionId: form.controls.regionId.value ?? 0,
      isActive: form.controls.isActive.value
    };

    this.api.update(this.data.tableId!, command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Update table error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
