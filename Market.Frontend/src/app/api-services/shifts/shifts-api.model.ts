import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export enum ShiftStatusType {
  Open = 1,
  Closed = 2,
}

export class ListShiftsRequest extends BasePagedQuery {
  search?: string | null;
  cashRegisterId?: number | null;
  status?: ShiftStatusType | null;
  dateFromUtc?: string | null;
  dateToUtc?: string | null;
}

export interface ListShiftsQueryDto {
  id: number;
  cashRegisterName: string;
  userFullName: string;
  openedAtUtc: string;
  closedAtUtc: string | null;
  openingCash: number;
  expectedCash: number | null;
  countedCash: number | null;
  difference: number | null;
  status: ShiftStatusType;
}

export type ListShiftsResponse = PageResult<ListShiftsQueryDto>;

export interface GetCurrentShiftQueryDto {
  id: number;
  cashRegisterId: number;
  cashRegisterName: string;
  openedAtUtc: string;
  openingCash: number;
  expectedCash: number;
}

export class ListWaitersRequest extends BasePagedQuery {
  search?: string | null;
}

export interface ListWaitersQueryDto {
  id: number;
  fullName: string;
  hasOpenShift: boolean;
}

export type ListWaitersResponse = PageResult<ListWaitersQueryDto>;

export interface OpenShiftCommand {
  cashRegisterId: number;
  openingCash: number;
  userId: number | null;
}

export interface CloseShiftCommand {
  countedCash: number;
}
