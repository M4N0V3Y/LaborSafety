import { Component, OnInit } from '@angular/core';
import { MapaService } from 'src/app/core/http/mapabloqueio/mapa-bloqueio.service';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';

@Component({
  selector: 'app-mapa-bloqueio',
  templateUrl: './mapa-bloqueio.component.html',
  styleUrls: ['./mapa-bloqueio.component.scss']
})
export class MapaBloqueioComponent implements OnInit {

  ordemColunas = ['Ordem', 'Status'];
  ordemTextArea = '';
  ordens: string[];
  ordensInvalidas = [];
  ordemDataSource: OrdemDataSource[] = [];

  constructor(public mapaService: MapaService, public router: Router, private auth: AuthenticationService) {
    if (!this.verificaLogin()) {
      return;
    }
  }

  ngOnInit() {

  }
  verificaLogin() {
    if (!this.auth.isLogin()) {
        this.router.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }

  redirecionarTelaGestao() {
    this.router.navigate(['/client/aprpt/']);
  }

  enviarOrdens() {
    if (this.ordemTextArea && this.ordemTextArea.length !== 0) {
      this.ordens = this.ordemTextArea.split('\n');
      const ordensValidadas = this.ordens.filter((ordem) => {
        if (ordem !== '') {
          return ordem;
        }
      });
      this.mapaService.sendGetOrdens(ordensValidadas).then((data) => {
        this.ordensInvalidas = data.OrdensInvalidas;
        this.setDataSource();
        const byteArray = new Uint8Array(atob(data._b64).split('').map(char => char.charCodeAt(0)));
        return new Blob([byteArray], {type: 'application/pdf'});
      }).then(blob => {
        if (blob.size !== 0) {
          const url = URL.createObjectURL(blob);
          window.open(url, '_blank');
        }


      });
    }
  }
  setDataSource() {
    this.ordemDataSource = [];
    this.ordens.forEach(ordem => {
      const auxDataSource = new OrdemDataSource();
      auxDataSource.Nome = ordem;
      if (this.ordensInvalidas.includes(ordem)) {
        auxDataSource.Status = 'clear';
      } else {
        auxDataSource.Status = 'done';
      }
      this.ordemDataSource.push(auxDataSource);
    });
  }

}
class OrdemDataSource {
  Nome: string;
  Status: string;
}
