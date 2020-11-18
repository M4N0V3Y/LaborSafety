export class PessoaModel {
    CodPessoa?: number;
    Matricula: string;
    Nome: string;
    CPF: string;
    Telefone: string;
    Email: string;
    Empresa: string;
    constructor(matricula: string, nome: string, CPF: string,  telefone: string, email: string,
                empresa: string) {
                    this.CPF = CPF;
                    this.Email = email;
                    this.Empresa = empresa;
                    this.Matricula = matricula;
                    this.Nome = nome;
                    this.Telefone = telefone;
    }
}
