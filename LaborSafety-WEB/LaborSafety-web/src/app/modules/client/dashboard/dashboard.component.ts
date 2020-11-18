import { Component, OnInit, ViewChild } from '@angular/core';
import { HomepageService } from 'src/app/core/http/dashboard/dashboard.service';
import { Tela } from 'src/app/shared/models/tela.model';
import { GestaoAtividadeInventarioService } from '../inventario-atividade/gestao-inv-atividade/gestao-inv-atividade.service';
import { TreeViewComponent } from '../treeview/treeview.component';
import { Router } from '@angular/router';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { AprServiceService } from 'src/app/core/http/apr/apr-service.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;


  constructor(public homepageService: HomepageService, private aprService: AprServiceService,
    private dialog: MatDialog, private router: Router, private auth: AuthenticationService) {

  }


  homepageObject: Tela[];
  dataSource: any;
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  ngOnInit() {
    if (!this.verificaLogin()) {
      return;
    }
    this.homepageService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
      .then((lista) => {
        this.listaTelasEFuncionalidadesPorPerfil = [...lista];
        this.homepageObject = lista;
      }).catch(() => {
        this.router.navigate(['/auth/login']);
      });
    /*this.homepageService.getAll().then(
      (data: Tela[]) => { this.homepageObject = data; }
    );*/
  }
  verificaLogin() {
    if (!this.auth.isLogin()) {
      this.router.navigate(['/auth/login']);
      return false;
    } else {
      return true;
    }
  }
  verificarPermissaoDeTela(permissaoRequerida: number) {
    let permissao = false;

    if (this.homepageObject) {
      this.homepageObject.forEach(
        (tela) => {
          if (permissaoRequerida === tela.CodTela) { permissao = true; }
        });
    }
    return permissao;
  }

  verificarPermissaoDeFuncionalidade(permissaoDeTela: number, permissaoDeFuncionalidade: number) {
    let permissao = false;
    if (this.homepageObject) {
      const telaSelecionada = this.homepageObject.find(tela => tela.CodTela === permissaoDeTela);
      if (telaSelecionada) {
        telaSelecionada.Funcionalidades.forEach(
          (funcionalidade) => {
            if (permissaoDeFuncionalidade === funcionalidade.CodFuncionalidade) { permissao = true; }
          }
        );
      }
    }
    return permissao;
  }

  gerenciamentoDeBotoes(id) {

  }

  gerenciamentoPerfisBtn() {
    this.router.navigate(['/client/perfil']);
  }

  inventarioAmbienteBtn() {
    this.router.navigate(['/client/inventario-ambiente']);
  }

  inventarioAtividadeBtn() {
    this.router.navigate(['/client/inventario-atividade']);
  }

  APRPTBtn() {
    this.router.navigate(['/client/aprpt']);
  }

  importarExportarDadosBtn() {
    this.router.navigate(['/client/importacao']);
  }

  novaAPRPTBtn() {
    this.router.navigate(['/client/aprpt/cadastro']);
  }

  gerarAPRPTEmBrancoBtn() {
    this.aprService.getModeloAPR().then((modelo) => {
      const linkSource = 'data:application/pdf;base64,' + modelo;
      const downloadLink = document.createElement('a');
      const fileName = 'ModeloAPR.pdf';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();
    });
  }

  novoInventarioAmbienteBtn() {
    this.router.navigate(['/client/inventario-ambiente/cadastro']);
  }

  novoInventarioAtividadeBtn() {
    this.router.navigate(['/client/inventario-atividade/cadastro']);
  }
}
