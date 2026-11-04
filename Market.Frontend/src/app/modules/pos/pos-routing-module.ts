import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PosLayoutComponent } from './pos-layout/pos-layout.component';
import { FloorPlanComponent } from './floor-plan/floor-plan.component';
import { MyProfileComponent } from '../account/my-profile/my-profile.component';

const routes: Routes = [
  {
    path: '',
    component: PosLayoutComponent,
    children: [
      {
        path: 'floor-plan',
        component: FloorPlanComponent,
      },
      {
        path: 'profile',
        component: MyProfileComponent,
      },
      {
        path: '',
        redirectTo: 'floor-plan',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PosRoutingModule {}
