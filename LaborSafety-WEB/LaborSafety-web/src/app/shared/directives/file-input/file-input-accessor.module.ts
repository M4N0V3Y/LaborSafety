import { FileInputValueAccessorDirective } from './file-input-accessor.directive';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [
    FileInputValueAccessorDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    FileInputValueAccessorDirective
  ]
})
export class FileInputObserverModule { 
}
