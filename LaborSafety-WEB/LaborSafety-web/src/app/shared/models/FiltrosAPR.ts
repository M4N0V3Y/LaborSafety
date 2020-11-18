export class FiltrosAPR {
  CodLocalInstalacao: any[];
  NumeroSerie: string;
  OrdemManutencao: string;
    constructor(NumeroSerie: string, OrdemManutencao: string, CodLI: any[]) {
      this.NumeroSerie = NumeroSerie;
      this.OrdemManutencao = OrdemManutencao;
      this.CodLocalInstalacao = [...CodLI];
    }
  }
