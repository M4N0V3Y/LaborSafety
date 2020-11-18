export class AdministracaoPerfilModelo {
    CodFuncionalidade?: number;
    Edicao: boolean;
    EdicaoArea: number[];
    VisualizacaoArea: number[];


    constructor(CodFuncionalidade?: number, Edicao?: boolean, EdicaoArea?: number[], VisualizacaoArea?: number[]){
        this.CodFuncionalidade = CodFuncionalidade,
        this.Edicao = Edicao,
        this.EdicaoArea = EdicaoArea,
        this.VisualizacaoArea = VisualizacaoArea
    }
}