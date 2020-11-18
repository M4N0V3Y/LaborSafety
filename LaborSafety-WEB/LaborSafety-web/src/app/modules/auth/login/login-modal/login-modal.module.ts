import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from 'src/app/core/theme.module';
import { LoginModalComponent } from './login-modal.component';
import { MatDialogModule } from '@angular/material';
import { LoginInternalModule } from '../login-internal/login-internal.module';

@NgModule({
  imports: [
    CommonModule,
    ThemeModule,
    MatDialogModule,
    LoginInternalModule
  ],
  entryComponents: [LoginModalComponent],
  declarations: [LoginModalComponent],
  providers: []
})
export class LoginModalModule { }
