import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ClientComponent } from './client.component';
import { ClientAuthGuard } from '../../core/guards/client-auth.guard';
import { GestaoInvAtividadeComponent } from './inventario-atividade/gestao-inv-atividade/gestao-inv-atividade.component';
import { GestaoPerfilComponent } from './perfil/gestao-perfil.component';
import { CadastroInvAtividadeComponent } from './inventario-atividade/cadastro-inv-atividade/cadastro-inv-atividade.component';
import {GestaoInvAmbienteComponent} from './inventario-ambiente/gestao-inv-ambiente/gestao-inv-ambiente.component';
import {ImportacaoComponent} from 'src/app/modules/client/importacao/importacao.component';
import { CadastroInvAmbienteComponent } from './inventario-ambiente/cadastro-inv-ambiente/cadastro-inv-ambiente.component';
import { GestaoAprptComponent } from './APR/gestao-aprpt/gestao-aprpt.component';
import { CadastroAprptComponent } from './APR/cadastro-aprpt/cadastro-aprpt.component';
import { ImpressaoMassaComponent } from './impressao-massa/impressao-massa.component';
import { MapaBloqueioComponent } from './mapa-bloqueio/mapa-bloqueio.component';


export const routes: Routes = [
  {
    path: '',
    component: ClientComponent,
    /*canActivate: [ClientAuthGuard],
    canActivateChild: [ClientAuthGuard],*/
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        data: { title: 'DashBoard' },
      },
      {
        path: 'perfil',
        component: GestaoPerfilComponent,
        data: { title: 'Gestão de Perfis' }
      },
      {
        path: 'inventario-atividade',
        component: GestaoInvAtividadeComponent,
        data: { title: 'Gestão de Inventário de Atividade' }
      },
      {
        path: 'inventario-ambiente',
        component: GestaoInvAmbienteComponent,
        data: { title: 'Gestão de Inventário de Ambiente' }
      },
      {
        path: 'inventario-atividade/cadastro',
        component: CadastroInvAtividadeComponent,
        data: { title: 'Cadastro de Inventário de Atividade'}
      },
      {
        path: 'inventario-ambiente/cadastro',
        component: CadastroInvAmbienteComponent,
        data: { title: 'Cadastro de Inventário de Ambiente'}
      },
      {
        path: 'usuarios',
        loadChildren: './usuarios/usuarios.module#UsuariosModule',
        data: { title: 'Usuarios' }
      },
      {
        path: 'importacao',
        component: ImportacaoComponent,
        data: { title: 'Importação' }
      },
      {
        path: 'aprpt',
        component: GestaoAprptComponent,
        data: { title: 'APR' }
      },
      {
        path: 'aprpt/cadastro',
        component: CadastroAprptComponent,
        data: { title: 'Nova APR' }
      },
      {
        path: 'impressao',
        component: ImpressaoMassaComponent,
        data: { title: 'Impressão' },
      },
      {
        path: 'mapa',
        component: MapaBloqueioComponent,
        data: { title: 'Mapa de Bloqueio' },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule {
  static components = [DashboardComponent, ClientComponent];
}
