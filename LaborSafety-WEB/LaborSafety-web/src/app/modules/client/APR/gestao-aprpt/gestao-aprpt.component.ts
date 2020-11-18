import { Component, OnInit, ViewChild } from '@angular/core';
import { FiltrosAPR } from 'src/app/shared/models/FiltrosAPR';
import { AprServiceService } from 'src/app/core/http/apr/apr-service.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatPaginator, MatTableDataSource, MatDialogConfig, MatDialog } from '@angular/material';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Tela } from 'src/app/shared/models/tela.model';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';

@Component({
  selector: 'app-gestao-aprpt',
  templateUrl: './gestao-aprpt.component.html',
  styleUrls: ['./gestao-aprpt.component.scss']
})
export class GestaoAprptComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  filtrosAPR: FiltrosAPR[] = [];
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  numeroDeSerie: string;
  ordemDeManutencao: string;
  nodosSelecionados: any;
  resultadoPesquisa: any[];
  dataSource: MatTableDataSource<any>;
  Colunas = ['coluna1', 'coluna2'];
  desabilitarButtonCadastro: boolean;
  constructor(private service: AprServiceService, private router: Router, private auth: AuthenticationService, private dialog: MatDialog,
              private route: ActivatedRoute) {
    if (!this.verificaLogin()) {
      return;
    } else {
      this.service.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
      .then((lista: Tela[]) => {
        this.listaTelasEFuncionalidadesPorPerfil = [...lista];
        if (this.verificarPermissaoDeTela() !== true) {
          this.router.navigate(['/notfound']);
        } else {
          this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {
            if (tela.CodTela === 4) {
              tela.Funcionalidades.forEach((funcionalidade) => {
                this.verificaFuncionalidades(funcionalidade.CodFuncionalidade);
              });
            }
          });
        }
      }).catch(() => {
        this.router.navigate(['/auth/login']);
      });

    }

   }

  verificaLogin() {
    if (!this.auth.isLogin()) {
        this.router.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }
  ngOnInit() {

  }
  verificarPermissaoDeTela() {
    let permissao = false;
    if (this.listaTelasEFuncionalidadesPorPerfil) {
        this.listaTelasEFuncionalidadesPorPerfil.forEach(
        (tela) => {
          if (4 === tela.CodTela) {
            permissao = true;
          }
        });
    }
    return permissao;
  }
  verificaFuncionalidades(codFuncionalidade: number) {
    switch (codFuncionalidade) {
      case 1:
        this.desabilitarButtonCadastro = true;
        break;
      case 2:
        break;
      case 3:
        break;
      case 4:
        break;
      case 5:
        break;
    }
  }
  setCodLI(nodos) {
    const CodLI = [];
    nodos.forEach(li => {
      CodLI.push(li.CodLocalInstalacao);
    });
    return CodLI;
  }
  pesquisar() {
    const locais = this.setCodLI(this.nodosSelecionados);
    const model = new FiltrosAPR(this.numeroDeSerie, this.ordemDeManutencao, locais);
    if (this.verificaFiltros()) {
      this.service.listarAPR(model).then((data) => {
        this.dataSource = new MatTableDataSource(data);
        this.dataSource.paginator = this.paginator;

      }).catch(() => {
        this.dataSource = new MatTableDataSource();
        this.dataSource.paginator = this.paginator;
      });
    }

  }
  retornaSelecionados(selecionados: any) {
    this.nodosSelecionados = selecionados;
  }

  redirecionarCadastro() {
    this.router.navigate(['/client/aprpt/cadastro']);
  }

  redirecionarMapa() {
    this.router.navigate(['/client/mapa']);
  }

  editar(APR) {
    this.router.navigate(['/client/aprpt/cadastro'],
    {queryParams: APR});

  }
  gerarTodosLogs() {
    this.service.gerarTodosLogs().then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_APR_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }
  baixarModelo() {
    this.service.getModeloAPR().then((modelo) => {
      const linkSource = 'data:application/pdf;base64,' + modelo;
      const downloadLink = document.createElement('a');
      const fileName = 'ModeloAPR.pdf';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();
    });
  }
  verificaFiltros() {
    if ( (this.nodosSelecionados.length === 0 )  && ( this.ordemDeManutencao === undefined || this.ordemDeManutencao === '' )
    &&  ( this.numeroDeSerie === undefined || this.numeroDeSerie === '' ) ) {
      this.abrirPopUp(0, 'Insira pelo menos UM FILTRO para pesquisar APR');
      return false;
    }
    return true;
  }
  async abrirPopUp(tipo: number, mensagem: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    await dialogRef.afterClosed().subscribe(result => {
      return result;
    });
  }

}
