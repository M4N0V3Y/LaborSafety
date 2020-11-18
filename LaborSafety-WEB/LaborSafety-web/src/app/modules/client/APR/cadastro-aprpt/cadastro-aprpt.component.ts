import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatTable, MatDialogConfig, MatDialog } from '@angular/material';
import { OperacaoComponent, RiscoApr } from '../../operacao/operacao.component';
import { OperacaoModel } from 'src/app/shared/models/OperacaoModel';
import { PessoaModel } from 'src/app/shared/models/PessoaModel';
import { PopUpPessoasComponent } from '../../pop-up-pessoas/pop-up-pessoas.component';
import { AprServiceService } from 'src/app/core/http/apr/apr-service.service';
import { Router, ActivatedRoute } from '@angular/router';
import * as jsPDF from 'jspdf';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { Tela } from 'src/app/shared/models/tela.model';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';

class ModeloAPRBack {
  CodAPR: number;
  CodStatusAPR: number;
  NumeroSerie: string;
  OrdemManutencao: string;
  Descricao: string;
  RiscoGeral = 1;
  RISCO_APR?: [];
  FOLHA_ANEXO_APR?: [];
  Observacao: string;
  OPERACAO_APR: OperacaoAPR[] = [];
  APROVADOR_APR: AprovadorAPR[] = [];
  EXECUTANTE_APR: ExecutanteAPR[] = [];
  Ativo: true;
  AprEditavel: true;
  EightIDUsuarioModificador: string;
  constructor(Descricao: string, RiscoGeral: number, Observacao: string,  CodStatusAPR: number ) {
                this.Descricao = Descricao;
                this.RiscoGeral = RiscoGeral;
                this.Observacao = Observacao;
                this.CodStatusAPR =  CodStatusAPR;
  }
}
class AprovadorAPR {
  CodAprovadorApr?: number;
  CodPessoa: number;
  CodApr?: number;
  Ativo?: boolean;
  constructor(CodPessoa: number) {
    this.CodPessoa = CodPessoa;
  }
}
class ExecutanteAPR {
  CodExecutanteApr?: number;
  CodPessoa: number;
  CodApr?: number;
  Ativo?: boolean;
  constructor(CodPessoa: number) {
    this.CodPessoa = CodPessoa;
  }
}
class OperacaoRetornoBack {
  CodOperacaoAPR: number;
  CodAPR: number;
  CodStatusAPR: number;
  Codigo: string;
  Descricao: string;
  CodLI: number;
  NomeLI: string;
  CodDisciplina: number;
  NomeDisciplina: string;
  CodAtvPadrao: number;
  NomeAtvPadrao: string;
  RiscoGeral: number;
}

class OperacaoAPR {
  CodLI: number;
  CodOperacaoAPR: number;
  Descricao: string;
  CodAtvPadrao: number;
  CodDisciplina: number;
  NomeLI: string;
  CodStatusAPR = 1;
  Codigo?: string;
  RiscoGeral = 0;
  Ativo?: boolean;
  constructor(Descricao: string, CodAtividadePadrao: number, CodDisciplina: number,
              LocalInstalacao: string, CodLI: number) {
    this.Descricao = Descricao;
    this.CodAtvPadrao = CodAtividadePadrao;
    this.CodDisciplina = CodDisciplina;
    this.NomeLI = LocalInstalacao;
    this.CodLI = CodLI;
  }

}

@Component({
  selector: 'app-cadastro-aprpt',
  templateUrl: './cadastro-aprpt.component.html',
  styleUrls: ['./cadastro-aprpt.component.scss']
})
export class CadastroAprptComponent implements OnInit {
  @ViewChild('tableOperacao') table1: MatTable<any>;
  @ViewChild('tableExecutor') table2: MatTable<any>;
  @ViewChild('tableAprovador') table3: MatTable<any>;
  @ViewChild('content') content: ElementRef;
  displayedColumns: string[] = ['Descrição', 'Local de Instalação', 'Atividade', 'Disciplina', 'Risco', 'Controles'];
  displayedColumnsPessoa: string[] = ['Matricula', 'Nome', 'CPF', 'Telefone', 'Email', 'Empresa', 'Controles'];
  todasOperacoes: OperacaoModel[] = [];
  todosExecutores: PessoaModel[] = [];
  todosAprovadores: PessoaModel[] = [];
  descricaoTextArea: string;
  isCadastro: boolean;
  numeroDeSerie = '';
  OrdemManutencao = '';
  retornoPopUp: boolean;
  aprRecebida: ModeloAPRBack;
  riscosGerais: number[] = [];
  expanded = false;
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  desabilitarButtonImpressao = true;
  desabilitarButtonEdicao = true;
  desabilitarButtonExcluir = true;
  desabilitarDesassociacaoTreeview = true;
  isLinkImpressao = false;
  isManual = true;
  AprAtiva: boolean;

