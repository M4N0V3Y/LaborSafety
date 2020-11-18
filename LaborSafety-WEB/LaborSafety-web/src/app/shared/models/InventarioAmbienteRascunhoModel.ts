import { EpiRascunhoModelo } from './epiRascunhoModel';
import { LocalInstalacaoModel } from './invAmbinteModel';
import { RiscoMapeadoModelo } from './riscomapeado.model';

export class NR_Rascunho {
    CodNrRascunhoInventarioAmbiente: number;
    CodNR: number;
    CodRascunhoInventarioAmbiente: number;
    Ativo: true;
    constructor( CodNR: number) {
        this.CodNR = CodNR;
    }
}
export class RISCO_RASCUNHO_INVENTARIO_AMBIENTE {
    id?: string;
    CodRascunhoRiscoInventarioAmbiente: number;
    CodRascunhoInventarioAmbiente: number;
    CodRiscoAmbiente: number;
    CodSeveridade: number;
    CodProbabilidade: number;
    FonteGeradora: string;
    ProcedimentosAplicaveis: string;
    ContraMedidas: string;
    EPIRiscoRascunhoInventarioAmbiente: EpiRascunhoModelo[] = [];


    constructor( CodRiscoInventarioAmbiente: number, CodInventarioAmbiente: number, CodRiscoAmbiente: number,
                 CodSeveridade: number, CodProbabilidade: number, FonteGeradora: string, ProcedimentosAplicaveis: string,
                 ContraMedidas: string , EpiRiscoModelo: EpiRascunhoModelo[]) {

            this.CodRascunhoRiscoInventarioAmbiente = CodRiscoInventarioAmbiente;
            this.CodRascunhoInventarioAmbiente = CodInventarioAmbiente;
            this.CodRiscoAmbiente = CodRiscoAmbiente;
            this.CodSeveridade = CodSeveridade;
            this.CodProbabilidade = CodProbabilidade;
            this.FonteGeradora = FonteGeradora;
            this.ProcedimentosAplicaveis = ProcedimentosAplicaveis;
            this.EPIRiscoRascunhoInventarioAmbiente = EpiRiscoModelo;
            this.ContraMedidas = ContraMedidas;
    }
}


export class InventarioAmbienteRascunho {

    CodRascunhoInventarioAmbiente: number;
    Codigo: string;
    CodAmbiente: number;
    CodLocalInstalacao: string;
    NomeLocalInstalacao: string;
    Descricao: string;
    ObservacaoGeral: string;
    RiscoGeral: number;
    novoInventario: boolean;
    EightIDUsuarioModificador: string;
    NR_RASCUNHO_INVENTARIO_AMBIENTE: NR_Rascunho[] = [];
    RISCO_RASCUNHO_INVENTARIO_AMBIENTE: RISCO_RASCUNHO_INVENTARIO_AMBIENTE[] = [];
    LOCAL_INSTALACAO_MODELO: LocalInstalacaoModel[] = [];

    setLocalInventario( LOCAL_INSTALACAO_MODELO: any[]) {
        this.LOCAL_INSTALACAO_MODELO = [...LOCAL_INSTALACAO_MODELO];
    }
    setRiscoInventario(RISCO_INVENTARIO_AMBIENTE: RiscoMapeadoModelo) {
        const epiModel: EpiRascunhoModelo[] = [];
        if (RISCO_INVENTARIO_AMBIENTE.EPI != null && RISCO_INVENTARIO_AMBIENTE.EPI !== undefined) {
            RISCO_INVENTARIO_AMBIENTE.EPI.forEach((epi) => {
                epiModel.push(new EpiRascunhoModelo(epi.CodEPI, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco, 0));
            });
        }
        const risco: RISCO_RASCUNHO_INVENTARIO_AMBIENTE = new RISCO_RASCUNHO_INVENTARIO_AMBIENTE(0,
            this.CodRascunhoInventarioAmbiente, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco,
            RISCO_INVENTARIO_AMBIENTE.Severidade.CodSeveridade,
            RISCO_INVENTARIO_AMBIENTE.Frequencia.CodProbabilidade, RISCO_INVENTARIO_AMBIENTE.FonteGeradora,
            RISCO_INVENTARIO_AMBIENTE.Procedimentos, RISCO_INVENTARIO_AMBIENTE.Contramedidas, epiModel);
        risco.id = RISCO_INVENTARIO_AMBIENTE.id;
        this.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.push(risco);
    }
    editarRisco(RISCO_INVENTARIO_AMBIENTE: RiscoMapeadoModelo, id: string) {
        const epiModelo: EpiRascunhoModelo[] = [];
        RISCO_INVENTARIO_AMBIENTE.EPI.forEach((epi) => {
            epiModelo.push(new EpiRascunhoModelo(epi.CodEPI, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco, 0));
        });

        const risco: RISCO_RASCUNHO_INVENTARIO_AMBIENTE = new RISCO_RASCUNHO_INVENTARIO_AMBIENTE
        (0, 0, RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco, RISCO_INVENTARIO_AMBIENTE.Severidade.CodSeveridade,
            RISCO_INVENTARIO_AMBIENTE.Frequencia.CodProbabilidade, RISCO_INVENTARIO_AMBIENTE.FonteGeradora,
            RISCO_INVENTARIO_AMBIENTE.Procedimentos, RISCO_INVENTARIO_AMBIENTE.Contramedidas, epiModelo);
        risco.id = id;

        for (let i = 0; i < this.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.length; i++) {
            if (  this.RISCO_RASCUNHO_INVENTARIO_AMBIENTE[i].id === id) {
                this.RISCO_RASCUNHO_INVENTARIO_AMBIENTE[i] = risco;
            }
          }
      }
}
