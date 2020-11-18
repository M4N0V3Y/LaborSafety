export class ImportacaoModel {
    status: boolean;
    erros: ErrosImportacao[];
}

export class ErrosImportacao {
    codigo: number;
    descricao: string;
    celula: string;
}