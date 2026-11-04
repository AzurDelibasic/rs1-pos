import { Component, inject, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { FloorPlanApiService } from '../../../api-services/floor-plan/floor-plan-api.service';
import {
  GetFloorPlanQueryDtoRegion,
  TableStatusType
} from '../../../api-services/floor-plan/floor-plan-api.model';

@Component({
  selector: 'app-floor-plan',
  standalone: false,
  templateUrl: './floor-plan.component.html',
  styleUrl: './floor-plan.component.scss',
})
export class FloorPlanComponent extends BaseComponent implements OnInit {
  private api = inject(FloorPlanApiService);
  private translate = inject(TranslateService);

  regions: GetFloorPlanQueryDtoRegion[] = [];

  readonly statuses = [
    { value: TableStatusType.Free, key: 'POS.FLOOR_PLAN.STATUS.FREE', cssClass: 'free' },
    { value: TableStatusType.Occupied, key: 'POS.FLOOR_PLAN.STATUS.OCCUPIED', cssClass: 'occupied' },
    { value: TableStatusType.Reserved, key: 'POS.FLOOR_PLAN.STATUS.RESERVED', cssClass: 'reserved' },
  ];

  ngOnInit(): void {
    this.loadFloorPlan();
  }

  loadFloorPlan(): void {
    this.startLoading();

    this.api.get().subscribe({
      next: (response) => {
        this.regions = response.regions;
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('POS.FLOOR_PLAN.ERROR_LOAD'));
        console.error('Load floor plan error:', err);
      },
    });
  }

  statusOf(status: TableStatusType) {
    return this.statuses.find(s => s.value === status) ?? this.statuses[0];
  }
}
