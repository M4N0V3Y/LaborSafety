import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { EPIService } from 'src/app/core/http/epi/epi.service';
import { EPI } from 'src/app/shared/models/EPI.model';
import { FormControl } from '@angular/forms';
import { prepareSyntheticPropertyName } from '@angular/compiler/src/render3/util';



@Component({
  selector: 'app-lista-epi',
  templateUrl: './lista-epi.component.html',
  styleUrls: ['./lista-epi.component.scss']
})
export class ListaEpiComponent implements OnInit {
  @Input() episRecebidas: any;
  @Output() episEnviadas = new EventEmitter();

  listaEPIs: EPI[] = [];
  epiSelecionados: EPI[] = [];
  availableEPIs: EPI[] = [];
  epiControl = new FormControl();
  showMode = false;
  pesquisa: string;

  constructor(public epiService: EPIService) { }

  async ngOnInit() {
    await this.epiService.ListarTodosEPIs().then((lista) => {
      this.listaEPIs = lista.filter((e) => e.N3 !== null && e.N3 !== '');
      this.availableEPIs = this.listaEPIs;
      this.showMode = true;
    });
    if (this.episRecebidas) {
      this.preencherEdicao(this.episRecebidas);
    }
    this.sendEPIs();
  }

  preencherEdicao(episParaMarcar: string[]) {
    episParaMarcar.forEach((epi) => {
      this.epiSelecionados.push(this.listaEPIs.find(e => e.Nome === epi));
    });
    this.epiControl.setValue(this.epiSelecionados);
  }

  sendEPIs() {
    this.episEnviadas.emit(this.epiSelecionados);
  }

  async markCheckbox(event: any) {
    if (event.isUserInput) {
      if (event.source.selected) {
        if (!this.epiSelecionados.includes(event.source.value)) {
          this.epiSelecionados.push(Object.assign(event.source.value));
          this.epiControl.setValue(this.epiSelecionados);
        }
      } else {
        if (this.epiSelecionados.includes(event.source.value)) {
          for (let i = 0; i < this.epiSelecionados.length; i++) {
            if (this.epiSelecionados[i] === event.source.value) {
               this.epiSelecionados.splice(i, 1); i--;
               this.epiControl.setValue(this.epiSelecionados);
            }
          }
        }
      }
      this.sendEPIs();
    }
  }

  epiOnKey(value: any) {
    this.pesquisa = value;
    if (!this.pesquisa || this.pesquisa === '') {
      this.availableEPIs = this.listaEPIs;
    } else {
      this.availableEPIs = this.listaEPIs.filter((epi) => (epi.Nome.toLowerCase()).includes(this.pesquisa.toLowerCase()));
    }
  }

  zerarPesquisa(event: any) {
    this.pesquisa = '';
    this.availableEPIs = this.listaEPIs;
  }

}