  constructor(private service: AprServiceService, private auth: AuthenticationService,
              private dialog: MatDialog, private router: Router,  private route: ActivatedRoute) {
    if (!this.auth.isLogin()) {
      this.route.queryParams.subscribe((queryParams) => {
        if ((queryParams.CodAPR !== undefined)) {
          const model = {
            CodAPR: queryParams.CodAPR
          };
          router.navigate(['/auth/login'], {
            queryParams: model,
          });
        } else {
          router.navigate(['/auth/login']);
        }
      });
      return;
    } else {
      this.route.queryParams.subscribe( async (queryParams) => {
        if ((queryParams.CodAPR !== undefined)) {
          this.isCadastro = false;
          if (queryParams.isLinkImpressao !== undefined) {
            // 'queryParams.NumeroSerie === undefined' para garantir que o usuario nao mude a variavel isLinkImpressao na url
            this.isLinkImpressao = true;
          }
          this.abrirPopUp(6, 'Aguarde, estamos carregando a APR ...');
          await this.service.getAPRPorID(queryParams.CodAPR).then((data: ModeloAPRBack) => {
            this.dialog.closeAll();
            this.aprRecebida = data;
            this.AprAtiva = this.aprRecebida.Ativo;
            this.numeroDeSerie = data.NumeroSerie;
            this.OrdemManutencao = data.OrdemManutencao;
            this.descricaoTextArea = data.Descricao;
            this.converterCodemModelo(data.OPERACAO_APR, data.EXECUTANTE_APR, data.APROVADOR_APR);
          }).catch(() => {
            this.redirecionarTelaGestao();
          });
        } else {
          this.AprAtiva = true;
          this.isCadastro = true;
        }
        await this.setPermissoes();
        this.verificaStatusAPR(this.aprRecebida);
      });
    }
  }

