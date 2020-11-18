export class PerfilDeCatalogo {
    CodPerfilCatalogo: number;
    Codigo: string;
    Nome: string;
    Idioma: string;
    Mdt: number;
    constructor(cpc: number, c: string, n: string, i: string, m: number) {
        this.CodPerfilCatalogo = cpc;
        this.Codigo = c;
        this.Nome = n;
        this.Idioma = i;
        this.Mdt = m;
    }
}
