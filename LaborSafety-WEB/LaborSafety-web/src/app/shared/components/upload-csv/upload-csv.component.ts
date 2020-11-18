import { Component,EventEmitter, OnInit, Input, Type, Output } from '@angular/core';
import { CSVExtractor } from '../../util/csv-extractor';

@Component({
  selector: 'upload-csv',
  templateUrl: './upload-csv.component.html'
})
export class UploadCsvComponent implements OnInit {


  @Output() eventoImportacao = new EventEmitter<any[]>();
  @Input() tipo: Type<any>;
  
  constructor() { }

  ngOnInit(): void {
  }

  async changeListener(files: FileList){
    const dadosImportados = await CSVExtractor.extractContent(files, this.tipo);
    this.eventoImportacao.emit(dadosImportados);
  }

}
