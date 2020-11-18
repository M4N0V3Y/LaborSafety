export class PesoFisico {
    CodPeso: number;
    Indice: number;
    Nome: string;

    constructor(cp: number, n: string, i: number) {
        this.CodPeso = cp;
        this.Indice = i;
        this.Nome = n;
    }
}
