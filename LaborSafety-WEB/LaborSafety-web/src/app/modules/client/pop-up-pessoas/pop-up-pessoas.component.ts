import { Component, OnInit, Inject, ViewChild, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatTableDataSource, MatPaginator, MatDialogConfig, MatDialog } from '@angular/material';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';
import { Router } from '@angular/router';
import { DisciplinaService } from 'src/app/core/http/disciplina/disciplina.service';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { AtividadeService } from 'src/app/core/http/atividade/atividade.service';
import { Atividade } from 'src/app/shared/models/atividade.model';
import { PessoaModel } from 'src/app/shared/models/PessoaModel';
import { AprServiceService } from 'src/app/core/http/apr/apr-service.service';
import { ValidarCpf } from 'src/app/shared/util/validatorCPF';
export interface DialogData {
  id: number;
  pessoaRecebida: PessoaModel;
}
@Component({
  selector: 'app-pop-up-pessoas',
  templateUrl: './pop-up-pessoas.component.html',
  styleUrls: ['./pop-up-pessoas.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PopUpPessoasComponent implements OnInit {
  pessoa = new PessoaModel('', '', '', '', '', '');
  isForm = false;
  isCadastro = true;
  filtro = 'Nome';
  @ViewChild(MatPaginator) paginator: MatPaginator;
  dataSource: MatTableDataSource<PessoaModel>;
  usuarios = [];
  usuarioService;
  usuariosPesquisado;
  displayedColumns: string[] = ['Matricula', 'CPF', 'Nome', 'Telefone', 'Email', 'Empresa', 'opcoes'];
  constructor(private router: Router, public dialogRef: MatDialogRef<PessoaModel>,  private dialog: MatDialog,
              private disciplinaService: DisciplinaService, private atividadeService: AtividadeService,
              private service: AprServiceService, @Inject(MAT_DIALOG_DATA) public data: DialogData, public validador: ValidarCpf) {
                this.dialogRef.updateSize('60%');
                this.service.getAllPessoas().then((pessoas: PessoaModel[]) => {
                  this.usuarios = pessoas;
                  this.dataSource = new MatTableDataSource(pessoas);
                  this.dataSource.paginator = this.paginator;
                  this.usuariosPesquisado =   this.dataSource;
                });
              }

  ngOnInit() {

    if (this.data.pessoaRecebida !== undefined ) {
      this.isCadastro = false;
      this.pessoa.CPF = this.data.pessoaRecebida.CPF;
      this.pessoa.Nome = this.data.pessoaRecebida.Nome;
      this.pessoa.Matricula = this.data.pessoaRecebida.Matricula;
      this.pessoa.Telefone = this.data.pessoaRecebida.Telefone;
      this.pessoa.Email = this.data.pessoaRecebida.Email;
      this.pessoa.Empresa = this.data.pessoaRecebida.Empresa;
    }

  }
  cadastrar() {
    this.dialogRef.updateSize('55%');
    this.isForm = true;
    this.isCadastro = true;
    // this.pessoa.Telefone = '+55';
  }
  inserir(pessoa: PessoaModel) {
    this.dialogRef.close({ data: pessoa });
  }
  filtrar(event) {

    const pesquisa = event.target.value;
    if (pesquisa === '' || pesquisa == null ) {
      this.dataSource = new MatTableDataSource(this.usuarios);
      this.dataSource.paginator = this.paginator;

    } else {
      this.dataSource = undefined;
      this.usuariosPesquisado = [];
      this.usuarios.forEach((user) => {

        switch (this.filtro) {
          case 'Nome':
            if (user.Nome.toUpperCase().includes(pesquisa.toUpperCase()) &&
             user.Nome[0].toUpperCase().includes(pesquisa[0].toUpperCase())) {
              this.usuariosPesquisado.push(user);
              this.dataSource = new MatTableDataSource(this.usuariosPesquisado);
              this.dataSource.paginator = this.paginator;
            }
            break;
          case 'Matricula':
            if (user.Matricula.toUpperCase().includes(pesquisa.toUpperCase()) &&
             user.Matricula[0].toUpperCase().includes(pesquisa[0].toUpperCase())) {
              this.usuariosPesquisado.push(user);
              this.dataSource = new MatTableDataSource(this.usuariosPesquisado);
              this.dataSource.paginator = this.paginator;
            }
            break;
          case 'CPF':
            if (user.CPF.toUpperCase().includes(pesquisa.toUpperCase()) &&
             user.Matricula[0].toUpperCase().includes(pesquisa[0].toUpperCase())) {
              this.usuariosPesquisado.push(user);
              this.dataSource = new MatTableDataSource(this.usuariosPesquisado);
              this.dataSource.paginator = this.paginator;
            }
            break;

        }


      });
    }

}
popUpDeletarPessoa(pessoa: PessoaModel){
  this.abrirPopUp(2, 'Deseja excluir essa pessoa?', pessoa);
}

  deletePessoa(pessoa: PessoaModel) {
    this.service.deletePessoa(pessoa.CodPessoa).then(() => {
      let posicaoRemovida = null;
      for (let i = 0; i < this.usuarios.length; i++) {
        if (this.usuarios[i].CodPessoa === pessoa.CodPessoa) {
          posicaoRemovida = i;
        }
      }
      this.usuarios.splice(posicaoRemovida, 1);
      this.dataSource = new MatTableDataSource(this.usuarios);
      this.dataSource.paginator = this.paginator;
    });
    this.dialogRef.close({ data: pessoa, tipo: 'exclusao' });
  }

  editarPessoa(pessoa: PessoaModel) {
    this.isForm = true;
    this.isCadastro = false;
    this.pessoa.CPF = pessoa.CPF;
    this.pessoa.Nome = pessoa.Nome;
    this.pessoa.Matricula = pessoa.Matricula;
    this.pessoa.Telefone = pessoa.Telefone;
    this.pessoa.Email = pessoa.Email;
    this.pessoa.Empresa = pessoa.Empresa;
    this.pessoa.CodPessoa = pessoa.CodPessoa;
  }

  validaCadastro(pessoa: PessoaModel) {
    if (pessoa.CPF === undefined || pessoa.CPF === '' || pessoa.CPF.trim() === '') {
      this.abrirPopUp(0, 'O campo CPF nao pode estar vazio');
      return false;
    } else if (!this.validador.cpf(pessoa.CPF)) {
      this.abrirPopUp(0, 'CPF invalido, tente novamente');
      return false;
    }
    if (pessoa.Nome === undefined || pessoa.Nome === '' || pessoa.Nome.trim() === '') {
      this.abrirPopUp(0, 'O campo nome nao pode estar vazio');
      return false;
    }
    return true;

  }

  salvarCadastro() {
    if (this.isCadastro === true) {
      if (this.validaCadastro(this.pessoa) ) {
        this.service.inserirPessoa(this.pessoa).then(() => {
          this.service.listarPessoaPorCPF(this.pessoa.CPF).then((pessoa: PessoaModel) => {
            this.pessoa.CodPessoa = pessoa.CodPessoa;
            this.dialogRef.close({ data: this.pessoa });
          });
        });
      }
    } else {
      if (this.validaCadastro(this.pessoa) ) {
        this.service.editarPessoa(this.pessoa).then(() => {
          this.service.listarPessoaPorCPF(this.pessoa.CPF).then((pessoa: PessoaModel) => {
            this.pessoa.CodPessoa = pessoa.CodPessoa;
            this.dialogRef.close({ data: this.pessoa, tipo: 'edicao' });
          });
        });
      }
    }
  }

  abrirPopUp(tipo: number, mensagem: string, pessoa?: PessoaModel) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.maxWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: tipo, textoRecebido: mensagem };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if(result === 'CONFIRMAR'){
        this.deletePessoa(pessoa);
      }
      return result;

    });
  }

}

