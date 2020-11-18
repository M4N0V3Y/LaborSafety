import { LocalInstalacaoInventarioAtividade, LocalInstalacaoModelAtividade } from './localinstalacaoinventarioatividade.model';
import { ResponsavelInventarioAtividade } from './responsavelinventarioatividade.model';
import { RiscoInventarioAtividade, EpiModeloAtividade } from './riscoinventarioatividade.model';
import {RiscoMapeadoModelo} from './riscomapeado.model';
export class InventarioAtividadeModelo {
    isRascunho?: boolean;
    CodRascunhoInventarioAtividade?: number;
    CodInventarioAtividade: number;
    Codigo: string;
    CodPeso: number;
    CodPerfilCatalogo: number;
    CodDuracao: number;
    CodAtividade: number;
    CodDisciplina: number;
    Descricao: string;
    RiscoGeral: number;
    ObservacaoGeral: string;
    Ativo: boolean;
    dataAtualizacao: null;
    EightIDUsuarioModificador: string;

    LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE: LocalInstalacaoInventarioAtividade[] = [];
    RISCO_INVENTARIO_ATIVIDADE: RiscoInventarioAtividade[] = [];

    constructor(CodInventarioAtividade: number, Codigo: string, CodPeso: number,
                CodPerfilCatalogo: number, CodDuracao: number, CodAtividade: number,
                CodDisciplina: number, Descricao: string, RiscoGeral: number,
                ObservacaoGeral: string) {
      this.CodInventarioAtividade = CodInventarioAtividade;
      this.Codigo = Codigo;
      this.CodPeso = CodPeso;
      this.CodPerfilCatalogo = CodPerfilCatalogo;
      this.CodDuracao = CodDuracao;
      this.CodAtividade = CodAtividade;
      this.CodDisciplina = CodDisciplina;
      this.Descricao = Descricao;
      this.RiscoGeral = RiscoGeral;
      this.ObservacaoGeral = ObservacaoGeral;
    }
    setRiscoInventario(RISCO_INVENTARIO_ATIVIDADE: RiscoMapeadoModelo) {

      const epiModel: EpiModeloAtividade[] = [];
      if (RISCO_INVENTARIO_ATIVIDADE.EPI != null && RISCO_INVENTARIO_ATIVIDADE.EPI !== undefined) {
        RISCO_INVENTARIO_ATIVIDADE.EPI.forEach((epi) => {
            epiModel.push(new EpiModeloAtividade(epi.CodEPI, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0));
        });
      }
      const risco: RiscoInventarioAtividade =
        new RiscoInventarioAtividade(0, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, this.CodInventarioAtividade,
        RISCO_INVENTARIO_ATIVIDADE.Severidade.CodSeveridade, RISCO_INVENTARIO_ATIVIDADE.FonteGeradora,
        RISCO_INVENTARIO_ATIVIDADE.Procedimentos, RISCO_INVENTARIO_ATIVIDADE.Contramedidas, epiModel);
      risco.id = RISCO_INVENTARIO_ATIVIDADE.id;

      this.RISCO_INVENTARIO_ATIVIDADE.push(risco);

  }
  // TESTAR
  setLocalInstalacaoStatic(cod) {
    let local: LocalInstalacaoInventarioAtividade;
    local = new LocalInstalacaoInventarioAtividade(cod);
    this.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.push(local);
}
  editarRisco(RISCO_INVENTARIO_ATIVIDADE: RiscoMapeadoModelo, id: string) {
    const epiModel: EpiModeloAtividade[] = [];
    if (RISCO_INVENTARIO_ATIVIDADE.EPI != null && RISCO_INVENTARIO_ATIVIDADE.EPI !== undefined) {
      RISCO_INVENTARIO_ATIVIDADE.EPI.forEach((epi) => {
          epiModel.push(new EpiModeloAtividade(epi.CodEPI, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0));
      });
    }
    // = new EpiModelo(2,RISCO_INVENTARIO_AMBIENTE.Risco.CodRisco,0);
    const risco: RiscoInventarioAtividade =
      new RiscoInventarioAtividade(0, RISCO_INVENTARIO_ATIVIDADE.Risco.CodRisco, 0,
      RISCO_INVENTARIO_ATIVIDADE.Severidade.CodSeveridade,
      RISCO_INVENTARIO_ATIVIDADE.FonteGeradora, RISCO_INVENTARIO_ATIVIDADE.Procedimentos,
      RISCO_INVENTARIO_ATIVIDADE.Contramedidas, epiModel);
    risco.id = id;

    for (let i = 0; i < this.RISCO_INVENTARIO_ATIVIDADE.length; i++) {
        if (  this.RISCO_INVENTARIO_ATIVIDADE[i].id === id) {
            this.RISCO_INVENTARIO_ATIVIDADE[i] = risco;
        }
      }
  }

  setLocalInventario(  LOCAL_INSTALACAO_MODELO: LocalInstalacaoModelAtividade[]) {
    const model: LocalInstalacaoInventarioAtividade[] = [];
    LOCAL_INSTALACAO_MODELO.forEach((li) => {
      model.push(new LocalInstalacaoInventarioAtividade(li));
    });
    this.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE = [...model];
  }

  setCodDisciplina(CodDisciplina) {
     this.CodDisciplina = CodDisciplina;
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
  setCodInventarioAtividade( CodInventarioAtividade: number) {
    this.CodInventarioAtividade = CodInventarioAtividade;
  }
  setCodPeso(CodPeso) {
      this.CodPeso = CodPeso;
  }
  setCodPerfilCatalogo(CodPerfilCatalogo) {
      this.CodPerfilCatalogo = CodPerfilCatalogo;
  }
  setCodDuracao(CodDuracao) {
    this.CodDuracao = CodDuracao;
  }
  setCodAtividade(CodAtividade) {
      this.CodAtividade = CodAtividade;
  }

  }
