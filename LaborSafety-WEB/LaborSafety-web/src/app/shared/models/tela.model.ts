import { Funcionalidade } from './funcionalidades.model';

export class Tela {
    CodTela: number;
    Codigo: number;
    Nome: string;
    Descricao: string;
    Funcionalidades: Funcionalidade[];

    constructor(codTela: number, codigo: number, nome: string, descricao: string, lista: Funcionalidade[]) {
        this.CodTela = codTela;
        this.Codigo = codigo;
        this.Nome = nome;
        this.Descricao = descricao;
        this.Funcionalidades = lista;
    }
}
