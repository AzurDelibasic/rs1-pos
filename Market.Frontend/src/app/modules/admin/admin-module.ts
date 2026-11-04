import {NgModule} from '@angular/core';

import {AdminRoutingModule} from './admin-routing-module';
import {ProductsComponent} from './catalogs/products/products.component';
import {ProductsAddComponent} from './catalogs/products/products-add/products-add.component';
import {ProductsEditComponent} from './catalogs/products/products-edit/products-edit.component';
import {AdminLayoutComponent} from './admin-layout/admin-layout.component';
import {ProductCategoriesComponent} from './catalogs/product-categories/product-categories.component';
import {
  ProductCategoryUpsertComponent
} from './catalogs/product-categories/product-category-upsert/product-category-upsert.component';
import {SharedModule} from '../shared/shared-module';
import {AccountModule} from '../account/account-module';
import { ProductImageComponent } from './catalogs/products/product-image/product-image.component';
import { ExchangeRatesComponent } from './finance/exchange-rates/exchange-rates.component';
import { AdminSettingsComponent } from './admin-settings/admin-settings.component';
import { RegionUpsertComponent } from './venue/regions/region-upsert/region-upsert.component';
import { RegionsComponent } from './venue/regions/regions.component';
import { TableUpsertComponent } from './venue/tables/table-upsert/table-upsert.component';
import { TablesComponent } from './venue/tables/tables.component';


@NgModule({
  declarations: [
    ProductsComponent,
    ProductsAddComponent,
    ProductsEditComponent,
    AdminLayoutComponent,
    ProductCategoriesComponent,
    ProductCategoryUpsertComponent,
    ProductImageComponent,
    ExchangeRatesComponent,
    AdminSettingsComponent,
    RegionUpsertComponent,
    RegionsComponent,
    TableUpsertComponent,
    TablesComponent,
  ],
  imports: [
    AdminRoutingModule,
    SharedModule,
    // "Moj profil" (/admin/profile) – ista stranica kao /pos/profile
    AccountModule,
  ]
})
export class AdminModule { }
