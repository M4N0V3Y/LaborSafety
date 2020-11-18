
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TelaPorPerfil } from 'src/app/shared/models/telaporperfil.model';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

const host = environment.host;


@Injectable({
  providedIn: 'root'
})
export class InvAmbienteService {
  constructor(private http: HttpClient) { }

  async gerarTodosLogs() {
    const path = host + environment.apiPaths.invAmbiente.getAllLogs();
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });
  }
 async filtrar(filtros) {
  const path = host + environment.apiPaths.invAmbiente.filtrar(filtros);
  return this.http
    .post(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data; });

  }
  async temAPR(codAmbiente) {
    const path = host + environment.apiPaths.invAmbiente.temAPR(codAmbiente);
    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data; });
    }
  async pesquisarInventarioRascunho(filtros: any) {
    const path = host + environment.apiPaths.rascunhoAmbiente.filtrarRascunho(filtros);
    return this.http
    .post(path, filtros)
    .toPromise()
    .then((data: any) => {
      return data.Data; });
  }
  async getNomeEPIporId(listaIds) {

    const path = host + environment.apiPaths.epi.getEPIsporIds(listaIds);
    return this.http
    .post(path, listaIds)
    .toPromise()
    .then((data: any) => {
      return data.Data; });

  }

  async getAllSOs() {

    const path = host + environment.apiPaths.invAmbiente.getAllSOs();
    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data; });
  }

  async editSO(model: any) {
    const path = host + environment.apiPaths.invAmbiente.editarSO(model);
    return this.http
    .put(path, model)
    .toPromise()
    .then((data: any) => data);
  }

  async deleteSO(code) {
    const path = host + environment.apiPaths.invAmbiente.deletarSO(code);
    return this.http
    .delete(path, code)
    .toPromise()
    .then((data) => {
      return data;
    });
  }

  async getRascunhoPorId(id: number) {
    const path = host + environment.apiPaths.rascunhoAmbiente.getPorId(id);

    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });

  }

  async insertSO(ambiente) {
    const path = host + environment.apiPaths.invAmbiente.insertSO(ambiente);
    return this.http.put(path, ambiente).toPromise().then((data: any) => data);
  }

  async getPorId(id) {
    const path = host + environment.apiPaths.invAmbiente.getPorId(id);

    return this.http
      .get(path)
      .toPromise()
      .then((data: any) => {
        return data.Data;
      });

  }
  async inserir(inventarioAmbiente) {
    const path = host + environment.apiPaths.invAmbiente.inserir(inventarioAmbiente);

    return this.http
    .put(path, inventarioAmbiente)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async inserirRascunho(inventarioAmbienteRascunho) {
    const path = host + environment.apiPaths.rascunhoAmbiente.inserirRascunho(inventarioAmbienteRascunho);

    return this.http
    .put(path, inventarioAmbienteRascunho)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async editarRascunho(inventarioAmbienteRascunho) {
    const path = host + environment.apiPaths.rascunhoAmbiente.editarRascunho(inventarioAmbienteRascunho);

    return this.http
    .put(path, inventarioAmbienteRascunho)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }
  async editar(inventarioAmbiente) {
    const path = host + environment.apiPaths.invAmbiente.editar(inventarioAmbiente);

    return this.http
    .put(path, inventarioAmbiente)
    .toPromise()
    .then((data: any) => {
      return data;
    });
  }

  async gerarLog(ids) {
    const path = host + environment.apiPaths.invAmbiente.gerarLog(ids);

    return this.http
    .put(path, ids)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

  async deletar(model) {
    const path = host + environment.apiPaths.invAmbiente.deletar(model);

    return this.http
    .post(path, model)
    .toPromise()
    .then((data: any) => {
      return data;
    });
  }
  async deletarRascunho(id) {
    const path = host + environment.apiPaths.rascunhoAmbiente.deletarRascunho(id);

    return this.http
    .delete(path, id)
    .toPromise()
    .then((data: any) => {
      return data;
    });
  }
  async listarTodasNRs() {
    const path = host + environment.apiPaths.invAmbiente.listarTodasNRs();

    return this.http
    .get(path)
    .toPromise()
    .then((data: any) => {
      return data.Data;
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
  async calcularRiscoGeral(riscoTotalAmbienteModelo	) {
    const path = host + environment.apiPaths.risco.calcularRiscoGeral(riscoTotalAmbienteModelo);
    return this.http
    .put(path, riscoTotalAmbienteModelo)
    .toPromise()
    .then((data: any) => {
      return data.Data;
    });
  }

}
