import { Funcionalidade } from './funcionalidades.model';

export class TelaPorPerfil {
    CodTela: number;
    Nome: string;
    Funcionalidades: Funcionalidade[];

    constructor(codTela: number, nome: string, lista: Funcionalidade[]) {
        this.CodTela = codTela;
        this.Nome = nome;
        this.Funcionalidades = lista;
    }
}
