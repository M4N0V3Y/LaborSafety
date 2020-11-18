import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { throwError, Observable } from 'rxjs';
import {ImportacaoService, ArquivoModelo} from 'src/app/core/http/importacao/importacao.service';
import { TreeViewComponent } from '@syncfusion/ej2-ng-navigations';
import { Tela } from 'src/app/shared/models/tela.model';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Router } from '@angular/router';
import { ImportacaoModel, ErrosImportacao } from 'src/app/shared/models/ImportacaoModel';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { NRModel } from 'src/app/shared/models/invAmbinteModel';
import { NrModelTela } from 'src/app/shared/models/NRmodel';
import { stringToKeyValue } from '@angular/flex-layout/extended/typings/style/style-transforms';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { PerfilDeCatalogo } from 'src/app/shared/models/perfildecatalogo.model';


class FiltrosAmbienteTela {
  title: string;
  itens = [];
  constructor(title: string, itens: []) {
    this.title = title;
    const aux = itens.map((data: any) => {
      data.checked = false;
      return data;
    });
    this.itens = aux;
  }
}
class FiltrosAtividadeTela {
  title: string;
  itens: any[];
  constructor(title: string, itens: []) {
    this.title = title;
    const aux = itens.map((data: any) => {
      data.checked = false;
      return data;
    });
    this.itens = aux;
  }
}
class FiltrosAprTela {
  title: string;
  itens = [];
  constructor(title: string, itens: any[]) {
    this.title = title;
    const aux = itens.map((data: any) => {
      data.checked = false;
      return data;
    });
    this.itens = aux;
  }
}
class FiltrosEnviados {
  ordemManutencao?: string;
  numeroDeSerie?: string;
  SEVERIDADE = [];
  RISCO = [];
  ATIVIDADE_PADRAO = [];
  PESO = [];
  PROBABILIDADE = [];
  AMBIENTE = [];
  DISCIPLINA = [];
  PERFIL_CATALOGO = [];
  NR = [];
  LOCAL_INSTALACAO = [];
  EPI = [];
  RISCO_GERAL = [];
}

