import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-modal-alerta',
  templateUrl: './modal-alerta.component.html',
  styleUrls: ['./modal-alerta.component.scss']
})
export class ModalAlertaComponent implements OnInit {

  mensagem: string;
  titulo: string;

  constructor(public matDialogRef: MatDialogRef<ModalAlertaComponent>,
    @Inject(MAT_DIALOG_DATA) private parametros: any) {
    this.mensagem = parametros.mensagem;
    this.titulo = parametros.titulo;
  }

  ngOnInit(): void {
  }

}
