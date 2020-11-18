import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig, MatDialog } from '@angular/material';

import { Router } from '@angular/router';
import { DisciplinaService } from 'src/app/core/http/disciplina/disciplina.service';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { AtividadeService } from 'src/app/core/http/atividade/atividade.service';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { OperacaoModel } from 'src/app/shared/models/OperacaoModel';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { AprServiceService } from 'src/app/core/http/apr/apr-service.service';
export interface DialogData {
  id: number;
  riscoRecebido: OperacaoModel;
  AprAtiva?: boolean;
}
export class RiscoApr {
  CodLi: number;
  CodAtividade: number;
  CodDisciplina: number;
  AprAtiva?: boolean;
  CodApr?: number;
}

@Component({
  selector: 'app-operacao',
  templateUrl: './operacao.component.html',
  styleUrls: ['./operacao.component.scss']
})
export class OperacaoComponent implements OnInit {
  disciplinaSelecionada: Disciplina;
  atividadeSelecionada: Atividade;
  descricaoTextArea: string;
  disciplinas: Disciplina[] = [];
  atividades: Atividade[] = [];
  idRiscoRecebido: string;
  teste = 'tsete';
  nodosSelecionados: any;
  nodeRecebido: string[] = [];
  treeviewLoader: boolean;

  constructor(private router: Router, public dialogRef: MatDialogRef<OperacaoComponent>,
              private disciplinaService: DisciplinaService, private service: AprServiceService,
              private atividadeService: AtividadeService, private dialog: MatDialog,
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {

  }

  ngOnInit() {
    this.treeviewLoader = false;
    this.disciplinaService.getAll().then((disciplina) => {
      this.disciplinas = disciplina;
    });
    this.atividadeService.getAll().then((atividade) => {
      this.atividades = atividade;
    });

    if (this.data && this.data.riscoRecebido) {
      const temp = this.data.riscoRecebido.localDeInstalacao.Nome.split(' ').splice(0, 1).join();
      this.nodeRecebido.push(temp);
      this.descricaoTextArea = this.data.riscoRecebido.descricao;
      this.atividadeSelecionada = this.data.riscoRecebido.atividade;
      this.disciplinaSelecionada = this.data.riscoRecebido.disciplina;
      this.dialogRef.updateSize('70%', '90%');
      this.treeviewLoader = true;
    }
    this.treeviewLoader = true;
  }

  retornaSelecionados(selecionados: any) {
    this.nodosSelecionados = selecionados;
  }
  salvar() {
    console.log(this.data.AprAtiva );
    if (this.validarForms()) {
      const operacaoRetorno = new OperacaoModel();
      operacaoRetorno.atividade = this.atividadeSelecionada;
      operacaoRetorno.disciplina = this.disciplinaSelecionada;
      operacaoRetorno.descricao = this.descricaoTextArea;
      operacaoRetorno.localDeInstalacao = this.nodosSelecionados[0];
      operacaoRetorno.CodLI = this.nodosSelecionados[0].CodLocalInstalacao;

      const modelCalculo = new RiscoApr();
      modelCalculo.CodAtividade = operacaoRetorno.atividade.CodAtividadePadrao;
      modelCalculo.CodDisciplina = operacaoRetorno.disciplina.CodDisciplina;
      modelCalculo.CodLi = operacaoRetorno.localDeInstalacao.CodLocalInstalacao;
      modelCalculo.AprAtiva = true ;
      this.service.calcularRiscoGeral(modelCalculo).then((risco: number) => {
        operacaoRetorno.riscoOperacao = risco;
        this.dialogRef.close({ data: operacaoRetorno });
      });
    }

  }
  validarForms() {
    if ((this.atividadeSelecionada === undefined) || (this.atividadeSelecionada == null)) {
      this.abrirPopUp(0, 'Insira uma ATIVIDADE para inserir uma operação');
      return false;
    }
    if ((this.disciplinaSelecionada === undefined) || (this.disciplinaSelecionada == null)) {
      this.abrirPopUp(0, 'Insira uma DISCIPLINA para inserir uma operação');
      return false;
    }
    if (this.nodosSelecionados.length < 1) {
      this.abrirPopUp(0, 'Insira um LOCAL DE INSTALAÇÃO para inserir uma operação');
      return false;
    }
    if (this.nodosSelecionados.length > 1) {
      this.abrirPopUp(0, 'Insira somente UM LOCAL DE INSTALAÇÃO por operação');
      return false;
    }
    return true;
  }
  abrirPopUp(tipo: number, mensagem: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      return result;

    });
  }

  selecionarAtividade(event: any) {
    if (!this.atividades) {
      this.atividadeService.getAll().then((atividadea) => {
        this.atividades = atividadea;
      });
    }
    this.atividades.forEach((atividade) => {
      if (atividade.Nome === event.value) {
        this.atividadeSelecionada = atividade;
      }
    });
  }

  selecionarDisciplina(event: any) {
    if (!this.disciplinas) {
      this.disciplinaService.getAll().then((disciplinas) => {
        this.disciplinas = disciplinas;
      });
    }
    this.disciplinas.forEach((disciplina) => {
      if (disciplina.Nome === event.value) {
        this.disciplinaSelecionada = disciplina;
      }
    });
  }

  selectFilters(selecao, tipoSelecao) {
    switch (tipoSelecao) {
      case 'atividade':
        this.atividadeSelecionada = selecao.value;
        break;
    }
  }
}
