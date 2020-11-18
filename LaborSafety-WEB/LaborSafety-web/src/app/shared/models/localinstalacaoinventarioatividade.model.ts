export class LocalInstalacaoInventarioAtividade {
    CodLocalInstalacaoInventarioAtividade: number;
    CodInventarioAtividade: number;
    CodLocalInstalacao: number;
    ativo: boolean;
    nome: string;
    item: string;
    LocalInstalacao: LocalInstalacaoModelAtividade;

    constructor(  LocalInstalacao: LocalInstalacaoModelAtividade) {

        this.LocalInstalacao = LocalInstalacao;
    }
}
export class LocalInstalacaoModelAtividade {
        CodLocalInstalacao: number;
        CodInventarioAmbiente: number;
        InventariosAtividade: [];
        CodPeso: number;
        CodPerfilCatalogo: number;
        N1: string;
        N2: string;
        N3: string;
        N4: string;
        N5: string;
        N6: string;
        Nome: string;
        Descricao: string;
        constructor(model: any[]) {

        }
}
