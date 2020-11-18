import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { BrowserErrorInternalComponent } from './browser-error-internal.component';

const app_route: Routes = [
  {
    path: '**',
    component: BrowserErrorInternalComponent,
    data: { title: 'Navegador Incompatível' }
  }
];

@NgModule({
  declarations: [BrowserErrorInternalComponent],
  exports: [RouterModule, BrowserErrorInternalComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(app_route)
  ]
})
export class BrowserErrorModule { }
