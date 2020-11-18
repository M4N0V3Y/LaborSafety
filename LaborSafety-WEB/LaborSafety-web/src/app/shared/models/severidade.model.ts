export class Severidade {
    CodSeveridade: number;
    Indice: number;
    Nome: string;

    constructor(cd: number, i: number, n: string) {
        this.CodSeveridade = cd;
        this.Indice = i;
        this.Nome = n;
    }
}
