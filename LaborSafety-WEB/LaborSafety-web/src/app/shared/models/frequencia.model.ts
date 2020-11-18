export class Frequencia {
    CodProbabilidade: number;
    Peso: number;
    Nome: string;

    constructor(cp: number, p: number, n: string) {
        this.CodProbabilidade = cp;
        this.Peso = p;
        this.Nome = n;
    }
}
