export class filtrosModel {
    CodAmbiente: number;
    CodLocaisInstalacao: number[];
      Riscos: [];
      CodSeveridade: number;
      CodProbabilidade: number;
    constructor(CodSistemaOperacional: number, Riscos, CodSeveridade: number, CodProbabilidade: number) {
        this.CodAmbiente = CodSistemaOperacional;
        this.Riscos = Riscos;
        this.CodSeveridade = CodSeveridade;
        this.CodProbabilidade = CodProbabilidade;

    }
    setLocaisInstalacao(NomeLocalInstalacao: any[]) {
        let model: any [] = [];
        NomeLocalInstalacao.forEach((li) =>
        model.push(li.CodLocalInstalacao)
        );
        this.CodLocaisInstalacao = [...model];
    }
    setCodSistemaOperacional(CodSistemaOperacional) {
        this.CodAmbiente = CodSistemaOperacional;

    }
    }
