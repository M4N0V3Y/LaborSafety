import { Component, OnInit, ViewChild, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { InventarioAtividadeModelo } from 'src/app/shared/models/InventarioAtividadeModelo';
import { TreeViewComponent } from 'src/app/modules/client/treeview/treeview.component';
import { Router } from '@angular/router';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { PesoFisico } from 'src/app/shared/models/pesofisico.model';
import { DisciplinaService } from 'src/app/core/http/disciplina/disciplina.service';
import { AtividadeService } from 'src/app/core/http/atividade/atividade.service';
import { PesoService } from 'src/app/core/http/pesofisico/peso.service';
import { PerfilDeCatalogo } from 'src/app/shared/models/perfildecatalogo.model';
import { PerfilDeCatalogoService } from 'src/app/core/http/perfildecatalogo/perfildecatalogo.service';
import { GestaoAtividadeInventarioService } from './gestao-inv-atividade.service';
import { Risco } from 'src/app/shared/models/risco.model';
import { filtrosModel } from 'src/app/shared/models/filtrosModel';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { Tela } from 'src/app/shared/models/tela.model';
import { TreeviewdoisComponent } from '../../treeviewdois/treeviewdois.component';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';

export class FiltroInventarioAtividade {

  public Riscos: number[];
  public Locais: number[];
  public CodPeso: number;
  public CodPerfilCatalogo: number;
  public CodAtividade: number;
  public CodDisciplina: number;
  public CodSeveridade: number;
  public CodProbabilidade: number;

  constructor(r: number[], l: number[], cp: number, cpc: number, ca: number, cd: number, cs: number, cprob: number) {
    this.Riscos = r;
    this.Locais = l;
    this.CodPeso = cp;
    this.CodPerfilCatalogo = cpc;
    this.CodAtividade = ca;
    this.CodDisciplina = cd;
    this.CodSeveridade = cs;
    this.CodProbabilidade = cprob;
  }
  setLocais(locais: any[]) {
    const model: any[] = [];
    locais.forEach((local) => {
      model.push(local.CodLocalInstalacao);
    });
    this.Locais = [...model];
  }
}

@Component({
  selector: 'app-gestao-inv-atividade',
  templateUrl: './gestao-inv-atividade.component.html',
  styleUrls: ['./gestao-inv-atividade.component.scss'],
  encapsulation: ViewEncapsulation.None,
  // providers: [GestaoAtividadeInventarioService]
})

export class GestaoInvAtividadeComponent implements OnInit {

  constructor(private changeDetector: ChangeDetectorRef, private router: Router, private gestaoService: GestaoAtividadeInventarioService,
              public atividadeService: AtividadeService, public disciplinaService: DisciplinaService,
              public pesoService: PesoService, public perfilDeCatalogoService: PerfilDeCatalogoService,
              private auth: AuthenticationService) {
                if (!this.verificaLogin()) {
                  return;
                }
                this.inventarioDataSource = new MatTableDataSource<InventarioAtividadeModelo>();
               }
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;
  @ViewChild(TreeviewdoisComponent) treeViewDois: TreeviewdoisComponent;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  desabilitarButtonCadastro = true;
  public dataSource: any;
  public nodosSelecionados: any;
  public listaTelasEFuncionalidadesPorPerfil: Tela[];
  public disciplinaControl = new FormControl('', [Validators.required]);
  public pesoControl = new FormControl('', [Validators.required]);
  public atividadeControl = new FormControl('', [Validators.required]);
  public perfilControl = new FormControl('', [Validators.required]);
  public selectFormControl = new FormControl('', Validators.required);
  public atividades: Atividade[] = [];
  public disciplinas: Disciplina[] = [];
  public pesos: PesoFisico[] = [];
  public perfis: PerfilDeCatalogo[] = [];
  public disciplinaSelecionada: Disciplina;
  public atividadeSelecionada: Atividade;
  public pesoSelecionado: PesoFisico;
  public perfilSelecionado: PerfilDeCatalogo;
  public availableProfiles: PerfilDeCatalogo[] = [];
  public filtro = new FiltroInventarioAtividade([], [], 0, 0, 0, 0, 0, 0);
  public inventarioDataSource: MatTableDataSource<InventarioAtividadeModelo>;
    testesColunas: string[] = ['coluna1', 'coluna2'];
    isRascunho: boolean;

  public treeviewEnabled = true;

  reloadTree() {
    this.treeviewEnabled = false;
    this.changeDetector.detectChanges();
    this.treeviewEnabled = true;
  }

  ngOnInit() {
    this.disciplinaService.getAll().then(
      (discips) => { this.disciplinas = discips; }
    );

    this.atividadeService.getAll().then(
      (atvs) => { this.atividades = atvs; }
    );

    this.pesoService.getAll().then(
      (pesos) => { this.pesos = pesos; }
    );

    this.perfilDeCatalogoService.getAll().then(
      (perfis) => { this.perfis = perfis;
                    this.availableProfiles = perfis;
                    document.getElementsByName('selectperf')[0].blur();
                  }
    );

    this.atividadeService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
    .then((lista: Tela[]) => {
      this.listaTelasEFuncionalidadesPorPerfil = [...lista];
      if (this.verificarPermissaoDeTela() !== true) {
        this.router.navigate(['/notfound']);
      }
    }).catch(() => {
      this.router.navigate(['/auth/login']);
    });
  }
  verificaLogin() {
    if (!this.auth.isLogin()) {
        this.router.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }
  verificarPermissaoDeTela() {
    let permissao = false;
    if (this.listaTelasEFuncionalidadesPorPerfil) {
        this.listaTelasEFuncionalidadesPorPerfil.forEach(
        (tela) => {
          if (3 === tela.CodTela) {
            permissao = true;
            tela.Funcionalidades.forEach((funcionalidade) => {
              this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
            });
          }
        });
    }
    return permissao;
  }
  verificaFuncionalidades(codFuncionalidade) {
    switch (codFuncionalidade) {
      case 1:
        this.desabilitarButtonCadastro = false;
        break;
      case 2:
        break;
      case 3:
        break;
      case 4:
        break;
      case 5:
        break;
    }
  }

  gerarTodosLogs() {
    this.atividadeService.gerarTodosLogs().then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_inventario_atividade_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }
  redirecionarTelaCadastro(): void {
    this.router.navigate(['/client/inventario-atividade/cadastro']);
  }

  editar(inventario: InventarioAtividadeModelo) {
    let model: { CodRascunhoInventarioAtividade?: number; isRascunho?: boolean; CodInventarioAtividade?: number; };
    if (this.isRascunho) {
       model = {
        CodRascunhoInventarioAtividade: inventario.CodRascunhoInventarioAtividade,
        isRascunho: true,
      };
       inventario.isRascunho = true;
    } else {
      model = {
        CodInventarioAtividade: inventario.CodInventarioAtividade,
      };
    }
    this.router.navigate(['/client/inventario-atividade/cadastro'], {
      queryParams: model
    });
  }

  entrarDadosTreeView() {
    if (this.dataSource !== undefined) {
      return this.dataSource;
    }
  }

  retornaSelecionados(selecionados) {
    this.nodosSelecionados = selecionados;
  }
  limparPesquisa() {
    if (this.isRascunho) {
      this.filtro = new FiltroInventarioAtividade([], [], 0, 0, 0, 0, 0, 0);
    }
    this.inventarioDataSource.data = [];
    this.inventarioDataSource.paginator = this.paginator;
  }

  pesquisar() {
    if (!this.isRascunho) {
      this.filtro.CodPeso = this.pesoSelecionado ? this.pesoSelecionado.CodPeso : 0;
      this.filtro.CodPerfilCatalogo = this.perfilSelecionado ? this.perfilSelecionado.CodPerfilCatalogo : 0;
      this.filtro.CodAtividade = this.atividadeSelecionada ? this.atividadeSelecionada.CodAtividadePadrao : 0;
      this.filtro.CodDisciplina = this.disciplinaSelecionada ? this.disciplinaSelecionada.CodDisciplina : 0;
      this.filtro.CodSeveridade = 0;
      this.filtro.CodProbabilidade = 0;
      this.filtro.setLocais(this.nodosSelecionados);
      this.gestaoService.pesquisarInventario(this.filtro).then(
        (data: any) => {
          this.inventarioDataSource.data = [...data];
          this.inventarioDataSource.paginator = this.paginator;
        }
      ).catch(() => {
        this.limparPesquisa();
      });
    } else {
      this.filtro.CodPeso = this.pesoSelecionado ? this.pesoSelecionado.CodPeso : 0;
      this.filtro.CodPerfilCatalogo = this.perfilSelecionado ? this.perfilSelecionado.CodPerfilCatalogo : 0;
      this.filtro.CodAtividade = this.atividadeSelecionada ? this.atividadeSelecionada.CodAtividadePadrao : 0;
      this.filtro.CodDisciplina = this.disciplinaSelecionada ? this.disciplinaSelecionada.CodDisciplina : 0;
      this.filtro.CodSeveridade = 0;
      this.filtro.CodProbabilidade = 0;
      this.filtro.setLocais(this.nodosSelecionados);
      this.gestaoService.pesquisarInventarioRascunho(this.filtro).then((data) => {
        this.inventarioDataSource.data = [...data];
        this.inventarioDataSource.paginator = this.paginator;
      });
    }

  }

  selectFilters(selecao, tipoSelecao) {
    switch (tipoSelecao) {
      case 'disciplina':
        this.disciplinaSelecionada = selecao.value;
        break;

        case 'atividade':
        this.atividadeSelecionada = selecao.value;
        break;

        case 'perfil':
        this.perfilSelecionado = selecao.value;
        this.reloadTree();
        break;

        case 'peso':
        this.pesoSelecionado = selecao.value;
        /*CÓDIGO REMOVIDO POR PREFERÊNCIA DO CLIENTE EM NÃO ENVIAR OS DADOS DE PERFIL E PESO
        FUNCIONALIDADE RESERVADA PARA UM POSSÍVEL FUTURO UPDATE

        IF REGRA DE PESO E PERFIL
        this.reloadTree(); */

        break;
    }
  }

  perfilOnKey(value: any) {
    if (!value || value === '') {
      this.availableProfiles = this.perfis;
    } else {
      const search = value.toLowerCase();
      this.availableProfiles = this.perfis.filter((perf) => (perf.Codigo.toLowerCase() + ' - ' +
                                                  perf.Nome.toLowerCase()).includes(search));
    }
  }
}




