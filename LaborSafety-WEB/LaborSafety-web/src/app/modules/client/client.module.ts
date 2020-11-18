import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientRoutingModule } from './client-routing.module';
import { ThemeModule } from '../../core/theme.module';
import { ClientAuthGuard } from '../../core/guards/client-auth.guard';
import { MatDatepickerModule, MatPaginatorIntl, MatDialogModule } from '@angular/material';
import { Factory } from 'src/app/shared/util/factory';
import { FileInputObserverModule } from 'src/app/shared/directives/file-input/file-input-accessor.module';
import { TextMaskModule } from 'angular2-text-mask';
import { MessageDivBoxModule } from 'src/app/shared/components/message-div-box/message-div-box.module';
import { LoginModalModule } from '../auth/login/login-modal/login-modal.module';
import { UsuariosModule } from './usuarios/usuarios.module';
import { RiscoComponent } from './risco/risco.component';
import { TreeViewComponent } from './treeview/treeview.component';
import { GestaoInvAtividadeComponent } from './inventario-atividade/gestao-inv-atividade/gestao-inv-atividade.component';
import { CadastroInvAtividadeComponent } from './inventario-atividade/cadastro-inv-atividade/cadastro-inv-atividade.component';
import { MatTreeModule } from '@angular/material/tree';
import { GestaoPerfilComponent } from './perfil/gestao-perfil.component';
import { GestaoInvAmbienteComponent } from './inventario-ambiente/gestao-inv-ambiente/gestao-inv-ambiente.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import { ImportacaoComponent } from './importacao/importacao.component';
import {MatTableModule} from '@angular/material/table';
import {NgxPrintModule} from 'ngx-print';
import {ScrollingModule} from '@angular/cdk/scrolling';
import {MatRadioModule} from '@angular/material/radio';
import { CadastroInvAmbienteComponent } from './inventario-ambiente/cadastro-inv-ambiente/cadastro-inv-ambiente.component';
import { GestaoAprptComponent } from './APR/gestao-aprpt/gestao-aprpt.component';
import { CadastroAprptComponent } from './APR/cadastro-aprpt/cadastro-aprpt.component';
import { OperacaoComponent } from './operacao/operacao.component';
import { PopUpPessoasComponent } from './pop-up-pessoas/pop-up-pessoas.component';
import { TreeviewdoisComponent, ChecklistDatabase } from './treeviewdois/treeviewdois.component';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { TreeviewEPIComponent, ChecklistDatabaseEPI } from './treeview-epi/treeview-epi.component';
import { TreeviewLIComponent } from './treeview-li/treeview-li.component';
import { getPortuguesePaginator } from './client-paginator';
import { ImpressaoMassaComponent } from './impressao-massa/impressao-massa.component';
import { MapaBloqueioComponent } from './mapa-bloqueio/mapa-bloqueio.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ValidarCpf } from 'src/app/shared/util/validatorCPF';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { ListaEpiComponent } from './lista-epi/lista-epi.component';
import { TreeviewPerformanceComponent } from './treeview-performance/treeview-performance.component';

@NgModule({
  imports: [
    ThemeModule,
    MatDatepickerModule,
    CommonModule,
    ClientRoutingModule,
    UsuariosModule,
    FileInputObserverModule,
    TextMaskModule,
    MessageDivBoxModule,
    LoginModalModule,
    MatDialogModule,
    MatTreeModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatCheckboxModule,
    MatCardModule,
    MatButtonModule,
    ScrollingModule,
    MatRadioModule,
    MatTableModule,
    NgxPrintModule,
    MatProgressBarModule,
    NgxMaskModule.forRoot(),
  ],
  declarations: [
    ClientRoutingModule.components,
    RiscoComponent,
    TreeViewComponent,
    GestaoInvAtividadeComponent,
    CadastroInvAtividadeComponent,
    GestaoPerfilComponent,
    GestaoInvAmbienteComponent,
    ImportacaoComponent,
    CadastroInvAmbienteComponent,
    GestaoAprptComponent,
    CadastroAprptComponent,
    OperacaoComponent,
    PopUpPessoasComponent,
    TreeviewdoisComponent,
    TreeviewEPIComponent,
    TreeviewLIComponent,
    ImpressaoMassaComponent,
    MapaBloqueioComponent,
    ListaEpiComponent,
    TreeviewPerformanceComponent
  ],
  providers: [
    ClientAuthGuard,
    Factory,
    ChecklistDatabaseEPI,
    ChecklistDatabase,
    ValidarCpf,
    { provide: MatPaginatorIntl, useValue: getPortuguesePaginator() }
  ],
  exports: [TextMaskModule],
  entryComponents: [RiscoComponent, TreeViewComponent, OperacaoComponent, PopUpPessoasComponent, PopupComponent]
})
export class ClientModule { }
