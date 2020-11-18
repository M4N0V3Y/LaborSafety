import { Component, OnInit, Input } from '@angular/core';
import { ngxCsv } from 'ngx-csv';

@Component({
  selector: 'download-csv',
  templateUrl: './download-csv.component.html'
})
export class DownloadCsvComponent implements OnInit {

  @Input() dados: any[];
  @Input() colunas?: string[];
  @Input() nomeArquivo: string;
  @Input() desabilita: boolean;

  constructor( ) { 
      
    }

  ngOnInit(): void {
  }

  exportar(): void {

    const options = { 
      fieldSeparator: ';',
      quoteStrings: '"',
      decimalseparator: ',',
      showLabels: this.colunas != null,
      useBom: true,
      noDownload: false,      
      headers: this.colunas ? this.colunas : []
    };
    // tslint:disable-next-line:no-unused-expression
    new ngxCsv(this.dados, this.nomeArquivo, options);
  }

}
