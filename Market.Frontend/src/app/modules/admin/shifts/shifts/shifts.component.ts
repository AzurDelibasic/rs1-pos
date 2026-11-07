import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { ShiftsApiService } from '../../../../api-services/shifts/shifts-api.service';
import { CashRegistersApiService } from '../../../../api-services/cash-registers/cash-registers-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import {
  ListShiftsQueryDto,
  ListShiftsRequest,
  ShiftStatusType,
} from '../../../../api-services/shifts/shifts-api.model';
import { ListCashRegistersQueryDto } from '../../../../api-services/cash-registers/cash-registers-api.model';
import { allItemsPaging } from '../../../../core/models/paging/paging-utils';
import { toUtcDateIso } from '../../../../core/forms/utc-date';
import {
  CloseShiftDialogComponent,
  CloseShiftDialogData,
} from '../../../shared/components/close-shift-dialog/close-shift-dialog.component';

@Component({
  selector: 'app-shifts',
  standalone: false,
  templateUrl: './shifts.component.html',
  styleUrl: './shifts.component.scss',
})
export class ShiftsComponent
  extends BaseListPagedComponent<ListShiftsQueryDto, ListShiftsRequest>
  implements OnInit
{
  private api = inject(ShiftsApiService);
  private cashRegistersApi = inject(CashRegistersApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private translate = inject(TranslateService);
  private destroyRef = inject(DestroyRef);

  displayedColumns: string[] = [
    'openedAtUtc',
    'closedAtUtc',
    'cashRegisterName',
    'userFullName',
    'openingCash',
    'expectedCash',
    'countedCash',
    'difference',
    'status',
    'actions',
  ];
  cashRegisters: ListCashRegistersQueryDto[] = [];
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  readonly ShiftStatusType = ShiftStatusType;
  readonly statuses = [
    { value: ShiftStatusType.Open, key: 'SHIFTS.STATUS.OPEN' },
    { value: ShiftStatusType.Closed, key: 'SHIFTS.STATUS.CLOSED' },
  ];

  constructor() {
    super();
    this.request = new ListShiftsRequest();
  }

  ngOnInit(): void {
    this.loadCashRegisters();
    this.initList();
  }

  private loadCashRegisters(): void {
    this.cashRegistersApi
      .list({ paging: allItemsPaging })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.cashRegisters = response.items;
        },
        error: (err) => {
          this.toaster.error(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD_CASH_REGISTERS'));
          console.error('Load cash registers error:', err);
        }
      });
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading(this.translate.instant('SHIFTS.DIALOGS.ERROR_LOAD_LIST'));
        console.error('Load shifts error:', err);
      },
    });
  }

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onCashRegisterFilterChange(cashRegisterId: number | null): void {
    this.request.cashRegisterId = cashRegisterId;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onStatusFilterChange(status: ShiftStatusType | null): void {
    this.request.status = status;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onDateFilterChange(): void {
    this.request.dateFromUtc = toUtcDateIso(this.dateFrom);
    this.request.dateToUtc = toUtcDateIso(this.dateTo);
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  hasFilters(): boolean {
    return !!(
      this.request.search ||
      this.request.cashRegisterId ||
      this.request.status ||
      this.request.dateFromUtc ||
      this.request.dateToUtc
    );
  }

  statusKey(status: ShiftStatusType): string {
    return this.statuses.find(s => s.value === status)?.key ?? '';
  }

  onClose(shift: ListShiftsQueryDto): void {
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
        this.loadPagedData();
      }
    });
  }
}
