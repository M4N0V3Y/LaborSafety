import { Component, OnInit, ViewChild, OnDestroy, ComponentRef, ChangeDetectorRef, ɵConsole } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RiscoMapeadoModelo } from 'src/app/shared/models/riscomapeado.model';
import { TipoRiscoService } from 'src/app/core/http/tiporisco/tiporisco.service';
import { TipoRisco } from 'src/app/shared/models/tiporisco.model';
import { InvAmbienteService } from 'src/app/core/http/inventarioAmbiente/inv-ambiente.service';
import { MatDialogConfig, MatDialog, MatTable } from '@angular/material';
import { RiscoComponent } from '../../risco/risco.component';
import { TreeViewComponent } from '../../treeview/treeview.component';
import { Ambiente } from 'src/app/shared/models/ambiente';
import { Risco } from 'src/app/shared/models/risco.model';
import { Frequencia } from 'src/app/shared/models/frequencia.model';
import { Severidade } from 'src/app/shared/models/severidade.model';
import { SeveridadeService } from 'src/app/core/http/severidade/severidade.service';
import { RiscoService } from 'src/app/core/http/risco/risco.service';
import { FrequenciaService } from 'src/app/core/http/frequencia/frequencia.service';
import { AmbienteModel, RiscoAmbiente, NRModel, LocalInstalacaoModel } from 'src/app/shared/models/invAmbinteModel';
import { NrModelTela } from 'src/app/shared/models/NRmodel';
import { Tela } from 'src/app/shared/models/tela.model';
import { RiscoTotalModelo } from 'src/app/shared/models/riscoTotalModelo';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import {NgxPrintDirective} from 'ngx-print/lib/ngx-print.directive';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { TreeviewdoisComponent } from '../../treeviewdois/treeviewdois.component';
import { InventarioAmbienteRascunho,
  NR_Rascunho, RISCO_RASCUNHO_INVENTARIO_AMBIENTE } from 'src/app/shared/models/InventarioAmbienteRascunhoModel';
import { EPI } from 'src/app/shared/models/EPI.model';
import { EpiRascunhoModelo } from 'src/app/shared/models/epiRascunhoModel';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
@Component({
  selector: 'app-cadastro-inv-ambiente',
  templateUrl: './cadastro-inv-ambiente.component.html',
  styleUrls: ['./cadastro-inv-ambiente.component.scss']
})
export class CadastroInvAmbienteComponent implements OnInit {
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;
  @ViewChild('tableFisico') table1: MatTable<any>;
  @ViewChild('tableQuimico') table2: MatTable<any>;
  @ViewChild('tableBiologico') table3: MatTable<any>;
  @ViewChild('tableErgonomico') table4: MatTable<any>;
  @ViewChild('tableAcidente') table5: MatTable<any>;
  @ViewChild('tableOutros') table6: MatTable<any>;

  displayedColumns: string[] = ['Risco', 'Severidade', 'Frequencia', 'Grau', 'Controles'];
  tiposRisco: TipoRisco[] = [];
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  todosRiscosMapeados: Array<RiscoMapeadoModelo[]> = [];
  sourceLocais: any;
  public riscoGeral = [[]];
  maiorRiscoGeral: number;
  descricaotextArea = '';
  observacaotextArea = '';
  ambientes: Ambiente[] = [];
  ambienteSelecionado: Ambiente;
  inventarioRecebido: AmbienteModel;
  public nodosSelecionados: any;
  inventarioAtualSalvo = new AmbienteModel(0, '', 0, '', '', 0);
  todosNRs: NrModelTela[];
  nrImpressao: NrModelTela[];
  NrsSelecionados: NRModel[] = [];
  isCadastro: boolean;
  flagRiscos = false;
  flagEdicao = false;
  flagNr = false;
  enviado = false;
  nrNaoAplicavel = true;
  desabilitarButtonImpressao = true;
  desabilitarButtonEdicao = true;
  desabilitarButtonExcluir = true;
  desabilitarDesassociacaoTreeview = true;
  expanded = false;
  treeviewLoader: boolean;
  nodosSelecionadosTreeview: string[] = [];
  inventarioRascunho = new InventarioAmbienteRascunho();
  isRascunho: boolean;
  treeviewEnabled: boolean;
  textoBtn = 'SALVAR COMO INVENTARIO';
  textoTitle = 'Cadastro de Inventario de Ambiente';
  user: string;
  isOperational = false;
  constructor(private routerVariable: Router, private route: ActivatedRoute,
              public tipoRiscoService: TipoRiscoService, public invService: InvAmbienteService,
              private dialog: MatDialog, public riscoService: RiscoService, public severidadeService: SeveridadeService,
              public frequenciaService: FrequenciaService, private changeDetector: ChangeDetectorRef, private auth: AuthenticationService) {
                if (!this.verificaLogin()) {
                  return;
                }
  }


