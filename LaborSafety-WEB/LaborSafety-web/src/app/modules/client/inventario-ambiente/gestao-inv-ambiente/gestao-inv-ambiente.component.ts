import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { TreeViewComponent } from '@syncfusion/ej2-ng-navigations';
import { InvAmbienteService } from 'src/app/core/http/inventarioAmbiente/inv-ambiente.service';
import { filtrosModel } from 'src/app/shared/models/filtrosModel';
import { Router } from '@angular/router';
import { Tela } from 'src/app/shared/models/tela.model';
import { MatTableDataSource, MatPaginator, MatDialogConfig, MatDialog } from '@angular/material';
import { AmbienteModel } from 'src/app/shared/models/invAmbinteModel';
import { Ambiente } from 'src/app/shared/models/ambiente';
import { trigger, transition, style, animate } from '@angular/animations';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';


@Component({
  selector: 'app-gestao-inv-ambiente',
  templateUrl: './gestao-inv-ambiente.component.html',
  styleUrls: ['./gestao-inv-ambiente.component.scss'],
  animations: [
    trigger(
      'enterAnimation', [
      transition(':enter', [
        style({ transform: 'scaleY(0)', 'transform-origin': 'top left', opacity: 0 }),
        animate('0.25s', style({ transform: 'scaleY(1)', opacity: 1 })),
      ]),
      transition(':leave', [
        style({ transform: 'scaleY(1)', 'transform-origin': 'top left', opacity: 1 }),
        animate('0.25s', style({ transform: 'scaleY(0)', opacity: 0 }))
      ])
    ]
    )
  ]
})
export class GestaoInvAmbienteComponent implements OnInit {
  @ViewChild(TreeViewComponent) treeView: TreeViewComponent;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  listaTelasEFuncionalidadesPorPerfil: Tela[];
  pesquisaRetorno: boolean;
  allSo: any[];
  service: InvAmbienteService;
  resultadoPesquisa: any;
  semInventario: boolean;
  dataSource: any;
  filtro: filtrosModel;
  desabilitarButtonCadastro: boolean;
  desabilitarButtonEdicao: boolean;
  public nodosSelecionados: any;
  isRascunho: boolean;
  testesColunas: string[] = ['coluna1', 'coluna2'];
  public inventarioDataSource: MatTableDataSource<AmbienteModel>;

  ambientes: Ambiente[] = [];
  ambienteSelecionado: Ambiente;
  ambienteCard = false;
  pesquisaAmbiente: string;
  ambienteDataSource: Ambiente[] = [];
  nomeAmbienteCadastro: string;
  descricaoAmbienteCadastro: string;
  codigoAmbienteEdicao: number;
  nomeAmbienteEdicao: string;
  descricaoAmbienteEdicao: string;
  edicaoAmbiente = false;
  existeAmbiente = false;
  ambienteColunas: string[] = ['Nome', 'Descricao', 'Controles'];

  reloadCard = false;


