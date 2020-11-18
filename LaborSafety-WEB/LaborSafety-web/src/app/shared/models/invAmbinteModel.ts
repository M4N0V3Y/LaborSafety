import {RiscoMapeadoModelo} from './riscomapeado.model';

export class NRModel {
    CodNRInventarioAmbiente: number;
    CodNR: number;
    CodInventarioAmbiente: number;
    constructor( CodNR: number) {
        this.CodNR = CodNR;
    }
}
export class RiscoAmbiente {
        id?: string;
        CodRiscoInventarioAmbiente: number;
        CodInventarioAmbiente: number;
        CodRiscoAmbiente: number;
        CodSeveridade: number;
        CodProbabilidade: number;
        FonteGeradora: string;
        ProcedimentosAplicaveis: string;
        ContraMedidas: string;
        EPIRiscoInventarioAmbienteModelo: EpiModeloAmbiente[] = [];

        constructor( CodRiscoInventarioAmbiente: number, CodInventarioAmbiente: number, CodRiscoAmbiente: number,
                     CodSeveridade: number, CodProbabilidade: number, FonteGeradora: string, ProcedimentosAplicaveis: string,
                     ContraMedidas: string, EpiRiscoModelo: EpiModeloAmbiente[]) {

                this.CodRiscoInventarioAmbiente = CodRiscoInventarioAmbiente;
                this.CodInventarioAmbiente = CodInventarioAmbiente;
                this.CodRiscoAmbiente = CodRiscoAmbiente;
                this.CodSeveridade = CodSeveridade;
                this.CodProbabilidade = CodProbabilidade;
                this.FonteGeradora = FonteGeradora;
                this.ProcedimentosAplicaveis = ProcedimentosAplicaveis;
                this.EPIRiscoInventarioAmbienteModelo = EpiRiscoModelo;
                this.ContraMedidas = ContraMedidas;
        }
        setEpiRiscoModelo() {

        }
}

export class LocalInstalacaoModel {
    CodLocalInstalacao?: number;
    CodInventarioAmbiente?: number;
    CodPeso?: number;
    CodPerfilCatalogo?: number;
    N1: string;
    N2: string;
    N3: string;
    N4: string;
    N5: string;
    N6: string;
    Nome: string;
    Descricao: string;
    constructor(Nome: string, CodLi: number) {
        this.Nome = Nome;
        this.CodLocalInstalacao = CodLi;
        this.CodInventarioAmbiente = null;
        this.CodPeso = null;
        this.CodPerfilCatalogo = null;
    }
}
export class EpiModeloAmbiente {
    CodEpiRiscoInventarioAmbiente: number;
    CodRiscoInventarioAmbiente: number;
    CodEPI: number;
    constructor(CodEPI: number, CodRisco: number, ) {

        this.CodEPI = CodEPI;
        this.CodRiscoInventarioAmbiente = CodRisco;
    }

}


export class AmbienteModel {
    // tslint:disable-next-line: variable-name
    isRascunho?: boolean;
    CodRascunhoInventarioAmbiente?: number;
    Local_antigo?: LocalInstalacaoModel[] = [];
    CodInventarioAmbiente: number;
    Codigo: string;
    CodAmbiente: number;
    Descricao: string;
    ObservacaoGeral: string;
    RiscoGeral: number;
    Ativo: boolean;
    DataAtualizacao: null;
    EightIDUsuarioModificador: string;
    NR_INVENTARIO_AMBIENTE: NRModel[] = [];
    RISCO_INVENTARIO_AMBIENTE: RiscoAmbiente[] = [];
    LOCAL_INSTALACAO_MODELO: LocalInstalacaoModel[] = [];

