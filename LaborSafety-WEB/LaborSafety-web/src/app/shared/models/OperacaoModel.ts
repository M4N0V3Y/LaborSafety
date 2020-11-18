import { Disciplina } from './disciplina.model';
import { Atividade } from './atividade.model';
import { LocalInstalacaoModel } from './invAmbinteModel';

export class OperacaoModel {
    CodOperacaoAPR?: number;
    descricao: string;
    disciplina: Disciplina;
    atividade: Atividade;
    localDeInstalacao: LocalInstalacaoModel;
    NomeLI: string;
    riscoOperacao: number;
    CodLI: number;
    Codigo?: string;
    constructor() {
       /* this.atividade = atividade;
        this.descricao = descricao;
        this.disciplina = disciplina;
        this.localDeInstalacao = localDeInstalacao;
        this.NomeLI = localDeInstalacao.Nome;*/
    }
    setDisciplina(codDisciplina: number, Nome: string) {
        this.disciplina = new Disciplina(codDisciplina, Nome, '');
    }
    setAtividade(codAtividade: number, Nome: string) {
        this.atividade = new Atividade(codAtividade, Nome, '');
    }
    setLI(NomeLI: string, CodLi) {
        this.localDeInstalacao = new LocalInstalacaoModel(NomeLI, CodLi);
    }
}
