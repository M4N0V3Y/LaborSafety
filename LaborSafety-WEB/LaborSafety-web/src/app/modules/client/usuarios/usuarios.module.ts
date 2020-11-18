import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsuariosRoutingModule } from './usuarios-routing.module';
import { ListagemUsuariosComponent } from './listagem-usuarios/listagem-usuarios.component';
import { ThemeModule } from 'src/app/core/theme.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ListagemUsuariosComponent],
  imports: [
    CommonModule,
    UsuariosRoutingModule,
    ThemeModule,
    HttpClientModule, 
    FormsModule
  ],
  exports: [ListagemUsuariosComponent]
})
export class UsuariosModule { }
