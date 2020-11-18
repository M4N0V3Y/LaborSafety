import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListagemUsuariosComponent } from './listagem-usuarios/listagem-usuarios.component';

const routes: Routes = [
  {
    path: 'listagem',
    component: ListagemUsuariosComponent
  },
  {
    path: '',
    redirectTo: 'listagem'
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsuariosRoutingModule { 
  static components = [ListagemUsuariosComponent];
}
