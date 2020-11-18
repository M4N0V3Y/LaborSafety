export class RiscoInventarioAtividade {
    id?: string;
    riscoGeral?: number;
    CodRiscoInventarioAtividade: number;
    CodRisco: number;
    CodInventarioAtividade: number;
    CodSeveridade: number;
    FonteGeradora: string;
    ProcedimentoAplicavel: string;
    ContraMedidas: string;
    ativo: boolean;
    EPIRiscoInventarioAtividadeModelo: EpiModeloAtividade[] = [];


    constructor(CodRiscoInventarioAtividade: number, codRisco: number, codInventarioAtividade: number,
                codSeveridade: number, fonteGeradora: string,
                procedimentoAplicavel: string, ContraMedida: string, EpiRiscoModelo: EpiModeloAtividade[]) {
        this.CodInventarioAtividade = codInventarioAtividade;
        this.CodRisco = codRisco;
        this.CodRiscoInventarioAtividade = CodRiscoInventarioAtividade;
        this.CodSeveridade = codSeveridade;
        this.FonteGeradora = fonteGeradora;
        this.ProcedimentoAplicavel = procedimentoAplicavel;
        this.EPIRiscoInventarioAtividadeModelo = EpiRiscoModelo;
        this.ContraMedidas = ContraMedida;
    }
}
/*export class EpiModelo {
    CodEpiRiscoInventarioAtividade: number;
    CodEPI: number;
    CodRisco: number;
    constructor(CodEPI: number, CodRisco: number,  CodEpiRiscoInventarioAtividade: number ) {
        this.CodEPI = CodEPI;
        this.CodRisco = CodRisco;
        this.CodEpiRiscoInventarioAtividade = CodEpiRiscoInventarioAtividade;
    }

}*/
export class EpiModeloAtividade {
    CodEpiRiscoInventarioAtividade: number;
    CodEPI: number;
    CodRisco: number;
    constructor(CodEPI: number, CodRisco: number,  CodEPIRisco: number ) {
        this.CodEPI = CodEPI;
        this.CodRisco = CodRisco;
        this.CodEpiRiscoInventarioAtividade = CodEPIRisco;
    }
}
