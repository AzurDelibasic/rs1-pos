import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { RegionsApiService } from '../../../../../api-services/regions/regions-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  CreateRegionCommand,
  UpdateRegionCommand
} from '../../../../../api-services/regions/regions-api.model';
import { RegionForm, RegionFormService } from '../services/region-form.service';
import { applyServerErrors } from '../../../../../core/forms/server-errors';

export interface RegionDialogData {
  mode: 'create' | 'edit';
  regionId?: number;
}

@Component({
  selector: 'app-region-upsert',
  standalone: false,
  templateUrl: './region-upsert.component.html',
  styleUrl: './region-upsert.component.scss',
  providers: [RegionFormService]
})
export class RegionUpsertComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<RegionUpsertComponent>);
  private data = inject<RegionDialogData>(MAT_DIALOG_DATA);
  private api = inject(RegionsApiService);
  protected formService = inject(RegionFormService);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  form?: FormGroup<RegionForm>;
  isLoading = false;
  errorMessage: string | null = null;
  isEditMode = false;

  ngOnInit(): void {
    this.isEditMode = this.data.mode === 'edit';

    if (this.isEditMode && this.data.regionId) {
      this.loadRegion(this.data.regionId);
    } else {
      this.form = this.formService.createRegionForm();
    }
  }

  private loadRegion(id: number): void {
    this.isLoading = true;

    this.api.getById(id).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (region) => {
        this.form = this.formService.createRegionForm(region);
        this.isLoading = false;
      },
      error: (err) => {
        this.toaster.error(this.translate.instant('REGIONS.DIALOGS.ERROR_LOAD'));
        console.error('Load region error:', err);
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

    if (this.isEditMode && this.data.regionId) {
      this.updateRegion(this.form);
    } else {
      this.createRegion(this.form);
    }
  }

  private createRegion(form: FormGroup<RegionForm>): void {
    const command: CreateRegionCommand = {
      name: form.controls.name.value.trim(),
      sortOrder: form.controls.sortOrder.value,
      isActive: form.controls.isActive.value
    };

    this.api.create(command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Create region error:', err);
      }
    });
  }

  private updateRegion(form: FormGroup<RegionForm>): void {
    const command: UpdateRegionCommand = {
      name: form.controls.name.value.trim(),
      sortOrder: form.controls.sortOrder.value,
      isActive: form.controls.isActive.value
    };

    this.api.update(this.data.regionId!, command).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = applyServerErrors(form, err);
        console.error('Update region error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
