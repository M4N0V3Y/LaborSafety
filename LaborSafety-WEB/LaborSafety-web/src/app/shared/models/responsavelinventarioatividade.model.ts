export class ResponsavelInventarioAtividade {
    CodResponsavelInventarioAtividade: number;
    CodUsuario: number;
    CodInventarioAtividade: number;

    constructor(codResponsavelInventarioAtividade: number, codUsuario: number, codInventarioAtividade: number) {
        this.CodResponsavelInventarioAtividade = codResponsavelInventarioAtividade;
        this.CodUsuario = codUsuario;
        this.CodInventarioAtividade = codInventarioAtividade;
    }
}