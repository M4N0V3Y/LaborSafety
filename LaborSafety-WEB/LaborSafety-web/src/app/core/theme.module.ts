import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';
import {
  MatNativeDateModule,
  MatIconModule,
  MatSidenavModule,
  MatListModule,
  MatToolbarModule,
  MatButtonModule,
  MatCheckboxModule,
  MatFormFieldModule,
  MatInputModule,
  MatRippleModule,
  MatCardModule,
  MatGridListModule,
  MatPaginatorModule,
  MatTableModule,
  MatProgressSpinnerModule,
  MatSelectModule,
  MatExpansionModule,
  MatTooltipModule
} from '@angular/material';
import { RouterModule } from '@angular/router';

import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { LayoutComponent } from './layout/layout/layout.component';
import { SearchInputComponent } from './layout/search-input/search-input.component';

import {
  CapitalizePipe,
  PluralPipe,
  RoundPipe,
  TimingPipe
} from '../shared/pipes';
import { SharedModule } from '../shared/shared.module';
import { ToasterModule } from 'angular2-toaster';

const MATERIAL_MODULES = [
  TextMaskModule,
  MatCheckboxModule,
  MatButtonModule,
  MatFormFieldModule,
  MatInputModule,
  MatRippleModule,
  MatCardModule,
  MatGridListModule,
  MatNativeDateModule,
  MatIconModule,
  MatSidenavModule,
  MatListModule,
  MatToolbarModule,
  MatPaginatorModule,
  MatTableModule,
  MatProgressSpinnerModule,
  MatSelectModule,
  MatTooltipModule,
  MatExpansionModule
];

const BASE_MODULES = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  RouterModule,
  ToasterModule
];

const COMPONENTS = [
  HeaderComponent,
  FooterComponent,
  SearchInputComponent,
  LayoutComponent
];

const PIPES = [CapitalizePipe, PluralPipe, RoundPipe, TimingPipe];

@NgModule({
  imports: [...BASE_MODULES, ...MATERIAL_MODULES, SharedModule],
  exports: [
    ...BASE_MODULES,
    ...MATERIAL_MODULES,
    ...COMPONENTS,
    ...PIPES,
    SharedModule
  ],
  declarations: [...COMPONENTS, ...PIPES]
})
export class ThemeModule {
  static forRoot(): ModuleWithProviders {
    return <ModuleWithProviders>{
      ngModule: ThemeModule
    };
  }
}
