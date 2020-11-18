import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import {  MatTable } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import { RiscoMapeadoModelo } from 'src/app/shared/models/riscomapeado.model';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { Duracao } from 'src/app/shared/models/duracao.model';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { PesoFisico } from 'src/app/shared/models/pesofisico.model';
import { DisciplinaService } from 'src/app/core/http/disciplina/disciplina.service';
import { AtividadeService } from 'src/app/core/http/atividade/atividade.service';
import { DuracaoService } from 'src/app/core/http/duracao/duracao.service';
import { PesoService } from 'src/app/core/http/pesofisico/peso.service';
import { PerfilDeCatalogo } from 'src/app/shared/models/perfildecatalogo.model';
import { PerfilDeCatalogoService } from 'src/app/core/http/perfildecatalogo/perfildecatalogo.service';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { RiscoComponent } from '../../risco/risco.component';
import { GestaoAtividadeInventarioService } from '../gestao-inv-atividade/gestao-inv-atividade.service';
import { TreeViewComponent } from '../../treeview/treeview.component';
import { TipoRiscoService } from 'src/app/core/http/tiporisco/tiporisco.service';
import { TipoRisco } from 'src/app/shared/models/tiporisco.model';
import { InventarioAtividadeModelo } from 'src/app/shared/models/InventarioAtividadeModelo';
import { RiscoInventarioAtividade } from 'src/app/shared/models/riscoinventarioatividade.model';
import { SeveridadeService } from 'src/app/core/http/severidade/severidade.service';
import { RiscoService } from 'src/app/core/http/risco/risco.service';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Tela } from 'src/app/shared/models/tela.model';
import { LocalInstalacaoInventarioAtividade } from 'src/app/shared/models/localinstalacaoinventarioatividade.model';
import {  InventarioAtividadeRascunho,
          LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE,
          RISCO_RASCUNHO_INVENTARIO_ATIVIDADE } from 'src/app/shared/models/InventarioAtividadeRascunhoModel';
import { InventarioAmbienteRascunho } from 'src/app/shared/models/InventarioAmbienteRascunhoModel';
import { EpiRascunhoModelo, EpiRascunhoAtividadeModelo } from 'src/app/shared/models/epiRascunhoModel';
import { EPI } from 'src/app/shared/models/EPI.model';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { async } from '@angular/core/testing';

@Component({
  selector: 'app-cadastro-inv-atividade',
  templateUrl: './cadastro-inv-atividade.component.html',
  styleUrls: ['./cadastro-inv-atividade.component.scss']
})
export class CadastroInvAtividadeComponent implements OnInit {
  desabilitarButtonExcluir = true;
  desabilitarButtonEdicao = true;
  desabilitarButtonImpressao = true;
  desabilitarDesassociacaoTreeview = true;
  treeviewEnabled = true;
  constructor(private routerVariable: Router, private route: ActivatedRoute, private dialog: MatDialog,
              public atividadeService: AtividadeService, public duracaoService: DuracaoService,
              public disciplinaService: DisciplinaService, public pesoService: PesoService,
              public perfilDeCatalogoService: PerfilDeCatalogoService, public gestaoService: GestaoAtividadeInventarioService,
              public tipoRiscoService: TipoRiscoService, public riscoService: RiscoService, public severidadeService: SeveridadeService,
              private changeDetector: ChangeDetectorRef, private auth: AuthenticationService) {
                if (!this.verificaLogin()) {
                  return;
                }

  }
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;
  @ViewChild('tableFisico') table1: MatTable<any>;
  @ViewChild('tableQuimico') table2: MatTable<any>;
  @ViewChild('tableBiologico') table3: MatTable<any>;
  @ViewChild('tableErgonomico') table4: MatTable<any>;
  @ViewChild('tableAcidente') table5: MatTable<any>;
  @ViewChild('tableOutros') table6: MatTable<any>;

