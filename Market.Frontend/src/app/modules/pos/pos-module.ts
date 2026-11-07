import { NgModule } from '@angular/core';

import { PosRoutingModule } from './pos-routing-module';
import { SharedModule } from '../shared/shared-module';
import { AccountModule } from '../account/account-module';
import { PosLayoutComponent } from './pos-layout/pos-layout.component';
import { FloorPlanComponent } from './floor-plan/floor-plan.component';
import { OpenShiftDialogComponent } from './open-shift-dialog/open-shift-dialog.component';

@NgModule({
  declarations: [
    PosLayoutComponent,
    FloorPlanComponent,
    OpenShiftDialogComponent,
  ],
  imports: [
    PosRoutingModule,
    SharedModule,
    AccountModule,
  ]
})
export class PosModule { }
