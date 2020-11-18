import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Atividade } from 'src/app/shared/models/atividade.model';
import {InventarioAtividadeModelo} from 'src/app/shared/models/InventarioAtividadeModelo';
const host = environment.host;
const atividadeRoute = environment.apiPaths.atividade;

@Injectable({
  providedIn: 'root'
})
export class AtividadeService {

  constructor(private http: HttpClient) { }
  async getAll(): Promise<Atividade[]> {
    const path = host + atividadeRoute.getAll;

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Atividade[]))
      .toPromise();
    return data;
  }
  async gerarTodosLogs() {
    const path = host + atividadeRoute.getAllLogs();
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });
  }
  async getPorId(id): Promise<InventarioAtividadeModelo> {
    const path = host + atividadeRoute.getPorId(id);

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as InventarioAtividadeModelo))
      .toPromise();
    return data;
  }
  async getRascunhoPorId(id) {
    const path = host + environment.apiPaths.rascunhoAtividade.getPorId(id);
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });
  }
  async gerarLog(ids) {
    const path = host + environment.apiPaths.atividade.gerarLog(ids);
    return this.http
      .put(path, ids)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });
  }
  async inserir(inventario) {
    const path = host + atividadeRoute.inserir(inventario);
    return this.http
      .put(path, inventario)
      .toPromise()
      .then((data) => {
        return data;
      });

  }
  async editarRascunho(inventarioRascunho) {
    const path = host + environment.apiPaths.rascunhoAtividade.editarRascunho(inventarioRascunho);
    return this.http
      .put(path, inventarioRascunho)
      .toPromise()
      .then((data) => {
        return data;
      });

  }

  async inserirRascunho(inventarioAtividadeRascunho) {
    const path = host + environment.apiPaths.rascunhoAtividade.inserirRascunho(inventarioAtividadeRascunho);

    return this.http
    .put(path, inventarioAtividadeRascunho)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async editar(inventario) {
    const path = host + atividadeRoute.editar(inventario);
    return this.http
    .put(path, inventario)
    .toPromise()
    .then((data) => {
      return data;
    });
  }

  async temAPR(codAtividade) {
    const path = host + atividadeRoute.temAPR(codAtividade);
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data; });
    }

  async deletar(model) {
    const path = host + atividadeRoute.deletar(model);
    return this.http
    .post(path, model)
    .toPromise()
    .then((data) => {
      return data;
    });
  }
  async deletarRascunho(id) {
    const path = host + environment.apiPaths.rascunhoAtividade.deletarRascunho(id);
    return this.http
    .delete(path, id)
    .toPromise()
    .then((data) => {
      return data;
    });
  }

  async getLocal(nivel) {
    const path = host + atividadeRoute.getLocal(nivel);
    return this.http
    .post(path, nivel)
    .toPromise()
    .then((data) => {
      return data;
    });
  }
  async getListaTelasEFuncionalidadesPorPerfil(codPerfil) {
    const path = host + environment.apiPaths.permissaoPerfil.getListaTelasEFuncionalidadesPorPerfil(codPerfil);
    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async calcularRiscoGeral(model) {
    const path = host + environment.apiPaths.risco.calcularRiscoGeralAtividade(model);
    return this.http
    .post(path, model)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
}