  displayedColumns: string[] = ['Risco', 'Severidade', 'Grau', 'Controles'];
  inventarioRecebido: InventarioAtividadeModelo;
  inventarioRascunhoRecebido: InventarioAtividadeRascunho;
  dataSource: any;
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  atividades: Atividade[] = [];
  atividadeSelecionada: Atividade;
  duracoes: Duracao[] = [];
  duracaoSelecionada: Duracao;
  testeDisciplina: Disciplina;
  disciplinas: Disciplina[] = [];
  disciplinaSelecionada: Disciplina;
  pesos: PesoFisico[] = [];
  pesoSelecionado: PesoFisico;
  perfis: PerfilDeCatalogo[] = [];
  perfilSelecionado: PerfilDeCatalogo;
  tiposRisco: TipoRisco[] = [];
  tipoRiscoSelecionado: TipoRisco;
  riscoGeral = [];
  todosRiscosMapeados: Array<RiscoMapeadoModelo[]> = [];
  descricaotextArea = '';
  observacoestextArea = '';
  isCadastro: boolean;
  riscomapeado: RiscoMapeadoModelo;
  inventarioAtualSalvo: InventarioAtividadeModelo;
  flagRiscos = false;
  flagEdicao = false;
  enviado = false;
  auxInventario: InventarioAtividadeModelo;
  public nodosSelecionados: any;
  expanded = false;
  nodosSelecionadosTreeview: string[] = [];
  treeviewLoader: boolean;
  maiorRiscoGeral;
  isRascunho: boolean;
  inventarioRascunho: InventarioAtividadeRascunho;
  textoBtn = 'SALVAR COMO INVENTARIO';
  textoTitle = 'Cadastro de inventario de Atividade';
  isOperational = true;

  availableProfiles: PerfilDeCatalogo[] = [];

