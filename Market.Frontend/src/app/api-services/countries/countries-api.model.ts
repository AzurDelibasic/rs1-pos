import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export class ListCountriesRequest extends BasePagedQuery {
  search?: string | null;
}

export interface ListCountriesQueryDto {
  id: number;
  name: string;
  code: string;
  vatRate: number;
}

export interface GetCountryByIdQueryDto {
  id: number;
  name: string;
  code: string;
  vatRate: number;
}

export type ListCountriesResponse = PageResult<ListCountriesQueryDto>;

export interface CreateCountryCommand {
  name: string;
  code: string;
  vatRate: number;
}

export interface UpdateCountryCommand {
  name: string;
  code: string;
  vatRate: number;
}
