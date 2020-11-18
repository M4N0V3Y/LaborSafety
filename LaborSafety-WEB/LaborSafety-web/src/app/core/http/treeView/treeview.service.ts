import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
const host = environment.host;
const treeviewRoute = environment.apiPaths.treeview;

@Injectable({
  providedIn: 'root'
})
export class TreeviewService {
  private dadosTreeView: any;

  constructor() { }

  // async getAll(): Promise<Nivel[]> {
  //   const path = host + treeviewRoute.getAll;

  //   const data = await this.http
  //     .get(path)
  //     .pipe(map((res: any) => res.Data as Nivel[]))
  //     .toPromise();
  //   return data;
  // }

  getDadosTreeView(): Promise<any> {
    return this.dadosTreeView;
  }

  setDadosTreeView(dadosTreeView): Promise<any> {
    this.dadosTreeView = dadosTreeView;
    return dadosTreeView;
  }
}