    constructor(CodInventarioAmbiente: number, Codigo: string, CodAmbiente: number, Descricao: string,
                ObservacaoGeral: string,  RiscoGeral: number, ) {
        this.CodInventarioAmbiente = CodInventarioAmbiente;
        this.Codigo = Codigo;
        this.CodAmbiente = CodAmbiente;
        this.Descricao = Descricao;
        this.ObservacaoGeral = ObservacaoGeral;
        this.RiscoGeral = RiscoGeral;
    }
    setRiscoInventario(RISCO_INVENTARIO_AMBIENTE: RiscoMapeadoModelo) {
        const epiModel: EpiModeloAmbiente[] = [];
        if (RISCO_INVENTARIO_AMBIENTE.EPI != null && RISCO_INVENTARIO_AMBIENTE.EPI !== undefined) {
            RISCO_INVENTARIO_AMBIENTE.EPI.forEach((epi) => {
                epiModel.push(new EpiModeloAmbiente(epi.CodEPI, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco));
            });
        }
        const risco: RiscoAmbiente = new RiscoAmbiente(0, this.CodInventarioAmbiente, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco,
            RISCO_INVENTARIO_AMBIENTE.Severidade.CodSeveridade,
            RISCO_INVENTARIO_AMBIENTE.Frequencia.CodProbabilidade, RISCO_INVENTARIO_AMBIENTE.FonteGeradora,
            RISCO_INVENTARIO_AMBIENTE.Procedimentos, RISCO_INVENTARIO_AMBIENTE.Contramedidas, epiModel);
        risco.id = RISCO_INVENTARIO_AMBIENTE.id;
        this.RISCO_INVENTARIO_AMBIENTE.push(risco);
    }
    setRiscoInventariorRiscoAmbiente(RISCO_INVENTARIO_AMBIENTE: RiscoAmbiente[]) {
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < RISCO_INVENTARIO_AMBIENTE.length; i++) {
            this.RISCO_INVENTARIO_AMBIENTE.push(RISCO_INVENTARIO_AMBIENTE[i]);
        }

    }
    setNrInventario( NR_INVENTARIO_AMBIENTE: NRModel[]) {
        this.NR_INVENTARIO_AMBIENTE = [... NR_INVENTARIO_AMBIENTE];
    }
    setLocalInventario( LOCAL_INSTALACAO_MODELO: any[]) {
        this.LOCAL_INSTALACAO_MODELO = [...LOCAL_INSTALACAO_MODELO];
    }
    // setLocalInstalacaoStatic(cod) {
    //     let local: LocalInstalacaoModel;
    //     local = new LocalInstalacaoModel(cod);
    //     this.LOCAL_INSTALACAO_MODELO.push(local);
    // }
    setCodAmbiente(CodAmbiente: number) {
        this.CodAmbiente = CodAmbiente;
    }
    setDescricao(Descricao: string) {
        this.Descricao = Descricao;
    }
    setObservacaoGeral(ObservacaoGeral: string) {
        this.ObservacaoGeral = ObservacaoGeral;
    }
    setRiscoGeral(RiscoGeral: number) {
        this.RiscoGeral = RiscoGeral;
    }

    editarRisco(RISCO_INVENTARIO_AMBIENTE: RiscoMapeadoModelo, id: string) {
        const epiModelo: EpiModeloAmbiente[] = [];
        RISCO_INVENTARIO_AMBIENTE.EPI.forEach((epi) => {
            epiModelo.push(new EpiModeloAmbiente(epi.CodEPI, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco));
        });

        // = new EpiModelo(2,RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco,0);
        const risco: RiscoAmbiente = new RiscoAmbiente(0, 0, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco,
            RISCO_INVENTARIO_AMBIENTE.Severidade.CodSeveridade, RISCO_INVENTARIO_AMBIENTE.Frequencia.CodProbabilidade,
            RISCO_INVENTARIO_AMBIENTE.FonteGeradora, RISCO_INVENTARIO_AMBIENTE.Procedimentos, RISCO_INVENTARIO_AMBIENTE.Contramedidas,
            epiModelo);
        risco.id = id;

        for (let i = 0; i < this.RISCO_INVENTARIO_AMBIENTE.length; i++) {
            if (  this.RISCO_INVENTARIO_AMBIENTE[i].id === id) {
                this.RISCO_INVENTARIO_AMBIENTE[i] = risco;
            }
          }
      }

}