  async ngOnInit() {
    this.user = this.auth.getUser();
    this.inventarioAtualSalvo.EightIDUsuarioModificador = this.auth.getUser();
    this.inventarioRascunho.EightIDUsuarioModificador = this.auth.getUser();
    await this.invService.listarTodasNRs().then((Nrs) => {
      this.todosNRs = [...Nrs];
    });
    await this.invService.getAllSOs().then((ambientes) => {
      this.ambientes = ambientes;
    });

    this.route.queryParams.subscribe((queryParams) => {

      if (queryParams.isRascunho) {
        this.abrirPopUp(6, 'Aguarde, estamos carregando o rascunho de inventario ...');
        this.textoTitle = 'Edição de Rascunho Inventario de Ambiente';
        this.textoBtn = 'SALVAR COMO INVENTARIO';
        this.desabilitarButtonExcluir = false;
        this.isCadastro = false;
        this.isRascunho = true;
        this.invService.getRascunhoPorId(queryParams.CodRascunhoInventarioAmbiente).then((data) => {
          this.dialog.closeAll();
          this.desabilitarDesassociacaoTreeview = false;
          this.setInventarioRascunhoRecebido(data);
        });
      }

      if (queryParams.CodInventarioAmbiente !== undefined) {
        this.textoTitle = 'Edição de Inventario de Ambiente';
        this.textoBtn = 'CONCLUIR EDICAO';
        this.abrirPopUp(6, 'Aguarde, estamos carregando o inventario ...');
        this.invService.getPorId(queryParams.CodInventarioAmbiente).then((data: any) => {
          this.dialog.closeAll();
          this.inventarioRecebido = data;

          if (this.inventarioRecebido.Ativo === false) {
            this.desabilitarButtonEdicao = true;
          }
          this.setInventarioRecebido();
        }).catch(() => {
          this.dialog.closeAll();
          this.dialog.closeAll();
        });
      } else {
        this.isOperational = true;
        this.isCadastro = true;
        this.treeviewLoader = true;
      }
    });
    this.tipoRiscoService.getAll().then((tipos) => {
      this.tiposRisco = tipos;
      for (let i = 0; i < tipos.length; i++) {
        this.todosRiscosMapeados[i] = [];
        this.riscoGeral[i] = [];
      }

    });

    if (this.isCadastro !== true) {

      await this.invService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
        .then((lista: Tela[]) => {
          this.listaTelasEFuncionalidadesPorPerfil = [...lista];
          this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {
            if (tela.CodTela === 2) {
              tela.Funcionalidades.forEach((funcionalidade) => {
                this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
              });
            }
          });
        }).catch(() => {
          this.routerVariable.navigate(['/auth/login']);
        });
    } else {
      this.treeviewEnabled = true;
      this.desabilitarButtonEdicao = false;
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
  baixarLog() {
    const id = [];
    id.push(this.inventarioRecebido.CodInventarioAmbiente);
    this.invService.gerarLog(id).then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_inventario_ambiente_' + this.inventarioRecebido.CodInventarioAmbiente + '_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }
  reloadTree() {
    this.treeviewEnabled = false;
    this.changeDetector.detectChanges();
    this.treeviewEnabled = true;
  }
  setInventarioRascunhoRecebido(inventario: InventarioAmbienteRascunho) {
    this.descricaotextArea = inventario.Descricao;
    this.maiorRiscoGeral = inventario.RiscoGeral;
    this.observacaotextArea = inventario.ObservacaoGeral;
    this.inventarioRascunho.CodRascunhoInventarioAmbiente = inventario.CodRascunhoInventarioAmbiente;
    this.setNodosSelecionadosTreeview(inventario.LOCAL_INSTALACAO_MODELO);
          // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < inventario.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.length; i++) {
      this.converterRiscoInvRascunhoToRiscoMapeado(inventario.RISCO_RASCUNHO_INVENTARIO_AMBIENTE[i]);
    }


        // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.todosNRs.length; i++) {
      // tslint:disable-next-line: prefer-for-of
      for (let j = 0; j < inventario.NR_RASCUNHO_INVENTARIO_AMBIENTE.length; j++) {
        if (this.todosNRs[i].CodNR === inventario.NR_RASCUNHO_INVENTARIO_AMBIENTE[j].CodNR) {
          this.todosNRs[i].checked = true;
          this.nrNaoAplicavel = false;
        }
      }
    }
    if (this.todosNRs[0].checked === true) {
      this.todosNRs[0].checked = false;
      this.nrNaoAplicavel = true;
    }
    this.ambientes.forEach((ambiente) => {
      if (ambiente.CodAmbiente === inventario.CodAmbiente) {
        this.ambienteSelecionado = ambiente;
      }
    });
  }
  setInventarioRecebido() {
    this.descricaotextArea = this.inventarioRecebido.Descricao;
    this.maiorRiscoGeral = this.inventarioRecebido.RiscoGeral;
    this.observacaotextArea = this.inventarioRecebido.ObservacaoGeral;
    this.setNodosSelecionadosTreeview(this.inventarioRecebido.LOCAL_INSTALACAO_MODELO);
          // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.inventarioRecebido.RISCO_INVENTARIO_AMBIENTE.length; i++) {
      this.converterRiscoInvToRiscoMapeado(this.inventarioRecebido.RISCO_INVENTARIO_AMBIENTE[i]);
    }


        // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.todosNRs.length; i++) {
      // tslint:disable-next-line: prefer-for-of
      for (let j = 0; j < this.inventarioRecebido.NR_INVENTARIO_AMBIENTE.length; j++) {
        if (this.todosNRs[i].CodNR === this.inventarioRecebido.NR_INVENTARIO_AMBIENTE[j].CodNR) {
          this.todosNRs[i].checked = true;
          this.nrNaoAplicavel = false;
        }
      }
    }
    if (this.todosNRs[0].checked === true) {
      this.todosNRs[0].checked = false;
      this.nrNaoAplicavel = true;
    }
    this.ambientes.forEach((ambiente) => {
      if (ambiente.CodAmbiente === this.inventarioRecebido.CodAmbiente) {
        this.ambienteSelecionado = ambiente;
      }
    });
  }

  setNodosSelecionadosTreeviewRascunho(locaisRecebidos: LocalInstalacaoModel[]) {
    locaisRecebidos.forEach((localRecebido) => {
      this.nodosSelecionadosTreeview.push(localRecebido.Nome.split(' ').slice(0, 1).join());
    });
    this.treeviewLoader = true;
  }

  setNodosSelecionadosTreeview(locaisRecebidos: LocalInstalacaoModel[]) {
    locaisRecebidos.forEach((localRecebido) => {
      this.nodosSelecionadosTreeview.push(localRecebido.Nome.split(' ').slice(0, 1).join());
    });
    this.reloadTree();
    this.treeviewLoader = true;
  }
  // NAO MEXER
  async imprimir() {
    if (!this.isOperational) {
      this.abrirPopUp(0, 'Aguarde todos os locais de instalação estarem carregados');
      return;
    }
    this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
    this.converterNrs(this.todosNRs);
    this.expanded = true;

    const descricaoStyle = this.adaptarTamanho('textareaDescricao');
    const observacaoStyle = this.adaptarTamanho('textarea');

    document.getElementById('textareaDescricao').setAttribute('style', descricaoStyle);
    document.getElementById('textarea').setAttribute('style', observacaoStyle);
    await this.delay(250);
    document.getElementById('imprimir').click();
  }
  async delay(ms: number) {
    await new Promise(resolve => setTimeout(() => resolve(), ms));
  }
  verificaFuncionalidades(codFuncionalidade) {
    switch (codFuncionalidade) {
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

  adaptarTamanho(docID: string) {
    const fullStyle = document.getElementById(docID).getAttribute('style').split(':');
    fullStyle[0] += ': ' + 5 + 'cm;';
    return fullStyle[0];
  }

  selectFilters(selecao, tipoSelecao) {
    switch (tipoSelecao) {
      case 'ambiente':
        this.flagEdicao = true;
        this.ambienteSelecionado = selecao.value;
        break;
    }
  }

  retornaSelecionados(selecionados) {
    this.flagEdicao = true;
    this.nodosSelecionados = selecionados;
  }
  retornaisOperational(editMode) {
    this.isOperational = editMode;
  }


  redirecionarTelaGestao(): void {
    this.routerVariable.navigate(['/client/inventario-ambiente/']);
  }


  associaLocaisTreeView() {
    if (this.sourceLocais !== undefined) {
      return this.sourceLocais;
    }
  }


  adicionarRisco(tipoDeRisco: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '50%';
    dialogConfig.height = '45%';
    dialogConfig.minHeight = '300px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = { id: tipoDeRisco, riscoRecebido: undefined, tela: 0 };

    const dialogRef = this.dialog.open(RiscoComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.flagEdicao = true;
        this.showRiscos(result.data);
      }
    });
  }


  async editar(tipoRisco: number, row: RiscoMapeadoModelo, posicao: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '45%';
    dialogConfig.height = '35%';
    dialogConfig.minHeight = '260px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = { id: tipoRisco, riscoRecebido: row, tela: 0, disabledEdicao: this.desabilitarButtonEdicao };
    const dialogRef = this.dialog.open(RiscoComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(async result => {
      if (result) {
        const model = new RiscoTotalModelo();
        model.CodSeveridade = result.data.Severidade.CodSeveridade;
        model.CodProbabilidade = result.data.Frequencia.CodProbabilidade;
        await this.invService.calcularRiscoGeral(model).then((riscoGeral) => {
          this.riscoGeral[tipoRisco][posicao] = riscoGeral;
          result.data.Grau = riscoGeral;
      });

        this.flagEdicao = true;
        this.inventarioAtualSalvo.editarRisco(result.data, result.data.id);
        this.inventarioRascunho.editarRisco(result.data, result.data.id);
        this.todosRiscosMapeados[tipoRisco].splice(posicao, 1, result.data);
        this.calcularMaiorRiscoGeral();
        this.refresh();
      }
    });

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
        this.flagEdicao = true;
        let posicaoRemovido: number = null;
        for (let j = 0; j < this.pegarQuantidadeItensMatriz(); j++) {
          if (this.todosRiscosMapeados[address][i].id === this.inventarioAtualSalvo.RISCO_INVENTARIO_AMBIENTE[j].id) {
            posicaoRemovido = j;
          }
        }
        if (posicaoRemovido != null) {
            this.inventarioAtualSalvo.RISCO_INVENTARIO_AMBIENTE.splice(posicaoRemovido, 1);
            this.inventarioRascunho.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.splice(posicaoRemovido, 1);
        }
        this.riscoGeral[address].splice(i, 1);
        this.todosRiscosMapeados[address].splice(i, 1);
        this.todosRiscosMapeados = [...this.todosRiscosMapeados];
        this.calcularMaiorRiscoGeral();
        this.refresh();
      }
    });


  }

  showRiscos(risco: RiscoMapeadoModelo) {
    risco.id = Math.random().toString(36).substr(2, 9);
    // Calcular RiscoGeral
    const model = new RiscoTotalModelo();
    model.CodSeveridade = risco.Severidade.CodSeveridade;
    model.CodProbabilidade = risco.Frequencia.CodProbabilidade;
    this.invService.calcularRiscoGeral(model).then((riscoGeral) => {
      this.riscoGeral[risco.Risco.CodTipoRisco - 1].push(riscoGeral);
      risco.Grau = riscoGeral;
      this.calcularMaiorRiscoGeral();
    });
    this.todosRiscosMapeados[risco.Risco.CodTipoRisco - 1].push(risco);
    this.todosRiscosMapeados = [...this.todosRiscosMapeados];
    this.inventarioAtualSalvo.setRiscoInventario(risco);
    this.inventarioRascunho.setRiscoInventario(risco);
    this.refresh();
  }

  refresh() {
    this.table2.renderRows();
    this.table3.renderRows();
    this.table4.renderRows();
    this.table5.renderRows();
    this.table6.renderRows();
  }

  calcularMaiorRiscoGeral() {
    const maxRow = this.riscoGeral.map((row) => {
      return Math.max.apply(Math, row);
    });
    this.maiorRiscoGeral = Math.max.apply(null, maxRow );
    if (this.maiorRiscoGeral < 0) {
      this.maiorRiscoGeral = 0;
    }
  }
  pegarQuantidadeItensMatriz(){
    const totalItens = this.todosRiscosMapeados.reduce((total, numero) => {
      return total + numero.length;
      }, 0);
    return totalItens;
  }


  converterRiscoMapeadoToRiscoInv(risco: RiscoMapeadoModelo) {
    // tslint:disable-next-line:prefer-const
    let riscoModelo: RiscoAmbiente;
    riscoModelo.CodSeveridade = risco.Severidade.CodSeveridade;
    riscoModelo.CodProbabilidade = risco.Frequencia.CodProbabilidade;
    riscoModelo.FonteGeradora = risco.FonteGeradora;
    riscoModelo.ProcedimentosAplicaveis = risco.Procedimentos;
    riscoModelo.CodRiscoAmbiente = risco.Risco.CodRisco;
   // riscoModelo.EpiRiscoModelo = risco.EPI;
    riscoModelo.CodRiscoInventarioAmbiente = 0;
    riscoModelo.CodInventarioAmbiente = 0;
    return riscoModelo;
  }
  converterEpiRascunho(epis: EpiRascunhoModelo[]) {
    const epiModelo = [];
    epis.forEach((epi) => {
      epiModelo.push(new EPI(epi.CodEPI));
    });
    return epiModelo;
  }
  converterRiscoInvRascunhoToRiscoMapeado(riscoInv: RISCO_RASCUNHO_INVENTARIO_AMBIENTE)  {
    const riscoMapeado = new RiscoMapeadoModelo(null, null, null, riscoInv.FonteGeradora,
      riscoInv.ProcedimentosAplicaveis, 0, riscoInv.ContraMedidas, null);
    riscoMapeado.EPI = this.converterEpiRascunho(riscoInv.EPIRiscoRascunhoInventarioAmbiente);
    this.riscoService.getAll().then((riscos) => {
      riscos.forEach((risco) => {
        if (risco.CodRisco === riscoInv.CodRiscoAmbiente) {
          risco = risco;
          riscoMapeado.setRisco(risco);
        }
      });
      this.severidadeService.getAll().then((severidades) => {
        severidades.forEach((severidade) => {
          if (severidade.CodSeveridade === riscoInv.CodSeveridade) {
            riscoMapeado.setSeveridade(severidade);
          }
        });

        this.frequenciaService.getAll().then((frequencias) => {
          frequencias.forEach((frequencia) => {
            if (frequencia.CodProbabilidade === riscoInv.CodProbabilidade) {
              frequencia = frequencia;
              riscoMapeado.setFrequencia(frequencia);
            }
          });

          this.showRiscos(riscoMapeado);
        });
      });
    });

  }


  converterRiscoInvToRiscoMapeado(riscoInv)  {
    const riscoMapeado = new RiscoMapeadoModelo(null, null, null, riscoInv.FonteGeradora,
      riscoInv.ProcedimentosAplicaveis, 0, riscoInv.ContraMedidas, null);
    riscoMapeado.EPI = riscoInv.EPIRiscoInventarioAmbienteModelo;
    this.riscoService.getAll().then((riscos) => {
      riscos.forEach((risco) => {
        if (risco.CodRisco === riscoInv.CodRiscoAmbiente) {
          risco = risco;
          riscoMapeado.setRisco(risco);
        }
      });
      this.severidadeService.getAll().then((severidades) => {
        severidades.forEach((severidade) => {
          if (severidade.CodSeveridade === riscoInv.CodSeveridade) {
            riscoMapeado.setSeveridade(severidade);
          }
        });

        this.frequenciaService.getAll().then((frequencias) => {
          frequencias.forEach((frequencia) => {
            if (frequencia.CodProbabilidade === riscoInv.CodProbabilidade) {
              frequencia = frequencia;
              riscoMapeado.setFrequencia(frequencia);
            }
          });

          this.showRiscos(riscoMapeado);
        });
      });
    });

  }


  salvar() {
    if (!this.isOperational) {
      this.abrirPopUp(0, 'Aguarde todos os locais de instalação estarem carregados');
      return;
    }
    this.inventarioRascunho.LOCAL_INSTALACAO_MODELO = [...this.nodosSelecionados];
    this.inventarioAtualSalvo.setLocalInventario(this.nodosSelecionados);
    if (this.isRascunho) {
      if (this.validarForms()) {
        this.setDadosParaSalvarRascunho();
        this.inventarioRascunho.novoInventario = true;
        this.invService.editarRascunho(this.inventarioRascunho).then((data) => {
        this.redirecionarTelaGestao();
        }).catch((data) => {
        });

      }
      return;
    }


    if (this.isCadastro) {
      if (this.validarForms() === true) {
        this.setDadosParaSalvar();
        this.invService.inserir(this.inventarioAtualSalvo).then((data) => {
          try {
            if (data.status === false) {
                this.abrirPopUp(5, 'Os seguintes locais de instalaçao ja tem inventario de ambiente associado', data.localModelo);
              } else {
                this.redirecionarTelaGestao();
              }
            } catch (e) {

            }
        });

      }
    } else {
      if (this.validarForms() === true) {
        this.setDadosParaSalvar();
        // VERIFICA SE ALGUM DOS CAMPOS PRINCIPAIS FORAM ALTERADOS
        if ((this.flagEdicao === true) || (this.flagRiscos === true) || (this.flagNr === true)) {
          this.inventarioAtualSalvo.Ativo = false;
        } else {
          this.inventarioAtualSalvo.Ativo = true;
        }
        this.invService.temAPR(this.inventarioRecebido.CodInventarioAmbiente).then((data) => {
          if (data === 0) {
            this.abrirPopUp(2, 'Deseja editar este inventario de ambiente?');
          } else {
            this.abrirPopUp(2, 'Esse inventario possui APR associada, deseja editar mesmo assim?');
          }
        });
      }

    }
  }


  setDadosParaSalvar() {
    if (this.ambienteSelecionado !== undefined) {
      this.inventarioAtualSalvo.CodAmbiente = this.ambienteSelecionado.CodAmbiente;
    }
    this.inventarioAtualSalvo.setDescricao(this.descricaotextArea);
    this.inventarioAtualSalvo.setObservacaoGeral(this.observacaotextArea);
    this.converterNrs(this.todosNRs);
    this.inventarioAtualSalvo.setRiscoGeral(this.maiorRiscoGeral);
    if (!this.isCadastro) {
        this.inventarioAtualSalvo.CodInventarioAmbiente = this.inventarioRecebido.CodInventarioAmbiente;
        this.inventarioAtualSalvo.Codigo = this.inventarioRecebido.Codigo;
      }
  }
  setDadosParaSalvarRascunho() {
    if (this.ambienteSelecionado !== undefined) {
      this.inventarioRascunho.CodAmbiente = this.ambienteSelecionado.CodAmbiente;
    }
    this.inventarioRascunho.Descricao = this.descricaotextArea;
    this.inventarioRascunho.ObservacaoGeral = this.observacaotextArea;
    this.converterNrsRascunhos(this.todosNRs);
    this.inventarioRascunho.RiscoGeral = this.maiorRiscoGeral;


  }

  salvarComoRascunho() {
    this.setDadosParaSalvarRascunho();
    this.inventarioRascunho.setLocalInventario(this.nodosSelecionados);
    if (this.isRascunho) {
      this.inventarioRascunho.novoInventario = false;
      this.invService.editarRascunho(this.inventarioRascunho).then((data) => {
        this.redirecionarTelaGestao();
      });
      return;
    }
    this.invService.inserirRascunho(this.inventarioRascunho).then((data) => {
      this.redirecionarTelaGestao();
    });
  }


  deletarInventario() {
    if (this.isRascunho) {
      this.confirmarExclusaoPopup(2, 'Deseja excluir o rascunho?');
    } else {
      this.invService.temAPR(this.inventarioRecebido.CodInventarioAmbiente).then((data) => {
        if (data === 0) {
          this.confirmarExclusaoPopup(2, 'Deseja excluir o inventário?');
        } else {
          this.excluirInventarioComAPR(2, 'Esse inventario possui APR associada, deseja excluir mesmo assim?');
        }

      });
    }
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
            CodInventarioAmbiente: this.inventarioRecebido.CodInventarioAmbiente,
            EightIDUsuarioModificador: this.user,
          };
          this.invService.deletar(modelDelete).then(() => {
            this.redirecionarTelaGestao();
          }).catch(() => {});
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
      if (result === 'CONFIRMAR') {
        if (this.isRascunho) {
          this.invService.deletarRascunho(this.inventarioRascunho.CodRascunhoInventarioAmbiente).then(() => {
            this.redirecionarTelaGestao();
          }).catch(() => {});
        } else {
          const modelDelete = {
            CodInventarioAmbiente: this.inventarioRecebido.CodInventarioAmbiente,
            EightIDUsuarioModificador: this.user,
          };
          this.invService.deletar(modelDelete).then(() => {

            this.redirecionarTelaGestao();
          }).catch(() => {});
        }
      }
    });
  }


  converterNrs(todosNRs: NrModelTela[]) {
    this.NrsSelecionados = [];
    this.nrImpressao = [];
    todosNRs.forEach((nr) => {
      if (nr.checked === true) {
        this.nrImpressao.push(nr);
        const nRModel = new NRModel(nr.CodNR);
        this.NrsSelecionados.push(nRModel);
      }
    });
    if (this.nrNaoAplicavel === true) {
      const nRModel = new NRModel(todosNRs[0].CodNR);
      this.NrsSelecionados.push(nRModel);
    } else if (this.NrsSelecionados.length === 0) {
      this.nrNaoAplicavel = true;
      const nRModel = new NRModel(todosNRs[0].CodNR);
      this.NrsSelecionados.push(nRModel);
    }
    this.inventarioAtualSalvo.setNrInventario(this.NrsSelecionados);
  }
  converterNrsRascunhos(todosNRs: NrModelTela[]) {
    const NrsSelecionadosRascunho: NR_Rascunho[] = [];
    todosNRs.forEach((nr) => {
      if (nr.checked === true) {
        const nRModel = new NR_Rascunho(nr.CodNR);
        NrsSelecionadosRascunho.push(nRModel);
      }
    });
    if (this.nrNaoAplicavel === true) {
      const nRModel = new NRModel(todosNRs[0].CodNR);
      this.NrsSelecionados.push(nRModel);
    } else if (NrsSelecionadosRascunho.length === 0) {
      const nRModel = new NRModel(todosNRs[0].CodNR);
      this.NrsSelecionados.push(nRModel);
      this.nrNaoAplicavel = true;
    }
    this.inventarioRascunho.NR_RASCUNHO_INVENTARIO_AMBIENTE = [...NrsSelecionadosRascunho];
  }


  changeCheckBox() {
    this.flagNr = true;
    if (this.nrNaoAplicavel === true) {
      this.nrNaoAplicavel = false;
    }
  }


  changeNR() {
    if (this.nrNaoAplicavel === true) {
      // tslint:disable-next-line:prefer-for-of
      for (let j = 0; j < this.todosNRs.length; j++) {
        this.todosNRs[j].checked = false;
      }
    } else {
      this.nrNaoAplicavel = false;
    }
  }
  validarForms() {
    if (this.ambienteSelecionado === undefined) {
     this.abrirPopUp(0, 'Insira um AMBIENTE para cadastrar inventario');
     return false;
    }
    if ((this.inventarioAtualSalvo.RISCO_INVENTARIO_AMBIENTE.length === 0) &&
    (this.inventarioRascunho.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.length === 0) ) {
      this.abrirPopUp(0, 'Insira pelo menos um RISCO para cadastrar inventario');
      return false;
    }
    if ((this.inventarioAtualSalvo.LOCAL_INSTALACAO_MODELO.length === 0) &&
    this.inventarioRascunho.LOCAL_INSTALACAO_MODELO.length === 0) {
      this.abrirPopUp(0, 'Insira pelo menos um LOCAL DE INSTALAÇÃO para cadastrar inventario');
      return false;
    }
    return true;
  }
  abrirPopUp(tipo: number, mensagem: string, locais?: string[]) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.disableClose = true;
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem, locaisInstalacao: locais};
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'CONFIRMAR') {
        this.invService.editar(this.inventarioAtualSalvo).then((data) => {
          if (data.status === false) {
            this.abrirPopUp(5, 'Os seguintes locais de instalaçao ja tem inventario de ambiente associado', data.localModelo);
          } else {
            this.redirecionarTelaGestao();
          }
        });

      }
      return result;

    });
  }
}
