import { Component, OnInit, Inject } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  msgTelaPrincipal: string;
  nomeCabecalho: string;

  logoSrc = '';
  constructor(@Inject(APP_BASE_HREF) href: string) {
    this.logoSrc = `${href}assets/img/logo.svg`;
  }

  ngOnInit() {
    this.msgTelaPrincipal = 'Bem vindo ao Portal Manutenção Segura';
    this.nomeCabecalho = 'Ternium';
  }
}
