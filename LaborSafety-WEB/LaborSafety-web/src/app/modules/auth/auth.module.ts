import { NgModule } from '@angular/core';
import { CommonModule, APP_BASE_HREF } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { ThemeModule } from '../../core/theme.module';
import { LoginInternalModule } from './login/login-internal/login-internal.module';
import { LoginModule } from './login/login.module';

@NgModule({
  imports: [
    CommonModule,
    AuthRoutingModule,
    ThemeModule,
    LoginInternalModule,
    LoginModule
  ],
  declarations: [AuthRoutingModule.components],
  entryComponents: [AuthRoutingModule.entryComponents],
  providers: [{ provide: APP_BASE_HREF, useValue: window['base-href'] }]
})
export class AuthModule { }
