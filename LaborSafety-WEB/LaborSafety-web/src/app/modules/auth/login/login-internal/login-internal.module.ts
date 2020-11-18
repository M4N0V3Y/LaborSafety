import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from 'src/app/core/theme.module';
import { MatDialogModule } from '@angular/material';
import { LoginInternalComponent } from './login-internal.component';
import {MatButtonModule} from '@angular/material/button';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';

@NgModule({
  imports: [
    CommonModule,
    ThemeModule,
  ],
  entryComponents: [LoginInternalComponent, PopupComponent],
  declarations: [LoginInternalComponent],
  exports: [LoginInternalComponent],
  providers: [],
})
export class LoginInternalModule { }
