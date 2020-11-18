import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnlynumbersDirective } from 'src/app/shared/directives/onlynumbers.directive';

@NgModule({
  declarations: [OnlynumbersDirective],
  imports: [
    CommonModule
  ],
  exports: [OnlynumbersDirective]
})
export class OnlynumbersModule { }
