import {Factory} from './factory';
import {MIN_DATE} from './consts';
import {camelCase} from '@swimlane/ngx-datatable/release/utils';

export class CSVExtractor {

    // static async extractContent<T>(files: FileList, parameters: CSVParameter[]) {
    static async extractContent<T>(files: FileList, type: (new () => T)): Promise<T[]> {
        const csvContent = await this.extractCSV(files);

        const result: T[] = [];
        // Ignora o cabeçalho do csv
        if (csvContent && csvContent.length > 1) {

            const properties = this.extractProperties(csvContent[0]);

            // Iteração pelas linhas
            for (let i = 1; i < csvContent.length; i++) {
                const item = Factory.create(type);
                // Iteração pelos itens da linha
                for (let j = 0; j < properties.length; j++) {
                    item[camelCase(properties[j])] = Factory.getValue(item, properties[j], csvContent[i][j]);
                }

                for (const key in item) {
                    if (item.hasOwnProperty(key)) {
                        const element = item[key];
                        if (element instanceof Date && element === MIN_DATE){
                            item[key] = null;
                        }
                    }
                }

                // Adiciona o novo item ao vetor                
                result.push(item);
            }
        }
        return result;
    }

    private static extractProperties(header: string[]): string[] {
        return header.map(parameter => parameter);
    }

    private static extractCSV(files: FileList): Promise<string[][]> {
        return new Promise(resolve => {
            if (files && files.length > 0) {
                const file: File = files.item(0);
                const reader: FileReader = new FileReader();
                reader.readAsText(file);
                reader.onload = () => {
                    const conteudo: string = reader.result as string;
                    // Separa por quebra de linha e remove linhas vazias sem conteúdo
                    const linhas = conteudo.split('\n').filter(linha => {
                        return linha.replace(/;/g, '').replace(/"/g, '').trim() !== '';
                    });
                    resolve(linhas.map(linha => linha.split(';')));
                };
            }
        });
    }
}
