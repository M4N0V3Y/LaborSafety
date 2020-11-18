import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from 'src/app/core/theme.module';
import { LoginComponent } from './login.component';
import { LoginInternalModule } from './login-internal/login-internal.module';

@NgModule({
  imports: [
    CommonModule,
    ThemeModule,
    LoginInternalModule
  ],
  declarations: [LoginComponent],
  providers: []
})
export class LoginModule { }
