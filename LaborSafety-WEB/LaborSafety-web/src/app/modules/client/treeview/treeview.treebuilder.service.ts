import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TreeviewService } from 'src/app/core/http/treeView/treeview.service';

@Injectable()
export class TreeBuilder {
  dataChange = new BehaviorSubject<any[]>([]);

  get data(): any[] { return this.dataChange.value; }

  constructor(private treeViewService: TreeviewService) {
    this.initialize(treeViewService);
  }

  initialize(treeViewService: TreeviewService) {
    const rawData = treeViewService.getDadosTreeView();

    if (rawData) {
      const data = this.buildFileTree(rawData, 0);
      this.dataChange.next(data);
    }
  }

  buildFileTree(array: {[key: string]: any}, level: number): any[] {
    const objArray = this.convertArrayToObject(array, 'Nome');

    return Object.keys(objArray).reduce<any[]>((accumulator, key) => {
      const value = objArray[key];
      const node = { nome : '', Filhos: [], CodLocalInstalacao: 0};
      node.nome = key;
      node.CodLocalInstalacao = value.CodLocalInstalacao;

      if (value.Filhos.length !== 0) {
        node.Filhos = this.buildFileTree(value.Filhos, level + 1);
      } else {
        node.nome = value.Nome;
      }

      return accumulator.concat(node);
    }, []);
  }

  convertArrayToObject = (array, key) => {
    const initialValue = {};
    return array.reduce((obj, item) => {
      return {
        ...obj,
        [item[key]]: item,
      };
    }, initialValue);
  }
}
