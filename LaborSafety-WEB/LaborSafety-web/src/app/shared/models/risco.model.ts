export class Risco {
    CodRisco: number;
    CodTipoRisco: number;
    Nome: string;

    constructor(CodRisco: number, Codigo: string, CodTipoRisco: number,
                Nome: string) {
      this.CodRisco = CodRisco;
      this.CodTipoRisco = CodTipoRisco;
      this.Nome = Nome;
    }
  }
