import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-impressao-massa',
  templateUrl: './impressao-massa.component.html',
  styleUrls: ['./impressao-massa.component.scss']
})
export class ImpressaoMassaComponent implements OnInit {


  ordemTextArea: string = '';

  constructor() { }

  ngOnInit() {
  }

}
