import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import {MatDialogModule} from '@angular/material';
import { ErrosImportacao } from '../../models/ImportacaoModel';

export interface PopupData {
  tipoPopup: number;
  textoRecebido: string;
  erros?: ErrosImportacao[];
  locais?: string[];
}

@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.scss']
})
export class PopupComponent implements OnInit {
  iconData: string;
  textoData: string;
  mensagemButton: string;
  displayedColumnsLocais: string[] = ['nome'];
  displayedColumns: string[] = ['codigo', 'descricao', 'celula'];
  constructor(@Inject(MAT_DIALOG_DATA) public data: PopupData,
              public dialogRef: MatDialogRef<PopupComponent>) { }

  ngOnInit() {

    this.textoData = this.data.textoRecebido;
    switch (this.data.tipoPopup) {
      case 0:
        // ERROR
        this.iconData = 'error_outline';
        this.dialogRef.updateSize('400px');
        this.mensagemButton = 'OK';
        break;
      case 1:
        // SUCESSO
        this.iconData = 'check';
        this.dialogRef.updateSize('400px');
        this.mensagemButton = 'OK';
        break;
      case 2:
        // CONFIRMAÇAO
        this.iconData = 'warning';
        this.dialogRef.updateSize('400px');
        this.mensagemButton = 'CANCELAR';
        break;
      case 3:
        // INFORMAÇÃO
        this.iconData = 'report';
        this.dialogRef.updateSize('500px');
        this.mensagemButton = 'OK';
        break;
      case 4:
        // tabela de erros
        this.iconData = 'error_outline';
        this.dialogRef.updateSize('500px', '300px');
        this.mensagemButton = 'OK';
        break;
        case 5:
          // tabela de erros
          this.iconData = 'error_outline';
          this.dialogRef.updateSize('500px', '350px');
          this.mensagemButton = 'OK';
          break;
        case 6:
          // Load
          this.iconData = 'report';
          this.dialogRef.updateSize('500px');
          this.mensagemButton = 'OK';
          break;
      default:
        this.iconData = 'report';
        this.dialogRef.updateSize('400px');
        this.textoData = 'Erro não identificado';
    }
  }

}