  async ngOnInit() {

    // tslint:disable-next-line:max-line-length
    this.inventarioAtualSalvo = new InventarioAtividadeModelo(0, '', 0, 0, 0, 0, 0, '', 0, ''); // inicia a variavel q vai receber os dados do inventario atual
    this.inventarioRascunho = new InventarioAtividadeRascunho();
    this.inventarioAtualSalvo.EightIDUsuarioModificador = this.auth.getUser();
    this.inventarioRascunho.EightIDUsuarioModificador = this.auth.getUser();
    this.todosRiscosMapeados = [];

    await this.disciplinaService.getAll().then((discips) => {
      this.disciplinas = discips;
       });
    await this.atividadeService.getAll().then((atvs) => {
      this.atividades = atvs;
    });
    await this.pesoService.getAll().then((pesos) => {
      this.pesos = pesos;
     });
    await this.perfilDeCatalogoService.getAll().then((perfis) => {
      this.perfis = perfis;
      this.availableProfiles = perfis;
    });
    await this.duracaoService.getAll().then((durs) => {
      this.duracoes = durs;

     });
    await this.tipoRiscoService.getAll().then((tipos) => {
      this.tiposRisco = tipos;
      for (let i = 0; i < tipos.length; i++) {
        this.todosRiscosMapeados[i] = [];
      }
    });
    this.route.queryParams.subscribe(async (queryParams: InventarioAtividadeModelo) => {// pega o id recebido por paramtro na url
      this.treeviewLoader = false;
      if (queryParams.isRascunho) {
        this.abrirPopUp(6, 'Aguarde, estamos carregando o rascunho de inventario ...');
        this.textoBtn = 'SALVAR COMO INVENTARIO';
        this.textoTitle = 'Edição de Rascunho Inventario de Atividade';
        this.desabilitarButtonExcluir = false;
        this.isCadastro = false;
        this.isRascunho = true;
        this.isOperational = false;
        await this.atividadeService.getRascunhoPorId(queryParams.CodRascunhoInventarioAtividade).then((data) => {
          this.dialog.closeAll();
          this.setInventarioRascunhoRecebido(data);
        });
      }
      if (queryParams.CodInventarioAtividade !== undefined) {// caso seja undefind, entao nao é uma edicao
        this.textoBtn = 'CONCLUIR EDIÇAO';
        this.textoTitle = 'Edição de Inventario de Atividade';
        this.isCadastro = false;
        this.isOperational = false;
        this.abrirPopUp(6, 'Aguarde, estamos carregando o inventario ...');
        await this.atividadeService.getPorId(queryParams.CodInventarioAtividade).then((data) => {// pega o invntario complto pelo id
          this.dialog.closeAll();
          this.inventarioRecebido = data;
          if (this.inventarioRecebido.Ativo === false) {
            this.desabilitarButtonEdicao = true;
          }
          this.setInventarioRecebido();
          }).catch(() => {
            this.dialog.closeAll();
            this.redirecionarTelaGestao();
          });

      } else {
        this.isCadastro = true;
        this.treeviewLoader = true;
      }
    });

    if (this.isCadastro !== true) {
      await this.atividadeService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
        .then((lista: Tela[]) => {
          this.listaTelasEFuncionalidadesPorPerfil = [...lista];
          this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {
            if (tela.CodTela === 3) {
              tela.Funcionalidades.forEach((funcionalidade) => {
                this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
              });
            }
          });
        }).catch((e) => {
          // this.routerVariable.navigate(['/auth/login']);
        });
    } else {
      this.isOperational = true;
      this.desabilitarButtonEdicao = false;
      this.desabilitarDesassociacaoTreeview = false;
    }
  }
  verificaLogin() {
    if (!this.auth.isLogin()) {
        this.routerVariable.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }
  setInventarioRascunhoRecebido(inventario: InventarioAtividadeRascunho) {
    this.treeviewLoader = false;
    this.maiorRiscoGeral = inventario.RiscoGeral;
    this.descricaotextArea = inventario.Descricao;
    this.observacoestextArea = inventario.ObservacaoGeral;
    this.inventarioRascunho.CodRascunhoInventarioAtividade = inventario.CodRascunhoInventarioAtividade;
    this.setCamposTelasRascunho(inventario);
    this.setNodosSelecionadosTreeviewRascunho(inventario.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE);
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i <  inventario.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.length; i++) {
      this.converterRiscoRascunhoInvToRiscoMapeado( inventario.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE[i]);
    }
    this.treeviewLoader = true;
  }

  setInventarioRecebido() {
    this.maiorRiscoGeral = this.inventarioRecebido.RiscoGeral;
    this.descricaotextArea = this.inventarioRecebido.Descricao;
    this.observacoestextArea = this.inventarioRecebido.ObservacaoGeral;
    this.setCamposTelas();
    this.setNodosSelecionadosTreeview(this.inventarioRecebido.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE);
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i <  this.inventarioRecebido.RISCO_INVENTARIO_ATIVIDADE.length; i++) {
      this.converterRiscoInvToRiscoMapeado( this.inventarioRecebido.RISCO_INVENTARIO_ATIVIDADE[i]);
    }
  }
  setCamposTelas() {
    this.disciplinas.forEach((disciplina) => {
      if (disciplina.CodDisciplina === this.inventarioRecebido.CodDisciplina) {
        this.disciplinaSelecionada = disciplina;
      }
    });
    this.atividades.forEach((atividade) => {
      if (atividade.CodAtividadePadrao === this.inventarioRecebido.CodAtividade) {
        this.atividadeSelecionada = atividade;
      }
    });
    this.pesos.forEach((peso) => {
      if (peso.CodPeso === this.inventarioRecebido.CodPeso) {
        this.pesoSelecionado = peso;
      }
    });
    this.perfis.forEach((perfil) => {// se for nao for cadastro vai adicionar o perfilCatalogo que veio do BD ao campo na tela
      if (perfil.CodPerfilCatalogo === this.inventarioRecebido.CodPerfilCatalogo) {
        this.perfilSelecionado = perfil;
      }
    });
    this.duracoes.forEach((duracao) => {// se for nao for cadastro vai adicionar a duracao que veio do BD ao campo na tela
      if (duracao.CodDuracao === this.inventarioRecebido.CodDuracao) {
        this.duracaoSelecionada = duracao;
      }
    });
  }

  setCamposTelasRascunho(inventario: InventarioAtividadeRascunho) {
    this.disciplinas.forEach((disciplina) => {
      if (disciplina.CodDisciplina === inventario.CodDisciplina) {
        this.disciplinaSelecionada = disciplina;
      }
    });
    this.atividades.forEach((atividade) => {
      if (atividade.CodAtividadePadrao === inventario.CodAtividade) {
        this.atividadeSelecionada = atividade;
      }
    });
    this.pesos.forEach((peso) => {
      if (peso.CodPeso === inventario.CodPeso) {
        this.pesoSelecionado = peso;
      }
    });
    this.perfis.forEach((perfil) => {// se for nao for cadastro vai adicionar o perfilCatalogo que veio do BD ao campo na tela
      if (perfil.CodPerfilCatalogo === inventario.CodPerfilCatalogo) {
        this.perfilSelecionado = perfil;
      }
    });
    this.duracoes.forEach((duracao) => {// se for nao for cadastro vai adicionar a duracao que veio do BD ao campo na tela
      if (duracao.CodDuracao === inventario.CodDuracao) {
        this.duracaoSelecionada = duracao;
      }
    });
  }

  setNodosSelecionadosTreeviewRascunho(locaisRecebidos: LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE[]) {
    locaisRecebidos.forEach((localRecebido) => {
      this.nodosSelecionadosTreeview.push(localRecebido.LocalInstalacao.Nome.split(' ').slice(0, 1).join());
    });
    this.treeviewLoader = true;
    this.reloadTree();
  }

  setNodosSelecionadosTreeview(locaisRecebidos: LocalInstalacaoInventarioAtividade[]) {
    locaisRecebidos.forEach((localRecebido) => {
      this.nodosSelecionadosTreeview.push(localRecebido.LocalInstalacao.Nome.split(' ').slice(0, 1).join());
    });
    this.treeviewLoader = true;

  }
  async imprimir() {
    if (!this.isOperational) {
      this.abrirPopUp(0, 'Aguarde todos os locais de instalação estarem carregados');
      return;
    }
    this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
    this.expanded = true;
    const descricaoStyle = this.adaptarTamanho('textareaDescricao');
    const observacaoStyle = this.adaptarTamanho('textarea');

    document.getElementById('textareaDescricao').setAttribute('style', descricaoStyle);
    document.getElementById('textarea').setAttribute('style', observacaoStyle);
    await this.delay(1000);
    document.getElementById('imprimir').click();
  }

  adaptarTamanho(docID: string) {
    const fullStyle = document.getElementById(docID).getAttribute('style').split(':');
   /* let styleNumber = parseInt(fullStyle[1].split('p').join(), 10);
    styleNumber += styleNumber;*/
    fullStyle[0] += ': ' + 5 + 'cm;';
    return fullStyle[0];
  }

  async delay(ms: number) {
    await new Promise(resolve => setTimeout(() => resolve(), ms));
  }
  verificaFuncionalidades(codFuncionalidade) {
    switch (codFuncionalidade) {
      case 1:
        break;
      case 2:
        this.desabilitarButtonImpressao = false;
        break;
      case 3:
        this.desabilitarButtonEdicao = false;
        break;
      case 4:
        this.desabilitarButtonExcluir = false;
        break;
      case 5:
        this.desabilitarDesassociacaoTreeview = false;
        break;
    }
  }
  retornaSelecionados(selecionados) {
    this.nodosSelecionados = selecionados;
    this.flagEdicao = true;
  }
  retornaisOperational(editMode) {
    this.isOperational = editMode;
  }

  async selectFilters(selecao, tipoSelecao) {
    switch (tipoSelecao) {
      case 'disciplina':

        this.disciplinaSelecionada = selecao.value;
        this.flagEdicao = true;
        await this.recalcularRiscos();
        break;

      case 'atividade':
        this.flagEdicao = true;
        this.atividadeSelecionada = selecao.value;
        await this.recalcularRiscos();
        break;

      case 'perfil':
        this.flagEdicao = true;
        this.perfilSelecionado = selecao.value;
        await this.recalcularRiscos();
        this.reloadTree();
        break;

      case 'peso':
        this.flagEdicao = true;
        this.pesoSelecionado = selecao.value;
        await this.recalcularRiscos();
        this.reloadTree();
        break;

      case 'duracao':
        this.flagEdicao = true;
        this.duracaoSelecionada = selecao.value;
        await this.recalcularRiscos();
        break;
    }
  }

  async recalcularRiscos() {
    this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE = [];
    this.inventarioRascunho.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE = [];
    if (this.todosRiscosMapeados[0].length > 0) {
      const riscosMapeadosAux = this.todosRiscosMapeados[0];
      this.todosRiscosMapeados[0] = [];
      this.riscoGeral = [];
      riscosMapeadosAux.forEach(async (risco) => {
        await this.showRiscos(risco);
      });
    }
  }
  reloadTree() {
    this.treeviewEnabled = false;
    this.changeDetector.detectChanges();
    this.treeviewEnabled = true;
  }


  redirecionarTelaGestao(): void {
    this.routerVariable.navigate(['/client/inventario-atividade/']);
  }

  async editar(tipoRisco: number, row: RiscoMapeadoModelo, posicao: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '50%';
    dialogConfig.height = '45%';
    dialogConfig.minHeight = '310px';
    dialogConfig.minWidth = '350px';
    if (this.verificarCampos()) {
      dialogConfig.data = {
        disabledEdicao: this.desabilitarButtonEdicao,
        id: tipoRisco,
        riscoRecebido: row,
        tela: 1,
        atv: {
          CodAtividade: this.atividadeSelecionada,
          CodDisciplina: this.disciplinaSelecionada,
          CodDuracao: this.duracaoSelecionada,
          CodPeso: this.pesoSelecionado
        }
      };
      const dialogRef = this.dialog.open(RiscoComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(async result => {
        if (result) {
          const modelATV = {
            CodRisco: result.data.Risco.CodRisco,
            CodSeveridade: result.data.Severidade.CodSeveridade,
            CodAtividade: this.atividadeSelecionada.CodAtividadePadrao,
            CodDisciplina: this.disciplinaSelecionada.CodDisciplina,
            CodDuracao: this.duracaoSelecionada.CodDuracao,
            CodPeso: this.pesoSelecionado.CodPeso
          };
          await this.atividadeService.calcularRiscoGeral(modelATV).then((riscoGeral) => {
            result.data.Grau = riscoGeral;
            this.riscoGeral[posicao] = riscoGeral;
          });
          this.inventarioAtualSalvo.editarRisco(result.data, result.data.id);
          this.inventarioRascunho.editarRisco(result.data, result.data.id);
          this.todosRiscosMapeados[tipoRisco].splice(posicao, 1, result.data);
          this.maiorRiscoGeral = Math.max.apply(null, this.riscoGeral );
          this.flagRiscos = true;
          this.refresh();
        }
      });
    }
  }

  deletar(address: number, i: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 2, textoRecebido: 'Deseja apagar o risco?' };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'CONFIRMAR') {
        this.flagRiscos = true;
        this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE.splice(i, 1);
        this.inventarioRascunho.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.splice(i, 1);
        this.todosRiscosMapeados[address].splice(i, 1);
        this.riscoGeral.splice(i, 1);
        this.todosRiscosMapeados = [...this.todosRiscosMapeados];
        this.maiorRiscoGeral = Math.max.apply(null, this.riscoGeral );
        if (this.maiorRiscoGeral < 0) {
          this.maiorRiscoGeral = 0;
        }
        this.refresh();
      }
    });
  }

  entrarDadosTreeView() {
    if (this.dataSource) {
      return this.dataSource;
    }
  }

  adicionarRisco(tipoDeRisco: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '45%';
    dialogConfig.height = '35%';
    dialogConfig.minHeight = '310px';
    dialogConfig.minWidth = '350px';
    if (this.verificarCampos()) {
    dialogConfig.data = {
      id: tipoDeRisco,
      riscoRecebido: undefined,
      tela: 1,
      atv: {
        CodAtividade: this.atividadeSelecionada.CodAtividadePadrao,
        CodDisciplina: this.disciplinaSelecionada.CodDisciplina,
        CodDuracao: this.duracaoSelecionada.CodDuracao,
        CodPeso: this.pesoSelecionado.CodPeso
      }
    };
    const dialogRef = this.dialog.open(RiscoComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
        if (result) {
          result.data.Grau = 4;
          this.showRiscos(result.data);
          this.flagRiscos = true;
        }
      });
    }
  }

  verificarCampos() {
    if ( this.atividadeSelecionada && this.disciplinaSelecionada && this.duracaoSelecionada && this.pesoSelecionado) {
      return true;
    } else {
      let textString = 'Dados insuficientes para o cálculo de risco. Favor preencher:<br>';
      if (!this.atividadeSelecionada) {
        textString += '<br><strong>Atividade</strong>';
      }
      if (!this.disciplinaSelecionada) {
        textString += '<br><strong>Disciplina</strong>';
      }
      if (!this.duracaoSelecionada) {
        textString += '<br><strong>Duração</strong>';
      }
      if (!this.pesoSelecionado) {
        textString += '<br><strong>Peso</strong>';
      }
      this.callErrorPopup(textString);
      return false;
    }
  }

  callErrorPopup(text: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.minWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 0, textoRecebido: text };
    this.dialog.open(PopupComponent, dialogConfig);
  }

  callInfoPopup(text: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.minWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 3, textoRecebido: text };
    this.dialog.open(PopupComponent, dialogConfig);
  }

 async showRiscos(risco: RiscoMapeadoModelo) {
    risco.id = Math.random().toString(36).substr(2, 9);

    const modelATV = {
      CodRisco: risco.Risco.CodRisco,
      CodSeveridade: risco.Severidade.CodSeveridade,
      CodAtividade: this.atividadeSelecionada.CodAtividadePadrao,
      CodDisciplina: this.disciplinaSelecionada.CodDisciplina,
      CodDuracao: this.duracaoSelecionada.CodDuracao,
      CodPeso: this.pesoSelecionado.CodPeso
    };
    await this.atividadeService.calcularRiscoGeral(modelATV).then((riscoGeral) => {
      risco.Grau = riscoGeral;
      this.riscoGeral.push(riscoGeral);

    });
    this.todosRiscosMapeados[risco.Risco.CodTipoRisco - 1].push(risco);
    this.todosRiscosMapeados = [...this.todosRiscosMapeados];
    this.inventarioAtualSalvo.setRiscoInventario(risco);
    this.inventarioRascunho.setRiscoInventario(risco);
    this.maiorRiscoGeral = Math.max.apply(null, this.riscoGeral );
    this.refresh();
  }

  refresh() {
    this.table1.renderRows();

  }
  converterEpiRascunho(epis: EpiRascunhoAtividadeModelo[]) {
    const epiModelo = [];
    epis.forEach((epi) => {
      epiModelo.push(new EPI(epi.CodEPI));
    });
    return epiModelo;


  }
  converterRiscoRascunhoInvToRiscoMapeado(riscoInv: RISCO_RASCUNHO_INVENTARIO_ATIVIDADE) {
    const riscoMapeado = new RiscoMapeadoModelo(null, null, null, riscoInv.FonteGeradora,
      riscoInv.ProcedimentoAplicavel, 0, riscoInv.ContraMedidas, null);
    riscoMapeado.EPI = this.converterEpiRascunho(riscoInv.EPIRiscoRascunhoInventarioAtividadeModelo);
    this.riscoService.getAll().then((riscos) => {
      riscos.forEach((risco) => {
        if (risco.CodRisco === riscoInv.CodRisco) {
          riscoMapeado.setRisco(risco);
        }
      });
      this.severidadeService.getAll().then((severidades) => {
        severidades.forEach((severidade) => {
          if (severidade.CodSeveridade === riscoInv.CodSeveridade) {
            riscoMapeado.setSeveridade(severidade);
          }
        });
        this.showRiscos(riscoMapeado);
      });
    });


  }


  converterRiscoInvToRiscoMapeado(riscoInv) {
    const riscoMapeado = new RiscoMapeadoModelo(null, null, null, riscoInv.FonteGeradora,
      riscoInv.ProcedimentoAplicavel, 0, riscoInv.ContraMedidas, null);
    riscoMapeado.EPI = [...riscoInv.EPIRiscoInventarioAtividadeModelo];
    this.riscoService.getAll().then((riscos) => {
      riscos.forEach((risco) => {
        if (risco.CodRisco === riscoInv.CodRisco) {
          riscoMapeado.setRisco(risco);
        }
      });
      this.severidadeService.getAll().then((severidades) => {
        severidades.forEach((severidade) => {
          if (severidade.CodSeveridade === riscoInv.CodSeveridade) {
            riscoMapeado.setSeveridade(severidade);
          }
        });
        this.showRiscos(riscoMapeado);
      });
    });

  }
  salvarComoRascunho() {
    this.setDadosParaSalvarRascunho();
    this.inventarioRascunho.setLocalInventario(this.nodosSelecionados);
    if (this.isRascunho) {
      this.inventarioRascunho.novoInventario = false;
      this.atividadeService.editarRascunho(this.inventarioRascunho).then((data) => {
        this.redirecionarTelaGestao();
      });
    } else {
      this.atividadeService.inserirRascunho(this.setDadosParaSalvarRascunho()).then((data) => {
        this.redirecionarTelaGestao();
      });
    }
  }
  setDadosParaSalvarRascunho() {
    if (this.pesoSelecionado !== undefined) {
      this.inventarioRascunho.CodPeso = this.pesoSelecionado.CodPeso;
    }
    if (this.duracaoSelecionada !== undefined) {
      this.inventarioRascunho.CodDuracao = this.duracaoSelecionada.CodDuracao;
    }
    if (this.disciplinaSelecionada !== undefined) {
      this.inventarioRascunho.CodDisciplina = this.disciplinaSelecionada.CodDisciplina;
    }
    if (this.atividadeSelecionada !== undefined) {
      this.inventarioRascunho.CodAtividade = this.atividadeSelecionada.CodAtividadePadrao;
    }
    if (this.perfilSelecionado !== undefined) {
      this.inventarioRascunho.CodPerfilCatalogo = this.perfilSelecionado.CodPerfilCatalogo;
    }
    this.inventarioRascunho.Descricao = this.descricaotextArea;
    this.inventarioRascunho.ObservacaoGeral = this.observacoestextArea;
    this.inventarioRascunho.RiscoGeral = Math.max.apply(null, this.riscoGeral );
    this.inventarioRascunho.setLocalInventario(this.nodosSelecionados);

    return this.inventarioRascunho;
  }


  setDadosParaSalvar() {
    if (this.isCadastro) {
      this.inventarioAtualSalvo.setDescricao(this.descricaotextArea);
      this.inventarioAtualSalvo.setObservacaoGeral(this.observacoestextArea);
      this.inventarioAtualSalvo.setCodDuracao(this.duracaoSelecionada.CodDuracao);
      this.inventarioAtualSalvo.setCodPeso(this.pesoSelecionado.CodPeso);
      this.inventarioAtualSalvo.setCodPerfilCatalogo(this.perfilSelecionado.CodPerfilCatalogo);
      this.inventarioAtualSalvo.setCodAtividade(this.atividadeSelecionada.CodAtividadePadrao);
      this.inventarioAtualSalvo.setCodDisciplina(this.disciplinaSelecionada.CodDisciplina);
      this.inventarioAtualSalvo.setRiscoGeral( Math.max.apply(null, this.riscoGeral ));
      this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
    } else {
      this.inventarioAtualSalvo.setCodInventarioAtividade(this.inventarioRecebido.CodInventarioAtividade);
      this.inventarioAtualSalvo.Codigo = this.inventarioRecebido.Codigo;
      this.inventarioAtualSalvo.setDescricao(this.descricaotextArea);
      this.inventarioAtualSalvo.setObservacaoGeral(this.observacoestextArea);
      this.inventarioAtualSalvo.setCodDuracao(this.duracaoSelecionada.CodDuracao);
      this.inventarioAtualSalvo.setCodPeso(this.pesoSelecionado.CodPeso);
      this.inventarioAtualSalvo.setCodPerfilCatalogo(this.perfilSelecionado.CodPerfilCatalogo);
      this.inventarioAtualSalvo.setCodAtividade(this.atividadeSelecionada.CodAtividadePadrao);
      this.inventarioAtualSalvo.setCodDisciplina(this.disciplinaSelecionada.CodDisciplina);
      this.inventarioAtualSalvo.setRiscoGeral( Math.max.apply(null, this.riscoGeral ));
      this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
      if (this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE.length > 0) {
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE.length; i++) {
          this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE[i].id = null;
        }
      }
    }

  }

  salvar() {
    if (!this.isOperational) {
      this.abrirPopUp(0, 'Aguarde todos os locais de instalação estarem carregados');
      return;
    }
    this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
    this.inventarioRascunho.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE = [...this.nodosSelecionados];
    if (this.isRascunho) {
      if (this.validarForms() === true) {
        this.setDadosParaSalvarRascunho();
        this.inventarioRascunho.novoInventario = true;
        this.atividadeService.editarRascunho(this.inventarioRascunho).then((data) => {
          this.redirecionarTelaGestao();
        });
      }

      return;
    }
    if (this.isCadastro) {
      // TREEVIEW
      if (this.nodosSelecionados.filter(n => n.level < 3).length > 0) {
        this.callInfoPopup('É possivel que o processamento seja lento pelo alto número de locais. Por favor, aguarde...');
      }

      if (this.validarForms() === true) {
        this.setDadosParaSalvar();
        this.atividadeService.inserir(this.inventarioAtualSalvo).then((data) => {
          this.redirecionarTelaGestao();
        });
      }

    } else {
      if (this.validarForms() === true) {
        this.setDadosParaSalvar();
        if ((this.flagEdicao === true) || (this.flagRiscos === true)) {
          this.inventarioAtualSalvo.Ativo = false;
          this.auxInventario = this.inventarioAtualSalvo;
        } else {
          this.inventarioAtualSalvo.Ativo = true;
        }
        this.atividadeService.temAPR(this.inventarioRecebido.CodInventarioAtividade).then((data) => {
          if (data === 0) {
            this.abrirPopUp(2, 'Deseja editar este inventario de atividade?');
          } else {
            this.abrirPopUp(2, 'Esse inventario possui APR associada, deseja editar mesmo assim?');
          }
        });
      }
    }
  }
  deletarInventario() {
    if (this.isRascunho) {
    this.confirmarExclusaoPopup(2, 'Deseja excluir o rascunho?');
    } else {
      this.atividadeService.temAPR(this.inventarioRecebido.CodInventarioAtividade).then((data => {
        console.log(data);
        if (data === 0) {
          this.confirmarExclusaoPopup(2, 'Deseja excluir o inventário?');
        } else {
          this.excluirInventarioComAPR(2, 'Esse inventario possui APR associada, deseja excluir mesmo assim?');
        }
      }));
    }

  }

  baixarLog() {
    const id = [];
    id.push(this.inventarioRecebido.CodInventarioAtividade);
    this.atividadeService.gerarLog(id).then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_inventario_atividade_' + this.inventarioRecebido.CodInventarioAtividade + '_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }

   excluirInventarioComAPR(tipo: number, mensagem: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'CONFIRMAR') {
        const modelDelete = {
          CodInventarioAtividade: this.inventarioRecebido.CodInventarioAtividade,
          EightIDUsuarioModificador: this.auth.getUser(),
        };
        this.atividadeService.deletar(modelDelete).then(() => {
          this.redirecionarTelaGestao();
        }).catch(() => {
        });
        }
    });
  }
  confirmarExclusaoPopup(tipo: number, mensagem: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (this.isRascunho) {
        if (result === 'CONFIRMAR') {
          this.atividadeService.deletarRascunho(this.inventarioRascunho.CodRascunhoInventarioAtividade).then(() => {
            this.redirecionarTelaGestao();
          }).catch(() => {
          });
        }

      } else if (result === 'CONFIRMAR') {
        const modelDelete = {
          CodInventarioAtividade: this.inventarioRecebido.CodInventarioAtividade,
          EightIDUsuarioModificador: this.auth.getUser(),
        };
        this.atividadeService.deletar(modelDelete).then(() => {
          this.redirecionarTelaGestao();
        }).catch(() => {
        });
      }
    });
  }

  validarForms() {
    if (this.disciplinaSelecionada === undefined) {
      this.abrirPopUp(0, 'Insira uma DISCIPLINA para cadastrar inventario');
      return false;
    }
    if (this.atividadeSelecionada === undefined) {
      this.abrirPopUp(0, 'Insira uma ATIVIDADE para cadastrar inventario');
      return false;
    }
    if (this.perfilSelecionado === undefined) {
      this.abrirPopUp(0, 'Insira um PERFIL DE CATÁLOGO para cadastrar inventario');
      return false;
    }
    if (this.pesoSelecionado === undefined) {
      this.abrirPopUp(0, 'Insira um PESO FÍSICO para cadastrar inventario');
      return false;
    }
    if (this.duracaoSelecionada === undefined) {
      this.abrirPopUp(0, 'Insira uma DURAÇÃO para cadastrar inventario');
      return false;
    }
    if ( (this.inventarioAtualSalvo.RISCO_INVENTARIO_ATIVIDADE.length === 0) &&
    (this.inventarioRascunho.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.length === 0)) {
      this.abrirPopUp(0, 'Insira pelo menos um RISCO para cadastrar inventario');
      return false;
    }
    if ((this.inventarioAtualSalvo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.length === 0) &&
    (this.inventarioRascunho.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.length === 0)) {
      this.abrirPopUp(0, 'Insira pelo menos um LOCAL DE INSTALAÇÃO para cadastrar inventario');
      return false;
    }
    return true;
  }
  abrirPopUp(tipo: number, mensagem: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.disableClose = true;
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'CONFIRMAR') {
        this.atividadeService.editar(this.inventarioAtualSalvo).then((data) => {
          this.flagEdicao = false;
          this.flagRiscos = false;
          this.enviado = true;
          // this.abrirPopUp(1,"Inventario Editado com Sucesso")
          this.redirecionarTelaGestao();
        }).catch(() => {

        });

      }
      return result;

    });
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
