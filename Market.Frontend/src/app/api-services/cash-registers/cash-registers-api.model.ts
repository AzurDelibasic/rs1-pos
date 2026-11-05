import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export class ListCashRegistersRequest extends BasePagedQuery {
  search?: string | null;
  onlyActive?: boolean | null;
}

export interface ListCashRegistersQueryDto {
  id: number;
  name: string;
  isActive: boolean;
}

export interface GetCashRegisterByIdQueryDto {
  id: number;
  name: string;
  isActive: boolean;
}

export type ListCashRegistersResponse = PageResult<ListCashRegistersQueryDto>;

export interface CreateCashRegisterCommand {
  name: string;
  isActive: boolean;
}

export interface UpdateCashRegisterCommand {
  name: string;
  isActive: boolean;
}