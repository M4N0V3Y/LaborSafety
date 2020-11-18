import { LocalInstalacaoModelAtividade } from './localinstalacaoinventarioatividade.model';
import { RiscoInventarioAtividade } from './riscoinventarioatividade.model';
import { RiscoMapeadoModelo } from './riscomapeado.model';
import { EpiRascunhoModelo, EpiRascunhoAtividadeModelo } from './epiRascunhoModel';


// tslint:disable-next-line: class-name
export class LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE {
  CodLocalInstalacaoRascunhoInventarioAtividade: number;
  CodRascunhoInventarioAtividade: number;
  CodLocalInstalacao: number;
  Ativo: true;
  LocalInstalacao: LocalInstalacaoModelAtividade;

  constructor(  LocalInstalacao: LocalInstalacaoModelAtividade) {
    this.LocalInstalacao = LocalInstalacao;
  }

}
// tslint:disable-next-line: class-name
export class RISCO_RASCUNHO_INVENTARIO_ATIVIDADE {
  id?: string;
  CodRiscoRascunhoInventarioAtividade: number;
  CodRisco: number;
  CodRascunhoInventarioAtividade: number;
  CodSeveridade: number;
  FonteGeradora: string;
  ProcedimentoAplicavel: string;
  ContraMedidas: string;
  Ativo: true;
  EPIRiscoRascunhoInventarioAtividadeModelo: EpiRascunhoAtividadeModelo [];
  constructor(CodRiscoInventarioAtividade: number, codRisco: number, codInventarioAtividade: number,
              codSeveridade: number, fonteGeradora: string,
              procedimentoAplicavel: string, ContraMedidas: string, EpiRiscoModelo: EpiRascunhoAtividadeModelo[]) {
    this.CodRiscoRascunhoInventarioAtividade = codInventarioAtividade;
    this.CodRisco = codRisco;
    this.CodRascunhoInventarioAtividade = CodRiscoInventarioAtividade;
    this.CodSeveridade = codSeveridade;
    this.FonteGeradora = fonteGeradora;
    this.ProcedimentoAplicavel = procedimentoAplicavel;
    this.EPIRiscoRascunhoInventarioAtividadeModelo = EpiRiscoModelo;
    this.ContraMedidas = ContraMedidas;
  }
}



export class InventarioAtividadeRascunho {
    novoInventario?: boolean;
    CodRascunhoInventarioAtividade = null;
    Codigo: string;
    CodPeso: number;
    CodPerfilCatalogo: number;
    CodDuracao: number;
    CodAtividade: number;
    CodDisciplina: number;
    Descricao: string;
    RiscoGeral: number;
    ObservacaoGeral: string;
    EightIDUsuarioModificador: string;
    LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE: LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE[] = [];
    RISCO_RASCUNHO_INVENTARIO_ATIVIDADE: RISCO_RASCUNHO_INVENTARIO_ATIVIDADE[] = [];

    setLocalInventario( LOCAL_INSTALACAO_MODELO: LocalInstalacaoModelAtividade[]) {
        const model: LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE[] = [];
        LOCAL_INSTALACAO_MODELO.forEach((li) => {
          model.push(new LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE(li));
        });
        this.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE = [...model];
      }

      setRiscoInventario(RISCO_INVENTARIO_ATIVIDADE: RiscoMapeadoModelo) {
        const epiModel: EpiRascunhoAtividadeModelo[] = [];
        if (RISCO_INVENTARIO_ATIVIDADE.EPI != null && RISCO_INVENTARIO_ATIVIDADE.EPI !== undefined) {
          RISCO_INVENTARIO_ATIVIDADE.EPI.forEach((epi) => {
              epiModel.push(new EpiRascunhoAtividadeModelo(epi.CodEPI, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0));
          });
        }
        const risco: RISCO_RASCUNHO_INVENTARIO_ATIVIDADE =
          new RISCO_RASCUNHO_INVENTARIO_ATIVIDADE(0, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, this.CodRascunhoInventarioAtividade,
          RISCO_INVENTARIO_ATIVIDADE.Severidade.CodSeveridade, RISCO_INVENTARIO_ATIVIDADE.FonteGeradora,
          RISCO_INVENTARIO_ATIVIDADE.Procedimentos, RISCO_INVENTARIO_ATIVIDADE.Contramedidas, epiModel);
        risco.id = RISCO_INVENTARIO_ATIVIDADE.id;
        this.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.push(risco);

    }

    editarRisco(RISCO_INVENTARIO_ATIVIDADE: RiscoMapeadoModelo, id: string) {
      const epiModel: EpiRascunhoAtividadeModelo[] = [];
      if (RISCO_INVENTARIO_ATIVIDADE.EPI != null && RISCO_INVENTARIO_ATIVIDADE.EPI !== undefined) {
        RISCO_INVENTARIO_ATIVIDADE.EPI.forEach((epi) => {
            epiModel.push(new EpiRascunhoAtividadeModelo(epi.CodEPI, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0));
        });
      }
      const risco: RISCO_RASCUNHO_INVENTARIO_ATIVIDADE =
        new RISCO_RASCUNHO_INVENTARIO_ATIVIDADE(0, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0,
        RISCO_INVENTARIO_ATIVIDADE.Severidade.CodSeveridade, RISCO_INVENTARIO_ATIVIDADE.FonteGeradora,
        RISCO_INVENTARIO_ATIVIDADE.Procedimentos, RISCO_INVENTARIO_ATIVIDADE.Contramedidas, epiModel);
      risco.id = id;
      for (let i = 0; i < this.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.length; i++) {
          if (  this.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE[i].id === id) {
              this.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE[i] = risco;
          }

        }


    }
}


