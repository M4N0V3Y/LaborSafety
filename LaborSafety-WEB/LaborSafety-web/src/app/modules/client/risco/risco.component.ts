import { Component, OnInit, Inject, ViewEncapsulation, ViewChild, OnChanges } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Risco } from 'src/app/shared/models/risco.model';
import { RiscoService } from 'src/app/core/http/risco/risco.service';
import { Router } from '@angular/router';
import { SeveridadeService } from 'src/app/core/http/severidade/severidade.service';
import { Severidade } from 'src/app/shared/models/severidade.model';
import { Frequencia } from 'src/app/shared/models/frequencia.model';
import { FrequenciaService } from 'src/app/core/http/frequencia/frequencia.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { RiscoMapeadoModelo } from 'src/app/shared/models/riscomapeado.model';
import { ContraMedidaRisco } from 'src/app/shared/models/contramedidarisco.model';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { InvAmbienteService } from 'src/app/core/http/inventarioAmbiente/inv-ambiente.service';
import { AtividadeService } from 'src/app/core/http/atividade/atividade.service';
import { EPI } from 'src/app/shared/models/EPI.model';

enum telaEnum {
  INV_AMB = 0,
  INV_ATV = 1
}

export interface DialogData {
  id: number;
  riscoRecebido: RiscoMapeadoModelo;
  tela: number;
  atv: {
    CodAtividade: number,
    CodDisciplina: number,
    CodDuracao: number,
    CodPeso: number,
  };
  disabledEdicao: boolean;
}

@Component({
  selector: 'app-risco',
  templateUrl: './risco.component.html',
  styleUrls: ['./risco.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RiscoComponent implements OnInit {

  estado = 0;
  riscoSelecionado: Risco;
  riscos: Risco[] = [];
  severidades: Severidade[] = [];
  severidadeSelecionada: Severidade;
  frequencias: Frequencia[] = [];
  frequenciaSelecionada: Frequencia;
  fGtextArea = '';
  pDtextArea = '';
  cMtextArea = '';
  idRiscoRecebido: string;
  telaAtual: number;
  listaStringEPI: string[] = [];
  EPISelecionado: EPI[] = [];
  EPIRecebido: EPI[] = [];
  treeviewLoader = true;
  disabledEdicao: boolean;
  constructor(public riscoService: RiscoService, private router: Router, private service: InvAmbienteService,
              private dialog: MatDialog, public invAmbService: InvAmbienteService, public invAtvService: AtividadeService,
              public dialogRef: MatDialogRef<RiscoComponent>, public severidadeService: SeveridadeService,
              @Inject(MAT_DIALOG_DATA) public data: DialogData, public frequenciaService: FrequenciaService) { }



  async ngOnInit() {
    this.riscoService.get(this.data.id).then((risco) => { this.riscos = risco; });
    this.severidadeService.getAll().then((severidade) => { this.severidades = severidade; });
    this.frequenciaService.getAll().then((frequencia) => { this.frequencias = frequencia; });
    this.disabledEdicao = this.data.disabledEdicao;
    if (this.data.riscoRecebido) {
      this.treeviewLoader = false;
      await this.setEPI(this.data.riscoRecebido.EPI);
      this.EPISelecionado = this.data.riscoRecebido.EPI;
      this.idRiscoRecebido = this.data.riscoRecebido.id;
      this.riscoSelecionado = this.data.riscoRecebido.Risco;
      this.severidadeSelecionada = this.data.riscoRecebido.Severidade;
      this.frequenciaSelecionada = this.data.riscoRecebido.Frequencia;
      this.fGtextArea = this.data.riscoRecebido.FonteGeradora;
      this.pDtextArea = this.data.riscoRecebido.Procedimentos;
      this.cMtextArea = this.data.riscoRecebido.Contramedidas;
      this.dialogRef.updateSize('65%', '90%');
      this.estado = 2;
    }
    switch (this.data.tela) {
      case telaEnum.INV_AMB:
      case telaEnum.INV_ATV:
        this.telaAtual = this.data.tela;
        break;
      default:
        const dialogConfig = new MatDialogConfig();
        dialogConfig.autoFocus = true;
        dialogConfig.minWidth = '400px';
        dialogConfig.panelClass = 'custom-dialog-container';
        dialogConfig.data = { tipoPopup: 0, textoRecebido: 'Tipo de inventário não identificado!' };
        this.dialog.open(PopupComponent, dialogConfig);
    }
  }
    async setEPI(EPIs: EPI[]) {
      const listaIDs = [];

      EPIs.forEach((epi) => {
        listaIDs.push(epi.CodEPI);
      });

      await this.service.getNomeEPIporId(listaIDs).then((epis) => {
        epis.forEach(epi => {
          this.listaStringEPI.push(epi.Nome);
        });
        this.treeviewLoader = true;
      });
  }

  selecionarRisco(event: any) {
    this.riscos.forEach(risco => {
      if (risco.Nome === event.value) {
        this.riscoSelecionado = risco;
      }
    });
  }

  selecionarSeveridade(event: any) {
    this.severidades.forEach(severidade => {
      if (severidade.Nome === event.value) {
        this.severidadeSelecionada = severidade;
      }
    });
  }

  selecionarFrequencia(event: any) {
    this.frequencias.forEach(frequencia => {
      if (frequencia.Nome === event.value) {
        this.frequenciaSelecionada = frequencia;
      }
    });
  }

  proximo() {
    if (this.riscoSelecionado) {
      this.dialogRef.updateSize('65%', '90%');
      this.estado = 1;

    }
  }

  voltar() {
    this.dialogRef.updateSize('50%', '45%');
    this.estado = 0;
  }

  retornaSelecionados(event: any) {
    this.EPISelecionado = [...event];
  }

  async savePopup(fonteGeradora: string, procedimentos: string, contramedidas: string) {
    if (this.validarForms(fonteGeradora, procedimentos, contramedidas)) {
        const riscoMapeado: RiscoMapeadoModelo = new RiscoMapeadoModelo(this.riscoSelecionado, this.severidadeSelecionada,
          this.frequenciaSelecionada, fonteGeradora, procedimentos, 0, contramedidas, this.EPISelecionado);
        if (this.idRiscoRecebido) {
          riscoMapeado.id = this.idRiscoRecebido;
        }
        this.dialogRef.close({ data: riscoMapeado });
    }
  }

  validarForms(fonteGeradora: string, procedimentos: string, contramedidas: string) {
    if (this.severidadeSelecionada === undefined) {
      this.abrirPopUp(0, 'Escolha uma severidade para cadastrar o risco!');
      return false;
    }
    if (this.data.tela === 0) {
      if (this.frequenciaSelecionada === undefined) {
        this.abrirPopUp(0, 'Escolha uma probabilidade para cadastrar o risco!');
        return false;
      }
    }
    if ((fonteGeradora === '') || (fonteGeradora === undefined) || (fonteGeradora.trim() === '')) {
      this.abrirPopUp(0, 'Insira pelo menos uma fonte geradora para cadastrar o risco!');
      return false;
    }

    if ((!this.EPISelecionado || this.EPISelecionado.length === 0) &&
        (procedimentos === '' || procedimentos === undefined || procedimentos.trim() === '') &&
        (contramedidas === '' || contramedidas === undefined || contramedidas.trim() === '')) {
          this.abrirPopUp(0, 'É necessário inserir ao menos uma contramedida, procedimento ou EPI para continuar!');
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
}
