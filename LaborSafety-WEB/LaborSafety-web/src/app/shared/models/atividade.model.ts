export class Atividade {
    CodAtividadePadrao: number;
    Nome: string;
    Descricao: string;
    constructor(cap: number, n: string, d: string) {
        this.CodAtividadePadrao = cap;
        this.Descricao = d;
        this.Nome = n;
    }
}
