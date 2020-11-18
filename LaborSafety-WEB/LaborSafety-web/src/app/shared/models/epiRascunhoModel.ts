export class EpiRascunhoModelo {
    CodEpiRiscoRascunhoInventarioAmbiente: number;
    CodEPI: number;
    CodRiscoRascunhoInventarioAmbiente: number;
    Ativo: true;
    constructor(CodEPI: number, CodRisco: number,  CodEPIRisco: number ) {
      this.CodEPI = CodEPI;
      this.CodRiscoRascunhoInventarioAmbiente = CodRisco;
      this.CodEpiRiscoRascunhoInventarioAmbiente = CodEPIRisco;
    }
  }
export class EpiRascunhoAtividadeModelo {
    CodRiscoRascunhoInventarioAtividade: number;
    CodEPI: number;
    CodEpiRiscoRascunhoInventarioAtividade: number;
    Ativo: true;
    constructor(CodEPI: number, CodRisco: number,  CodEPIRisco: number ) {
      this.CodEPI = CodEPI;
      this.CodRiscoRascunhoInventarioAtividade = CodRisco;
      this.CodEpiRiscoRascunhoInventarioAtividade = CodEPIRisco;
    }
  }