  ngOnInit() {
  }
  async setPermissoes() {
    if (this.isCadastro !== true) {
      await this.service.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
        .then((lista: Tela[]) => {
          this.listaTelasEFuncionalidadesPorPerfil = [...lista];
          this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {
            if (tela.CodTela === 4) {
              tela.Funcionalidades.forEach((funcionalidade) => {
                this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
              });
            }
          });
        });
    } else {
      this.desabilitarButtonEdicao = false;
    }
  }

  verificaStatusAPR(apr: ModeloAPRBack) {
    if (!this.isCadastro) {
      if (!apr.NumeroSerie.includes('M')) {
        this.desabilitarButtonEdicao = true;
      }
      if (apr.CodStatusAPR !== 2) {
        this.desabilitarButtonImpressao = true;
      }
      if (this.aprRecebida.OPERACAO_APR.length < 1) {
        this.desabilitarButtonImpressao = true;
      }
      if (this.AprAtiva === false) {
        this.desabilitarButtonEdicao = true;
      }
    }
  }

  verificaFuncionalidades(codFuncionalidade) {
    switch (codFuncionalidade) {
      case 2:
        this.desabilitarButtonImpressao = false;
        break;
      case 3:
        if (!this.isLinkImpressao) {
          this.desabilitarButtonEdicao = false;
        }
        break;
      case 4:
        this.desabilitarButtonExcluir = false;
        break;
      case 5:
        break;
    }
  }

  converterCodemModelo(operacoes: OperacaoAPR[], executores: ExecutanteAPR[], aprovadores: AprovadorAPR[]) {
    const listaDeCodExecutores = [];
    const listaDeCodAprovadores = [];
    const listaDeCodOperaçoes = [];
    executores.forEach((executor) => {
      if (executor.Ativo === true) {
        listaDeCodExecutores.push(executor.CodPessoa);
      }
    });
    aprovadores.forEach((aprovador) => {
      if (aprovador.Ativo === true) {
        listaDeCodAprovadores.push(aprovador.CodPessoa);
      }
    });
    operacoes.forEach((operacao) => {
      if (operacao.Ativo === true) {
        listaDeCodOperaçoes.push(operacao.CodOperacaoAPR);
      }
    });
    this.service.getPessoasPorIDs(listaDeCodExecutores).then((data) => {
      this.todosExecutores = [...data];

    });
    this.service.getPessoasPorIDs(listaDeCodAprovadores).then((data) => {
      this.todosAprovadores = [...data];
    });
    this.service.getOperacoesPorIDs(listaDeCodOperaçoes).then((operacoes) => {
      const operacaoModel: OperacaoModel[] = [];
      operacoes.forEach((operacao: OperacaoRetornoBack) => {
        const model = new OperacaoModel();
        if(operacao.Codigo !== undefined){
          model.Codigo = operacao.Codigo;
        }
        model.CodOperacaoAPR = operacao.CodOperacaoAPR;
        model.descricao = operacao.Descricao;
        model.setAtividade(operacao.CodAtvPadrao, operacao.NomeAtvPadrao);
        model.setDisciplina(operacao.CodDisciplina, operacao.NomeDisciplina);
        model.setLI(operacao.NomeLI, operacao.CodLI);
        model.CodLI = operacao.CodLI;
        operacaoModel.push(model);
      });
      this.todasOperacoes = [...operacaoModel];
      this.calculaRiscoOperacao();
    });

  }
  baixarModelo() {
    this.service.getModeloAPR().then((modelo) => {
      const linkSource = 'data:application/xls;base64,' + modelo;
      const downloadLink = document.createElement('a');
      const fileName = 'ModeloAPR.xls';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();
    });
  }
  async calculaRiscoOperacao() {
    this.todasOperacoes.forEach(async (operacao) => {
      const modelCalculo = new RiscoApr();
      modelCalculo.CodAtividade = operacao.atividade.CodAtividadePadrao;
      modelCalculo.CodDisciplina = operacao.disciplina.CodDisciplina;
      modelCalculo.CodLi = operacao.localDeInstalacao.CodLocalInstalacao;
      modelCalculo.AprAtiva = this.aprRecebida.Ativo;
      modelCalculo.CodApr = this.aprRecebida.CodAPR;
      await this.service.calcularRiscoGeral(modelCalculo).then((risco) => {
        operacao.riscoOperacao = risco;
        this.riscosGerais.push(operacao.riscoOperacao);
      });
    });
  }

  baixarLog() {
    const id = [];
    id.push(this.aprRecebida.CodAPR);
    this.service.gerarLog(id).then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_APR_' + this.aprRecebida.CodAPR + '_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }

  popUpImpressao() {
   if (this.aprRecebida.OrdemManutencao == null || this.aprRecebida.OrdemManutencao === undefined || 
    this.aprRecebida.OrdemManutencao === '') {
      this.abrirPopUp(0, 'Não é possivel gerar um arquivo de impressão APR sem uma Ordem de Manutenção cadastrada');
    } else{
      this.abrirPopUp(2, 'Deseja imprimir essa APR?');
    }

  }
  async imprimir() {
    this.service.getBase64(this.numeroDeSerie).then((base64) => {
      const byteArray = new Uint8Array(atob(base64).split('').map(char => char.charCodeAt(0)));
      return new Blob([byteArray], {type: 'application/pdf'});
    }).then((blob) => {
      const url = URL.createObjectURL(blob);
      window.open(url, '_blank');
    });


    /*this.expanded = true;
    document.getElementById('textareaDescricao').setAttribute('style', document.getElementById('textareaDescricao').getAttribute('style'));
    await this.delay(250);
    document.getElementById('imprimir').click();*/
  }
  async delay(ms: number) {
    await new Promise(resolve => setTimeout(() => resolve(), ms));
  }
  async verificaOrdemManutencao(ordemManutencao: string, codApr: number) {
    return await this.service.verificaOrdemManutencao(ordemManutencao, codApr).then((data) => {
      return data;
    });
  }

  async inserir() {

    if (this.isCadastro === true) {
      if (await this.verificaOrdemManutencao(this.OrdemManutencao, 0)) {
        this.abrirPopUp(0, 'Essa ordem de manutenção ja possui APR associada');
        return;
      }
      const model = this.setDadosInserir();
      model.OrdemManutencao = this.OrdemManutencao;
      if (this.validarForms(model) === true) {
        this.service.inserirAPR(model).then(() => {
          this.redirecionarTelaGestao();
        });

      }
    } else {
      if (await this.verificaOrdemManutencao(this.OrdemManutencao, this.aprRecebida.CodAPR)) {
        this.abrirPopUp(0, 'Essa ordem de manutenção ja possui APR associada');
        return;
      }
      const model = this.setDadosInserir();
      if (this.validarForms(model) === true) {
        this.abrirPopUp(2, 'Deseja editar essa APR?');
      }
    }

  }
  setDadosInserir() {
    const maiorRisco = Math.max.apply(null, this.riscosGerais );
    const model = new ModeloAPRBack(this.descricaoTextArea, 2, '', 1);
    model.EightIDUsuarioModificador = this.auth.getUser();
    const aprovador: AprovadorAPR[] = [];
    const executor: ExecutanteAPR[] = [];
    const operacao: OperacaoAPR[] = [];
    this.todosAprovadores.forEach((data) => {
      aprovador.push(new AprovadorAPR(data.CodPessoa));
    });
    this.todosExecutores.forEach((data) => {
      executor.push(new ExecutanteAPR(data.CodPessoa));
    });
    this.todasOperacoes.forEach((data) => {
      operacao.push(new OperacaoAPR(data.descricao, data.atividade.CodAtividadePadrao,
         data.disciplina.CodDisciplina, data.localDeInstalacao.Nome, data.CodLI));
    });
    model.RiscoGeral = maiorRisco;
    model.APROVADOR_APR = [...aprovador];
    model.EXECUTANTE_APR = [...executor];
    model.OPERACAO_APR = [...operacao];
    return model;
  }

  editarAPR() {
    const model = this.setDadosInserir();
    model.CodAPR = this.aprRecebida.CodAPR;
    model.RISCO_APR = this.aprRecebida.RISCO_APR;
    model.FOLHA_ANEXO_APR = this.aprRecebida.FOLHA_ANEXO_APR;
    model.OrdemManutencao = this.OrdemManutencao;
    model.NumeroSerie = this.numeroDeSerie;
    this.service.editarAPR(model).then(() => {
      this.redirecionarTelaGestao();
    });


  }
  validarForms(model: ModeloAPRBack) {
   /* if (this.OrdemManutencao == null || this.OrdemManutencao === undefined || this.OrdemManutencao === '') {
      this.abrirPopUp(0, 'Insira a ORDEM DE MANUTENÇÃO para cadastrar APR');
      return false;
    }*/
    /*if (this.OrdemManutencao !== '') {
      const ordemString = String(this.OrdemManutencao);
      if(ordemString.length !== 12){
        this.abrirPopUp(0, 'Insira uma Ordem de manutenção com 12 numeros');
        return false;
      }
    }*/
    if (model.OPERACAO_APR.length === 0) {
      this.abrirPopUp(0, 'Insira pelo menos uma OPERAÇÃO para cadastrar APR');
      return false;
    }
    if (model.EXECUTANTE_APR.length === 0) {
      this.abrirPopUp(0, 'Insira pelo menos um EXECUTOR para cadastrar APR');
      return false;
    }
    if (model.APROVADOR_APR.length === 0) {
      this.abrirPopUp(0, 'Insira pelo menos um APROVADOR para cadastrar APR');
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
      if ( (result === 'CONFIRMAR') && ( mensagem === 'Deseja editar essa APR?')) {
        this.editarAPR();
      }

      if ( (result === 'CONFIRMAR') && ( mensagem === 'Deseja imprimir essa APR?')) {
        this.imprimir();
      }
      return result;
    });
  }

  redirecionarTelaGestao() {
    this.router.navigate(['/client/aprpt/']);
  }
  refresh() {
    this.table1.renderRows();
    this.table2.renderRows();
    this.table3.renderRows();
  }
  // OPERACOES
  adicionarOperacao() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '90%';
    dialogConfig.minHeight = '260px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = { id: 0, riscoRecebido: undefined, AprAtivo: this.AprAtiva };

    const dialogRef = this.dialog.open(OperacaoComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.showOperacao(result.data);
      }
    });
  }
  editarOperacao(row: OperacaoModel, posicao: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '90%';
    dialogConfig.minHeight = '260px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = { id: posicao, riscoRecebido: row, AprAtivo: this.AprAtiva};
    const dialogRef = this.dialog.open(OperacaoComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.todasOperacoes.splice(posicao, 1, result.data);
        this.refresh();
      }
    });
  }
  async popUpDeletarOperacao(i: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 2, textoRecebido: 'Deseja excluir a operação?' };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    await dialogRef.afterClosed().subscribe(result => {
      if ( (result === 'CONFIRMAR') ) {
        this.riscosGerais.splice(i, 1);
        this.todasOperacoes.splice(i, 1);
        this.todasOperacoes = [...this.todasOperacoes];
        this.refresh();
      }
      return result;
    });
  }
  deletarOperacao(i: number) {
    this.popUpDeletarOperacao(i);
  }
  showOperacao(operacao: OperacaoModel) {
    this.riscosGerais.push(operacao.riscoOperacao);
    this.todasOperacoes.push(operacao);
    this.todasOperacoes = [...this.todasOperacoes];
    this.riscosGerais = [...this.riscosGerais];
    // this.inventarioAtualSalvo.setRiscoInventario(risco);
    this.refresh();
  }

  // EXECUTOR
  editarExecutor(row: PessoaModel, posicao: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '85%';
    dialogConfig.data = { id: posicao, pessoaRecebida: row };
    const dialogRef = this.dialog.open(PopUpPessoasComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.todosExecutores.splice(posicao, 1, result.data);
        this.refresh();
      }
    });

  }

  deletarExecutor(i: number) {

    this.todosExecutores.splice(i, 1);
    this.todosExecutores = [...this.todosExecutores];
    this.refresh();
  }
  showExecutor(executor: PessoaModel) {
    this.todosExecutores.push(executor);
    this.todosExecutores = [...this.todosExecutores];
    this.refresh();
  }


  adicionarExecutor() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '85%';
    dialogConfig.minHeight = '260px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = {pessoaRecebido: undefined };

    const dialogRef = this.dialog.open(PopUpPessoasComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.tipo !== 'exclusao') {
          this.showExecutor(result.data);
        }
      }
    });
  }

  // APROVADOR
  editarAprovador(row: PessoaModel, posicao: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '85%';
    dialogConfig.data = { id: posicao, pessoaRecebida: row };
    const dialogRef = this.dialog.open(PopUpPessoasComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.tipo === 'exclusao') {
          this.todosAprovadores.splice(posicao, 1);
        }
        this.todosAprovadores.splice(posicao, 1, result.data);
        this.refresh();
      }
    });

  }

  deletarAprovador(i: number) {

    this.todosAprovadores.splice(i, 1);
    this.todosAprovadores = [...this.todosAprovadores];
    this.refresh();
  }
  showAprovador(executor: PessoaModel) {
    this.todosAprovadores.push(executor);
    this.todosAprovadores = [...this.todosAprovadores];
    this.refresh();
  }


  adicionarAprovador() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '65%';
    dialogConfig.height = '85%';
    dialogConfig.minHeight = '260px';
    dialogConfig.minWidth = '350px';
    dialogConfig.data = {pessoaRecebido: undefined };

    const dialogRef = this.dialog.open(PopUpPessoasComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.tipo !== 'exclusao') {
          this.showAprovador(result.data);
        }

      }
    });
  }
}

