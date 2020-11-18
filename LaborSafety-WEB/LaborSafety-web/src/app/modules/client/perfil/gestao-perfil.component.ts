import { Component, OnInit, ChangeDetectionStrategy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Perfil } from 'src/app/shared/models/perfil.model';
import { PerfilService } from 'src/app/core/http/perfil/perfil.service';
import { TelaService } from 'src/app/core/http/tela/tela.service';
import { Tela } from 'src/app/shared/models/tela.model';
import { Funcionalidade } from 'src/app/shared/models/funcionalidades.model';
import { FuncionalidadeService } from 'src/app/core/http/funcionalidade/funcionalidade.service';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { formatDate } from '@angular/common';

export class CheckboxDeFuncionalidade {
  funcionalidade: Funcionalidade;
  selected: boolean;

  constructor(f: Funcionalidade, s: boolean) {
    this.funcionalidade = f;
    this.selected = s;
  }
}

export class LogPerfil {
  Usuario: string;
  Perfil: string;
  Tela: Tela;
  Data: any;
  FuncAnt: Funcionalidade[];
  FuncNova: Funcionalidade[];

  constructor(u: string, p: string, t: Tela, d: Date, fant: Funcionalidade[], fnov: Funcionalidade[]) {
    this.Usuario = u;
    this.Perfil = p;
    this.Tela = t;
    this.Data = d;
    this.FuncAnt = fant;
    this.FuncNova = fnov;
  }
}

