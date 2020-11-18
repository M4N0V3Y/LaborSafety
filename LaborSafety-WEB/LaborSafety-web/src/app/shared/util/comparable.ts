export class Comparable {

    static compare(value1: any, value2: any): boolean {
        // tslint:disable-next-line: triple-equals
        return value1 && value2 && (value1 == value2);
    }
}
