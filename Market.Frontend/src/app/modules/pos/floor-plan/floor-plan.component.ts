import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { FloorPlanApiService } from '../../../api-services/floor-plan/floor-plan-api.service';
import {
  GetFloorPlanQueryDtoRegion,
  TableStatusType
} from '../../../api-services/floor-plan/floor-plan-api.model';
import { ShiftsApiService } from '../../../api-services/shifts/shifts-api.service';
import {
  GetCurrentShiftQueryDto,
  ListShiftsQueryDto,
  ShiftStatusType
} from '../../../api-services/shifts/shifts-api.model';
import { ToasterService } from '../../../core/services/toaster.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { allItemsPaging } from '../../../core/models/paging/paging-utils';
import {
  OpenShiftDialogComponent,
  OpenShiftDialogData,
} from '../open-shift-dialog/open-shift-dialog.component';
import {
  CloseShiftDialogComponent,
  CloseShiftDialogData,
} from '../../shared/components/close-shift-dialog/close-shift-dialog.component';

@Component({
  selector: 'app-floor-plan',
  standalone: false,
  templateUrl: './floor-plan.component.html',
  styleUrl: './floor-plan.component.scss',
})
export class FloorPlanComponent extends BaseComponent implements OnInit {
  private api = inject(FloorPlanApiService);
  private shiftsApi = inject(ShiftsApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private currentUser = inject(CurrentUserService);

  regions: GetFloorPlanQueryDtoRegion[] = [];
  shift: GetCurrentShiftQueryDto | null = null;
  isShiftLoaded = false;
  openShifts: ListShiftsQueryDto[] = [];

  readonly isWaiter =
    this.currentUser.isEmployee() && !this.currentUser.isAdmin() && !this.currentUser.isManager();

  readonly statuses = [
    { value: TableStatusType.Free, key: 'POS.FLOOR_PLAN.STATUS.FREE', cssClass: 'free' },
    { value: TableStatusType.Occupied, key: 'POS.FLOOR_PLAN.STATUS.OCCUPIED', cssClass: 'occupied' },
    { value: TableStatusType.Reserved, key: 'POS.FLOOR_PLAN.STATUS.RESERVED', cssClass: 'reserved' },
  ];

  ngOnInit(): void {
    if (this.isWaiter) {
      this.loadCurrentShift();
    } else {
      this.loadOpenShifts();
      this.loadFloorPlan();
    }
  }

  onRefresh(): void {
    if (!this.isWaiter) {
      this.loadOpenShifts();
    }
    this.loadFloorPlan();
  }

  loadOpenShifts(): void {
    this.shiftsApi.list({ paging: allItemsPaging, status: ShiftStatusType.Open }).subscribe({
      next: (response) => {
        this.openShifts = response.items;
      },
      error: (err) => {
        this.toaster.error(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load open shifts error:', err);
      },
    });
  }

  loadCurrentShift(): void {
    this.startLoading();

    this.shiftsApi.getCurrent().subscribe({
      next: (shift) => {
        this.shift = shift;
        this.isShiftLoaded = true;

        if (shift) {
          this.loadFloorPlan();
        } else {
          this.regions = [];
          this.stopLoading();
        }
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD'));
        console.error('Load current shift error:', err);
      },
    });
  }

  onOpenShift(): void {
    const data: OpenShiftDialogData = { selectWaiter: !this.isWaiter };

    const dialogRef = this.dialog.open(OpenShiftDialogComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'open-shift-dialog',
      autoFocus: true,
      disableClose: false,
      data,
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.toaster.success(this.translate.instant('SHIFTS.DIALOGS.SUCCESS_OPEN'));
        if (this.isWaiter) {
          this.loadCurrentShift();
        } else {
          this.loadOpenShifts();
        }
      }
    });
  }

  onCloseOpenShift(shift: ListShiftsQueryDto): void {
    const data: CloseShiftDialogData = {
      shiftId: shift.id,
      cashRegisterName: shift.cashRegisterName,
      userFullName: shift.userFullName,
      openedAtUtc: shift.openedAtUtc,
      openingCash: shift.openingCash,
      expectedCash: shift.expectedCash ?? shift.openingCash,
    };

    const dialogRef = this.dialog.open(CloseShiftDialogComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'close-shift-dialog',
      autoFocus: true,
      disableClose: false,
      data,
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.toaster.success(this.translate.instant('SHIFTS.DIALOGS.SUCCESS_CLOSE'));
        this.loadOpenShifts();
      }
    });
  }

  onCloseShift(): void {
    if (!this.shift) {
      return;
    }

    const data: CloseShiftDialogData = {
      shiftId: this.shift.id,
      cashRegisterName: this.shift.cashRegisterName,
      openedAtUtc: this.shift.openedAtUtc,
      openingCash: this.shift.openingCash,
      expectedCash: this.shift.expectedCash,
    };

    const dialogRef = this.dialog.open(CloseShiftDialogComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'close-shift-dialog',
      autoFocus: true,
      disableClose: false,
      data,
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.toaster.success(this.translate.instant('SHIFTS.DIALOGS.SUCCESS_CLOSE'));
        this.loadCurrentShift();
      }
    });
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