  constructor(private router: Router, public invservice: InvAmbienteService,
              public changeDetector: ChangeDetectorRef, public dialog: MatDialog, private auth: AuthenticationService) {
                if (!this.verificaLogin()) {
                  return;
                }
                this.inventarioDataSource = new MatTableDataSource<AmbienteModel>();
                this.service = invservice;
                this.service.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
                  .then((lista: Tela[]) => {
                    this.listaTelasEFuncionalidadesPorPerfil = [...lista];
                    if (this.verificarPermissaoDeTela() !== true) {
                      this.router.navigate(['/notfound']);
                    } else {
                      this.listaTelasEFuncionalidadesPorPerfil.forEach((tela) => {
                        if (tela.CodTela === 2) {
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

  async ngOnInit() {
    await this.service.getAllSOs().then((data: any) => {
      this.allSo = data;
      if (this.allSo.length !== 0) {
        this.existeAmbiente = true;
      }
    });
    this.filtro = new filtrosModel(0, 0, 0, 0);

    this.invservice.getAllSOs().then((ambientes) => {
      this.ambientes = ambientes;
      this.ambienteDataSource = ambientes;
    });
  }
  verificaLogin() {
    if (!this.auth.isLogin()) {
        this.router.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }

  selectFilters(event: any, type: string) {
    switch (type) {
      case 'ambiente':
        this.ambienteSelecionado = event.value;
        break;
    }
  }

  verificarPermissaoDeTela() {
    let permissao = false;
    if (this.listaTelasEFuncionalidadesPorPerfil) {
      this.listaTelasEFuncionalidadesPorPerfil.forEach(
        (tela) => {
          if (2 === tela.CodTela) {
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
        this.desabilitarButtonEdicao = true;
        break;
      case 4:
        break;
      case 5:
        break;
    }
  }

  gerarTodosLogs() {
    this.service.gerarTodosLogs().then((data) => {
      const linkSource = 'data:text/plain;base64,' + data.resultadoString;
      const downloadLink = document.createElement('a');
      const date = new Date();
      const fileName = 'log_inventario_ambiente_'
      + new Date().toLocaleDateString() + '.txt';
      downloadLink.href = linkSource;
      downloadLink.download = fileName;
      downloadLink.click();

    });
  }
  cadastrarAmbiente() {
    if (this.nomeAmbienteCadastro && this.descricaoAmbienteCadastro
      && this.nomeAmbienteCadastro.trim() !== '' && this.descricaoAmbienteCadastro.trim() !== '') {
      const ambienteACadastrar: Ambiente = {
        CodAmbiente: null,
        Nome: this.nomeAmbienteCadastro,
        Descricao: this.descricaoAmbienteCadastro
      };
      this.invservice.insertSO(ambienteACadastrar).then((data: any) => {
        if (data.StatusCode === 200) {
          this.invservice.getAllSOs().then((ambientes) => {
            this.existeAmbiente = true;
            this.ambientes = ambientes;
            this.ambienteDataSource = ambientes;
            this.nomeAmbienteCadastro = '';
            this.descricaoAmbienteCadastro = '';
          });
        }
      });
    } else {
      this.erroNosCampos();
    }
  }



  editarAmbienteView(row: any, i: number) {
    this.codigoAmbienteEdicao = row.CodAmbiente;
    this.nomeAmbienteEdicao = row.Nome;
    this.descricaoAmbienteEdicao = row.Descricao;
    this.edicaoAmbiente = true;
    this.reloadCard = true;
    this.changeDetector.detectChanges();
    this.reloadCard = false;
    this.changeDetector.detectChanges();
    const element = document.getElementById('target');
    if (element) {
      element.scrollIntoView({ behavior: 'auto', block: 'start' });
    }
  }

  editarAmbiente() {
    if (this.codigoAmbienteEdicao && this.nomeAmbienteEdicao && this.descricaoAmbienteEdicao
      && this.nomeAmbienteEdicao.trim() !== '' && this.descricaoAmbienteEdicao.trim() !== '') {
      const ambienteModel: Ambiente = {
        CodAmbiente: this.codigoAmbienteEdicao,
        Nome: this.nomeAmbienteEdicao,
        Descricao: this.descricaoAmbienteEdicao
      };
      this.invservice.editSO(ambienteModel).then((data: any) => {
        if (data) {
          if (data.StatusCode === 200) {
            this.invservice.getAllSOs().then((ambientes) => {
              this.ambientes = ambientes;
              this.ambienteDataSource = ambientes;
            });
          }
        }
      });
    } else {
      this.erroNosCampos();
    }
  }

  erroNosCampos() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 0, textoRecebido: 'É necessário preencher os campos nome e descrição para concluir o cadastro!' };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
  }

  deletarAmbiente(row: any) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 2, textoRecebido: 'Deseja apagar o ambiente selecionado?' };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result === 'CONFIRMAR') {
        this.invservice.deleteSO(row.CodAmbiente).then((data: any) => {
          if (data) {
            if (data.StatusCode === 200) {
              this.invservice.getAllSOs().then((ambientes) => {
                if (ambientes.length === 0) {
                  this.existeAmbiente = false;
                }
                this.nomeAmbienteCadastro = '';
                this.nomeAmbienteEdicao = '';
                this.descricaoAmbienteCadastro = '';
                this.descricaoAmbienteEdicao = '';
                this.edicaoAmbiente = false;
                this.ambientes = ambientes;
                this.ambienteDataSource = ambientes;
              });
            }
          }
        });
      }

    });
  }

  filtrarAmbiente(event: any) {
    if (!this.pesquisaAmbiente || this.pesquisaAmbiente === '') {
      this.ambienteDataSource = this.ambientes;
    } else {
      this.ambienteDataSource = this.ambientes.filter(a => {
        if (a.Nome.includes(this.pesquisaAmbiente) || a.Descricao.includes(this.pesquisaAmbiente)) {
          return a;
        }
      });
    }
  }

  retornaSelecionados(selecionados: any) {
    this.nodosSelecionados = selecionados;
  }

  limparPesquisa() {
    this.inventarioDataSource.data = [];
    this.inventarioDataSource.paginator = this.paginator;
  }

  pesquisar() {
    if (this.ambienteSelecionado !== undefined) {
      this.filtro.CodAmbiente = this.ambienteSelecionado.CodAmbiente;
    } else {
      this.filtro.CodAmbiente = 0;
    }
    if (!this.isRascunho) {
      this.filtro.setLocaisInstalacao(this.nodosSelecionados);
      this.service.filtrar(this.filtro).then((data) => {
        this.limparPesquisa();
        this.inventarioDataSource.data = [...data];
        this.inventarioDataSource.paginator = this.paginator;
      }).catch(() => {
        this.limparPesquisa();
      });
    } else {
      this.filtro.setLocaisInstalacao(this.nodosSelecionados);
      this.service.pesquisarInventarioRascunho(this.filtro).then((data) => {
        this.inventarioDataSource.data = [...data];
        this.inventarioDataSource.paginator = this.paginator;
      }).catch(() => {
        this.limparPesquisa();
      });
    }

  }

  redirecionarTelaCadastro(): void {
    this.router.navigate(['/client/inventario-ambiente/cadastro']);
  }

  editar(inventario: AmbienteModel) {
    let model: { CodRascunhoInventarioAmbiente?: number; isRascunho?: boolean; CodInventarioAmbiente?: number; };
    if (this.isRascunho) {
       model = {
        CodRascunhoInventarioAmbiente: inventario.CodRascunhoInventarioAmbiente,
        isRascunho: true,
      };
       inventario.isRascunho = true;
    } else {
      model = {
      CodInventarioAmbiente: inventario.CodInventarioAmbiente,
      };
    }
    this.router.navigate(['/client/inventario-ambiente/cadastro'], {
      queryParams: model
    });
  }

}
