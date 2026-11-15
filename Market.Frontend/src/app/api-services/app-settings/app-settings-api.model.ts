export enum RoundingModeType {
  None = 1,
  NearestFiveCents = 2,
  NearestTenCents = 3,
  NearestWhole = 4,
}

export interface GetAppSettingsQueryDto {
  id: number;
  businessName: string;
  address: string | null;
  taxNumber: string | null;
  countryId: number;
  countryName: string;
  vatRate: number;
  defaultCurrencyCode: string;
  roundingMode: RoundingModeType;
  reservationHoldMinutes: number;
}

export interface UpdateAppSettingsCommand {
  businessName: string;
  address: string | null;
  taxNumber: string | null;
  countryId: number;
  defaultCurrencyCode: string;
  roundingMode: RoundingModeType;
  reservationHoldMinutes: number;
}