@Component({
  selector: 'app-gestao-perfil',
  templateUrl: './gestao-perfil.component.html',
  styleUrls: ['./gestao-perfil.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})


export class GestaoPerfilComponent implements OnInit {
 listaTelasEFuncionalidadesPorPerfil: Tela[];
  constructor(public perfilService: PerfilService, public telaService: TelaService, private changeDetector: ChangeDetectorRef,
              public funcionalidadeService: FuncionalidadeService, private router: Router,
              private authenticationService: AuthenticationService) {
                if (!this.verificaLogin()) {
                  return;
                }
                this.perfilService.getListaTelasEFuncionalidadesPorPerfil(window.sessionStorage.getItem('perfil'))
                .then((lista: Tela[]) => {
                  this.listaTelasEFuncionalidadesPorPerfil = [...lista];
                  if (this.verificarPermissaoDeTela() !== true) {
                    this.router.navigate(['/notfound']);
                  }
                }).catch(() => {
                  this.router.navigate(['/auth/login']);
                });
               }

  usuario: any;
  telas: Tela[] = [];
  telaSelecionada: Tela;
  telasParaPainelDireito: Tela[] = [];
  perfis: Perfil[] = [];
  perfilSelecionado: Perfil;
  funcionalidadesAnterioresLog: Funcionalidade[] = [];
  funcionalidadesResultado: Funcionalidade[] = [];
  todasAsFuncionalidades: Funcionalidade[];
  telaSelectionDisabled = true;
  pannelDisabled = true;
  checkBoxes: CheckboxDeFuncionalidade[] = [];
  falseBoolean = false;
  checada = false;
  panelsEnabled = true;
  panelsExpanded = false;
  funcTooltip: string;
  user: any;
  perfilLogDataSource: LogPerfil[] = [];
  vetorlixo: LogPerfil[] = [];
  logColunas: string[] = ['Usuario',
                          'Perfil alterado',
                          'Tela',
                          'Data de alteracao',
                          'Funcionalidades anteriores',
                          'Funcionalidades novas'
                        ];

  ngOnInit() {
    this.user = this.authenticationService.getUser();
    this.usuario = this.authenticationService.getUser();

    this.perfilService.getPerfis().then(
      (data: Perfil[]) => {
        this.perfis = data;
      //  this.perfis = this.perfis.filter(p => p.CodPerfil !== 1); // Remoção de adm
       }
    );

    this.telaService.getAll().then(
      (data: Tela[]) => {
        this.telas = data;
      //  this.telas = this.telas.filter(t => t.CodTela !== 1); // Remoção de ger_perf
      }
    );

    this.funcionalidadeService.getAll().then(
      (data: Funcionalidade[]) => {
        this.todasAsFuncionalidades = data;
        this.funcTooltip = 'Referência de funcionalidades:\n';
        data.forEach((d) => { this.funcTooltip += d.CodFuncionalidade + ': ' + d.Descricao + '\n'; });
      }
    );
  }
  verificaLogin() {
    if (!this.authenticationService.isLogin()) {
        this.router.navigate(['/auth/login']);
        return false;
   } else {
     return true;
   }
  }
  refreshPanels() {
    this.panelsEnabled = false;
    this.changeDetector.detectChanges();
    this.panelsEnabled = true;
  }

  verificarPermissaoDeTela() {
    let permissao = false;
    if (this.listaTelasEFuncionalidadesPorPerfil) {
        this.listaTelasEFuncionalidadesPorPerfil.forEach(
        (tela) => {
          if (1 === tela.CodTela) {
            permissao = true;
          }
        });
    }
    return permissao;
  }

  selecionarPerfil(event: any) {
    this.checkBoxes.length = 0;
    this.refreshPanels();
    if (this.perfis) {
      this.perfis.forEach(
        (perfil) => {
          if (perfil.Nome === event.value) {
            this.perfilSelecionado = perfil;
            this.prepararPainel();
            this.telaSelectionDisabled = false;
            this.perfilService.getListarListaTelasEFuncionalidadesPorPerfil(perfil.CodPerfil).then(
              (data: Tela[]) => { this.telasParaPainelDireito = data; }
            );
            return this.perfilSelecionado;
          }
        }
      );
    }
  }

  selecionarTela(event: any) {
    this.checkBoxes.length = 0;
    this.refreshPanels();
    if (this.telas) {
      this.telas.forEach(
        (tela) => {
          if (tela.Nome === event.value) {
            this.telaSelecionada = tela;
            this.prepararPainel();
            this.pannelDisabled = false;
            return this.telaSelecionada;
          }
        }
      );
    }
  }

  prepararPainel() {
    if (this.perfilSelecionado && this.telaSelecionada) {
      this.perfilService.getListarTelaEFuncionalidadesPorPerfilETela(this.perfilSelecionado.CodPerfil, this.telaSelecionada.CodTela).then(
        (tela) => {
          this.perfilSelecionado.TelaSelecionada = tela;
          if (this.perfilSelecionado.TelaSelecionada) {
            this.funcionalidadesAnterioresLog = this.perfilSelecionado.TelaSelecionada.Funcionalidades;
            this.funcionalidadesResultado = this.perfilSelecionado.TelaSelecionada.Funcionalidades;
            this.preencherCheckBoxes();
          }
        }
      );
    }
  }

  preencherCheckBoxes() {
    if (this.perfilSelecionado && this.telaSelecionada) {
      this.telaSelecionada.Funcionalidades.forEach(
        (funcionalidade) => {
          const checkboxElement = new CheckboxDeFuncionalidade(funcionalidade,
            this.getTelasDoPerfil(funcionalidade.CodFuncionalidade));
          this.checkBoxes.push(checkboxElement);
        }
      );
    }
  }



  onChange(listaDeFuncionalidadesDaTela: any) {
    const listaResultante: Funcionalidade[] = [];
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < listaDeFuncionalidadesDaTela.selectedOptions.selected.length; i++) {
      const func = this.todasAsFuncionalidades.find(
        element => listaDeFuncionalidadesDaTela.selectedOptions.selected[i].value === element.CodFuncionalidade
      );
      if (func) {
        listaResultante.push(func);
      }
    }
    listaResultante.sort((a, b) => {
      if (a.CodFuncionalidade > b.CodFuncionalidade) { return 1; }
      if (a.CodFuncionalidade < b.CodFuncionalidade) { return -1; }
    });
    this.funcionalidadesResultado = [...listaResultante];
  }

  getTelasDoPerfil(codigoFuncionalidade: number) {
    if (this.perfilSelecionado && this.perfilSelecionado.TelaSelecionada && this.perfilSelecionado.TelaSelecionada.Funcionalidades) {
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < this.perfilSelecionado.TelaSelecionada.Funcionalidades.length; i++) {
        if (this.perfilSelecionado.TelaSelecionada.Funcionalidades[i].CodFuncionalidade === codigoFuncionalidade) {
          return true;
        }
      }
      return false;
    }
    return false;
  }


  verificarEstado(codTela: number, codFunc: number) {
    this.checada = false;
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.telas.length; i++) {
      if (this.telas[i].CodTela === codTela) {
        // tslint:disable-next-line: prefer-for-of
        for (let j = 0; j < this.telas[i].Funcionalidades.length; j++) {
          if (this.telas[i].Funcionalidades[j].CodFuncionalidade === codFunc) {
            // tslint:disable-next-line: prefer-for-of
            for (let k = 0; k < this.telasParaPainelDireito.length; k++) {
              if (this.telasParaPainelDireito[k].CodTela === codTela) {
                // tslint:disable-next-line: prefer-for-of
                for (let l = 0; l < this.telasParaPainelDireito[k].Funcionalidades.length; l++) {
                  if (this.telasParaPainelDireito[k].Funcionalidades[l].CodFuncionalidade === codFunc) {
                    this.checada = true;
                  }
                }
              }
            }
            return true;
          }
        }
      }
    }
    return false;
  }

  criarLog(funcsAnteriores: Funcionalidade[], funcsAtuais: Funcionalidade[], perfilAlterado: Perfil, telaAlterada: Tela) {
    const newLogEntry: LogPerfil = {
      Usuario: this.usuario,
      Perfil: perfilAlterado.GrupoAD,
      Tela: telaAlterada,
      Data: formatDate(new Date(), 'dd/MM/yyyy, HH:mm', 'en'),
      FuncAnt: funcsAnteriores,
      FuncNova: funcsAtuais
    };
    this.vetorlixo.push(newLogEntry);
    this.vetorlixo.sort((a, b) => {
      return (new Date(b.Data) as any) - (new Date(a.Data) as any);
    });
    this.changeDetector.detectChanges();
    this.perfilLogDataSource = [...this.vetorlixo];
    this.changeDetector.detectChanges();

  }

  inserir() {
    const perfilResponse = {
      CodPerfil: this.perfilSelecionado.CodPerfil,
      CodTela: this.telaSelecionada.CodTela,
      Funcionalidades: this.funcionalidadesResultado,
      eightIDUsuarioModificador: this.user,
    };

    this.perfilService.atualizarPerfil(perfilResponse).then(
      (data: boolean) => { if (data) {
        this.perfilService.getListarListaTelasEFuncionalidadesPorPerfil(this.perfilSelecionado.CodPerfil).then(
          (dataTela: Tela[]) => {
            this.telasParaPainelDireito = dataTela;
            this.criarLog(this.funcionalidadesAnterioresLog, this.funcionalidadesResultado, this.perfilSelecionado, this.telaSelecionada);
            this.funcionalidadesAnterioresLog = this.funcionalidadesResultado;
          }
        );
       }
    });
  }

}
