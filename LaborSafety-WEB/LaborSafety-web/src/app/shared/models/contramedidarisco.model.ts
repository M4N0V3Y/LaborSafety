export class ContraMedidaRisco {
    CodContraMedidaRisco: number;
    CodContraMedida: number;
    CodRisco: number;

    constructor(codContraMedidaRisco: number, codContraMedida: number, codRisco: number) {
        this.CodContraMedida = codContraMedida;
        this.CodContraMedidaRisco = codContraMedidaRisco;
        this.CodRisco = codRisco;
    }
}