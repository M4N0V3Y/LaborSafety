import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { map } from 'rxjs/operators';
import { InventarioAtividadeModelo } from 'src/app/shared/models/InventarioAtividadeModelo';

const host = environment.host;
const localInstalacaoRoute = environment.apiPaths.localInstalacao;


@Injectable({
  providedIn: 'root'
})
export class GestaoAtividadeInventarioService {
  constructor(private http: HttpClient) {}

  async getLocalInstalacao(levelObject) {
    const path = host + localInstalacaoRoute.filtrar(levelObject);
    const data = this.http
      .post(path, levelObject)
      .pipe(map((res: any) => res.Data as any))
      .toPromise();
    return data;
  }
 
  async pesquisarInventario(dados: any): Promise<any> {
    const path = host + environment.apiPaths.invAtividade.filtrar(dados);
    const data = await this.http
      .post(path, dados)
      .pipe(map((res: any) => res.Data as InventarioAtividadeModelo[]))
      .toPromise();
    return data;
  }

  async pesquisarInventarioRascunho(dados: any): Promise<any> {
    const path = host + environment.apiPaths.rascunhoAtividade.filtrarRascunho(dados);
    const data = await this.http
      .post(path, dados)
      .pipe(map((res: any) => res.Data as InventarioAtividadeModelo[]))
      .toPromise();
    return data;
  }
  async inserirInventario(dados: any): Promise<any> {
    const path = host + environment.apiPaths.invAtividade.inserir(dados);
    const data = await this.http
      .put(path, dados)
      .toPromise();
    return data;
  }
}
