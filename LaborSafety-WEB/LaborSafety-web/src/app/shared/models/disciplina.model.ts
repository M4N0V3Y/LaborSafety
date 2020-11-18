export class Disciplina {
    CodDisciplina: number;
    Nome: string;
    Descricao: string;

    constructor(cd: number, n: string, d: string) {
        this.CodDisciplina = cd;
        this.Descricao = d;
        this.Nome = n;
    }
}
