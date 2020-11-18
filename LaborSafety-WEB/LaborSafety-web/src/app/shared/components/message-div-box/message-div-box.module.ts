import { NgModule } from '@angular/core';
import { MessageDivBoxComponent } from './message-div-box.component';
import { MatIconModule } from '@angular/material';

@NgModule({
  imports: [
    MatIconModule
  ],
  entryComponents: [
    MessageDivBoxComponent
  ],
  declarations: [
    MessageDivBoxComponent
  ],
  exports: [
    MessageDivBoxComponent
  ]
})
export class MessageDivBoxModule {
}
