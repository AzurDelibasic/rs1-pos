import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { ProductsComponent } from './catalogs/products/products.component';
import { ProductsAddComponent } from './catalogs/products/products-add/products-add.component';
import { ProductsEditComponent } from './catalogs/products/products-edit/products-edit.component';
import { ProductCategoriesComponent } from './catalogs/product-categories/product-categories.component';
import {MyProfileComponent} from '../account/my-profile/my-profile.component';
import {AdminSettingsComponent} from './admin-settings/admin-settings.component';
import {ExchangeRatesComponent} from './finance/exchange-rates/exchange-rates.component';
import {RegionsComponent} from './venue/regions/regions.component';
import {TablesComponent} from './venue/tables/tables.component';
import {CashRegistersComponent} from './shifts/cash-registers/cash-registers.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      // PRODUCTS
      {
        path: 'products',
        component: ProductsComponent,
      },
      {
        path: 'products/add',
        component: ProductsAddComponent,
      },
      {
        path: 'products/:id/edit',
        component: ProductsEditComponent,
      },

      // PRODUCT CATEGORIES
      {
        path: 'product-categories',
        component: ProductCategoriesComponent,
      },

      {
        path: 'regions',
        component: RegionsComponent,
      },
      {
        path: 'tables',
        component: TablesComponent,
      },
      {
        path: 'cash-registers',
        component: CashRegistersComponent,
      },

      // FINANCE
      {
        path: 'exchange-rates',
        component: ExchangeRatesComponent,
      },

      // POSTAVKE APLIKACIJE (prazna stranica – ovdje idu postavke vaše aplikacije)
      {
        path: 'settings',
        component: AdminSettingsComponent,
      },

      // MOJ PROFIL (ista stranica kao /pos/profile, deklarisana u AccountModule)
      {
        path: 'profile',
        component: MyProfileComponent,
      },

      // default admin route → /admin/products
      {
        path: '',
        redirectTo: 'products',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
