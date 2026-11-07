import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {myAuthData, myAuthGuard} from './core/guards/my-auth-guard';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireManagement: true }),
    loadChildren: () =>
      import('./modules/admin/admin-module').then(m => m.AdminModule)
  },
  {
    path: 'pos',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireStaff: true }),
    loadChildren: () =>
      import('./modules/pos/pos-module').then(m => m.PosModule)
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: '**', redirectTo: 'auth/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