@Component({
  selector: 'app-importacao',
  templateUrl: './importacao.component.html',
  styleUrls: ['./importacao.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class ImportacaoComponent implements OnInit {
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  file: any;
  flashcardInputExpanded: boolean;
  service: ImportacaoService;
  selectedOptionImportacao: string;
  selectedOptionExportacao: string;
  checkbox: boolean;
  nodosSelecionados: any;
  nodosSelecionadosEPI: any;
  dataSource: any;
  filtrosAmbiente: FiltrosAmbienteTela [] = [];
  filtroAtividade: FiltrosAtividadeTela[] = [];
  filtroApr: FiltrosAprTela[] = [];
  errosImportacao: ErrosImportacao [];
  filtrosEnviados: FiltrosEnviados;
  desabilitarButtonUpload = true;
  desabilitarButtonDownload = true;
  listaTelas: any;
  tabelaTemErro = false;
  todasNRs = [];
  NrsSelecionados = [];
  nrNaoAplicavel: boolean;
  ordemDeManutencaoTextArea: string;
  numeroDeSerieTextArea: string;


  constructor(importacaoService: ImportacaoService, private router: Router,  private dialog: MatDialog,
              private auth: AuthenticationService ) {
    if (!this.verificaLogin()) {
      return;
    }
    this.service = importacaoService;
    this.filtrosEnviados = new FiltrosEnviados();

    this.service.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
    .then((lista: Tela[]) => {
      this.listaTelasEFuncionalidadesPorPerfil = [...lista];
      if (this.verificarPermissaoDeTela() !== true) {
        this.router.navigate(['/notfound']);
      } else {
        this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {

          tela.Funcionalidades.forEach((funcionalidade) => {
                this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
            });
        });
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
  retornaSelecionados(selecionados) {
    this.nodosSelecionados = selecionados;
  }
  retornaSelecionadosEpi(selecionados) {
    this.nodosSelecionadosEPI = selecionados;
  }
  geraRiscosGerais() {
    let riscosGerais: [{
      Nome: number;
    }];
    riscosGerais = [{Nome: 0}];
    for (let i = 1; i < 17; i++) {
      riscosGerais.push({Nome: i});
    }
    riscosGerais.splice(0, 1);
    return riscosGerais;
  }

  ngOnInit() {
    this.carregarFiltros();
  }

 async carregarFiltros() {
   await this.filtroApr.push(new FiltrosAprTela('Risco Geral',  this.geraRiscosGerais()));
   await this.service.getAllAtividadePadrao().then((data: any) => {
    this.filtroAtividade.push(new FiltrosAtividadeTela('Atividades Padrao', data));
  });
   await this.service.getAllPesos().then((data: any) => {
    this.filtroAtividade.push(new FiltrosAtividadeTela('Pesos', data));
  });
   await this.service.getAllSeveridades().then((data: any) => {
    this.filtroAtividade.push(new FiltrosAtividadeTela('Severidades', data));
    this.filtrosAmbiente.push(new FiltrosAmbienteTela('Severidades', data));
    });
   await this.service.getAllProbabilidades().then((data: any) => {
    this.filtrosAmbiente.push(new FiltrosAmbienteTela('Probabilidades', data));
    });
   await this.service.getAllSistemasOperacionais().then((data: any) => {
    this.filtrosAmbiente.push(new FiltrosAmbienteTela('Ambiente', data));
    });
   await this.service.getAllRiscos().then((data: any) => {
    const riscosAtividade = data.filter((risco) => {
      if (risco.CodTipoRisco === 1) {
        return risco;
      }
    });
    const riscosAmbiente = data.filter((risco) => {
      if (risco.CodTipoRisco !== 1) {
        return risco;
      }
    });
    this.filtroAtividade.push(new FiltrosAtividadeTela('Riscos', riscosAtividade));
    this.filtrosAmbiente.push(new FiltrosAmbienteTela('Riscos', riscosAmbiente));
    this.filtroApr.push(new FiltrosAprTela('Riscos', data));
    });
   await this.service.getAllDisciplina().then((data: any) => {
    this.filtroAtividade.push(new FiltrosAtividadeTela('Disciplinas', data));
    });

   await this.service.getAllPerfilCatalogo().then((data: any) => {
    this.filtroAtividade.push(new FiltrosAtividadeTela('Perfis de Catalago', data));
    });
   await this.service.listarTodasNRs().then((Nrs) => {
      this.todasNRs = [...Nrs];
    });
  }
  verificarPermissaoDeTela() {
    let permissao = false;
    if (this.listaTelasEFuncionalidadesPorPerfil) {
        this.listaTelasEFuncionalidadesPorPerfil.forEach(
        (tela) => {
          if (5 === tela.CodTela) {
            permissao = true;
          }
        });
    }
    return permissao;
  }
  verificaFuncionalidades(codFuncionalidade) {
    switch (codFuncionalidade) {
      case 6:
        this.desabilitarButtonUpload = false;
        break;
      case 7:
        this.desabilitarButtonDownload = false;
        break;
    }
  }
  selecionarArquivo(event) {// abre o seletor de arquivo
    this.file = event;
  }
  importar() {
    try {
      let arquivoBase64;
      const file = this.file.target.files[0];
      const reader = new FileReader();
      if (this.selectedOptionImportacao !== undefined && this.file.srcElement.files !== undefined) {
        if (this.file.srcElement.files.length === 0) {
          this.abrirPopUp(0, 'Ocorreu um erro ao escolher arquivo, Tente novamente');
        } else {
          reader.onloadend = (e) => {
            const binaryString = reader.result;
            if (binaryString === undefined || binaryString === null) {
              this.abrirPopUp(0, 'Ocorreu um erro ao escolher arquivo, Tente novamente');
              return;
            }
            arquivoBase64 = btoa(binaryString.toString());
            const arquivo = new ArquivoModelo(arquivoBase64, this.auth.getUser());
            if (this.selectedOptionImportacao === 'Ambiente') {
              this.service.inserirPlanilhaAmbiente(arquivo).then((data: ImportacaoModel) => {
                if (!data.status) {
                  this.tabelaTemErro = true;
                  this.errosImportacao = [...data.erros];
                  this.abrirPopUp(4, 'Erro ao inserir Planilha de Inventario de Ambiente', this.errosImportacao);
                }
              });
            } else {
              this.service.inserirPlanilhaAtividade(arquivo).then((data) => {
                if (!data.status) {
                  this.tabelaTemErro = true;
                  this.abrirPopUp(4, 'Erro ao inserir Planilha de Inventario de Atividade', data.erros);
                }
              });
            }
          };
          reader.readAsBinaryString(file);
        }
      }
    } catch (e) {
    }
  }

  changeTipoInvntario() {
   this.resetFiltros();
  }
  resetFiltros() {
    this.filtrosEnviados.ATIVIDADE_PADRAO = [];
    this.filtrosEnviados.DISCIPLINA = [];
    this.filtrosEnviados.PERFIL_CATALOGO = [];
    this.filtrosEnviados.PESO = [];
    this.filtrosEnviados.PROBABILIDADE = [];
    this.filtrosEnviados.RISCO = [];
    this.filtrosEnviados.SEVERIDADE = [];
    this.filtrosEnviados.AMBIENTE = [];
  }
  setListaCodEpi() {
    const listaCodEpi = [];
    this.nodosSelecionadosEPI.forEach((epi) => {
      listaCodEpi.push(epi.CodEPI);
    });
    return listaCodEpi;
  }

  exportar() {
    this.filtrosEnviados.LOCAL_INSTALACAO = [...this.setListaCodLocalInstalacao()];
    this.filtrosEnviados.EPI = [...this.setListaCodEpi()];
    if (this.selectedOptionExportacao === 'Ambiente') {
      this.filtrosEnviados.NR = [...this.converterNrs(this.todasNRs)];
      this.service.exportarDadosAmbiente(this.filtrosEnviados).then((data) => {
        const linkSource = 'data:application/xls;base64,' + data.planilha;
        const downloadLink = document.createElement('a');
        const date = new Date();
        const fileName = 'Dados_inventario_ambiente_' + new Date().toLocaleDateString() + '.xls';
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
      });
    } else if (this.selectedOptionExportacao === 'Atividade') {
      this.service.exportarDadosAtividade(this.filtrosEnviados).then((data) => {
        const linkSource = 'data:application/xls;base64,' + data.planilha;
        const downloadLink = document.createElement('a');
        const date = new Date();
        const fileName = 'Dados_inventario_atividade_' + new Date().toLocaleDateString() + '.xls';
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
      });
    } else {
      this.filtrosEnviados.numeroDeSerie = this.numeroDeSerieTextArea;
      this.filtrosEnviados.ordemManutencao = this.ordemDeManutencaoTextArea;
      this.service.exportarDadosAPR(this.filtrosEnviados).then((data) => {
        const linkSource = 'data:application/xls;base64,' + data.planilha;
        const downloadLink = document.createElement('a');
        const date = new Date();
        const fileName = 'Dados_APR_' + new Date().toLocaleDateString() + '.xls';
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
      });
    }

  }
  setListaCodLocalInstalacao() {
    const listaCodLI: number [] = [];
    this.nodosSelecionados.forEach(element => {
      listaCodLI.push(element.CodLocalInstalacao);
    });
    return listaCodLI;
  }

  baixarModelo() {
    if (this.selectedOptionImportacao === undefined || this.selectedOptionImportacao == null) {
      this.abrirPopUp(0, 'Escolha um tipo de inventario para baixar o modelo');
      return;
    }

    if (this.selectedOptionImportacao === 'Ambiente') {
        this.service.getModeloImportacaoAmbiente().then((modelo) => {
      const linkSource = 'data:application/xls;base64,' + modelo;
      const downloadLink = document.createElement('a');
      const fileName = 'ModeloImportacaoAmbiente.xls';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();
    });
    } else {
      this.service.getModeloImportacaoAtividade().then((modelo) => {
        const linkSource = 'data:application/xls;base64,' + modelo;
        const downloadLink = document.createElement('a');
        const fileName = 'ModeloImportacaoAtividade.xls';
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
      });
    }
  }

  entrarDadosTreeView() {
    if (this.dataSource !== undefined) {
      return this.dataSource;
    }
  }
  abrirPopUp(tipo: number, mensagem: string, errosRecebido?: ErrosImportacao[]) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    // dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.disableClose = true;
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem, erros: errosRecebido};
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (mensagem === 'Ocorreu um erro ao escolher arquivo, Tente novamente') {
        window.location.reload();
      }
      return result;

    });
  }

  adicionarFiltros(title: string, data: any) {
    switch (title) {
      case 'Atividades Padrao':
        if (this.filtrosEnviados.ATIVIDADE_PADRAO.includes(data.CodAtividadePadrao)) {
          this.filtrosEnviados.ATIVIDADE_PADRAO.
          splice(this.filtrosEnviados.ATIVIDADE_PADRAO.indexOf(data.CodAtividadePadrao), 1);
        } else {
         this.filtrosEnviados.ATIVIDADE_PADRAO.push(data.CodAtividadePadrao);
        }
        break;
      case 'Pesos':
        if (this.filtrosEnviados.PESO.includes(data.CodPeso)) {
          this.filtrosEnviados.PESO.
          splice(this.filtrosEnviados.PESO.indexOf(data.CodPeso), 1);
        } else {
         this.filtrosEnviados.PESO.push(data.CodPeso);
        }
        break;
      case 'Severidades':
        if (this.filtrosEnviados.SEVERIDADE.includes(data.CodSeveridade)) {
          this.filtrosEnviados.SEVERIDADE.
          splice(this.filtrosEnviados.SEVERIDADE.indexOf(data.CodSeveridade), 1);
        } else {
         this.filtrosEnviados.SEVERIDADE.push(data.CodSeveridade);
        }
        break;
      case 'Probabilidades':
        if (this.filtrosEnviados.PROBABILIDADE.includes(data.CodProbabilidade)) {
          this.filtrosEnviados.PROBABILIDADE.
          splice(this.filtrosEnviados.PROBABILIDADE.indexOf(data.CodProbabilidade), 1);
        } else {
         this.filtrosEnviados.PROBABILIDADE.push(data.CodProbabilidade);
        }
        break;
      case 'Ambiente':
        if (this.filtrosEnviados.AMBIENTE.includes(data.CodAmbiente)) {
          this.filtrosEnviados.AMBIENTE.
          splice(this.filtrosEnviados.AMBIENTE.indexOf(data.CodAmbiente), 1);
        } else {
         this.filtrosEnviados.AMBIENTE.push(data.CodAmbiente);
        }
        break;
      case 'Riscos':
        if (this.filtrosEnviados.RISCO.includes(data.CodRisco)) {
          this.filtrosEnviados.RISCO.
          splice(this.filtrosEnviados.RISCO.indexOf(data.CodRisco), 1);
        } else {
         this.filtrosEnviados.RISCO.push(data.CodRisco);
        }
        break;
      case 'Disciplinas':
        if (this.filtrosEnviados.DISCIPLINA.includes(data.CodDisciplina)) {
          this.filtrosEnviados.DISCIPLINA.
          splice(this.filtrosEnviados.DISCIPLINA.indexOf(data.CodDisciplina), 1);
        } else {
         this.filtrosEnviados.DISCIPLINA.push(data.CodDisciplina);
        }
        break;
      case 'Perfis de Catalago':

        if (this.filtrosEnviados.PERFIL_CATALOGO.includes(data.CodPerfilCatalogo)) {
          this.filtrosEnviados.PERFIL_CATALOGO.
          splice(this.filtrosEnviados.PERFIL_CATALOGO.indexOf(data.CodPerfilCatalogo), 1);
          // data.checked = false;
        } else {
         this.filtrosEnviados.PERFIL_CATALOGO.push(data.CodPerfilCatalogo);
         // data.checked = true;
        }
        break;
      case 'Risco Geral':
        if (this.filtrosEnviados.RISCO_GERAL.includes(data.Nome)) {
          this.filtrosEnviados.RISCO_GERAL.
          splice(this.filtrosEnviados.RISCO_GERAL.indexOf(data.Nome), 1);
        } else {
          this.filtrosEnviados.RISCO_GERAL.push(data.Nome);
        }
    }

  }

  changeCheckBox() {

    if (this.nrNaoAplicavel === true) {
      this.nrNaoAplicavel = false;
    }
  }
  changeNR() {
    if (this.nrNaoAplicavel === true) {
      // tslint:disable-next-line:prefer-for-of
      for (let j = 0; j < this.todasNRs.length; j++) {
        this.todasNRs[j].checked = false;
      }
    } else {
      this.nrNaoAplicavel = false;
    }
  }
  converterNrs(todosNRs: NrModelTela[]) {
    this.NrsSelecionados = [];
    todosNRs.forEach((nr) => {
      if (nr.checked === true) {
        const nRModel = nr.CodNR;
        this.NrsSelecionados.push(nRModel);
      }
    });
    if (this.nrNaoAplicavel === true) {
      const nRModel = todosNRs[0].CodNR;
      this.NrsSelecionados.push(nRModel);
    }
    return this.NrsSelecionados;
  }

}
