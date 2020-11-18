import { NgModule } from '@angular/core';
import {
  MatButtonModule,
  MatDialogModule,
  MatFormFieldModule,
  MatIconModule,
  MatInputModule,
  MatToolbarModule,
  MatTooltipModule
} from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalAlertaComponent } from './modal-alerta.component';

@NgModule({
  entryComponents: [
    ModalAlertaComponent
  ],
  declarations: [
    ModalAlertaComponent,
  ],
  imports: [
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatToolbarModule,
    MatTooltipModule,
    MatDialogModule,
  ],
  exports: [

  ]
})
export class ModalAlertaModule {
}

