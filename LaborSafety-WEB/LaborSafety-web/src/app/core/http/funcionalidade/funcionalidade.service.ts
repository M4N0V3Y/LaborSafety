import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Funcionalidade } from 'src/app/shared/models/funcionalidades.model';
const host = environment.host;
const funcionalidadeRoute = environment.apiPaths.funcionalidade;

@Injectable({
  providedIn: 'root'
})
export class FuncionalidadeService {

  constructor(private http: HttpClient) { }
  async getAll(): Promise<Funcionalidade[]> {
    const path = host + funcionalidadeRoute.getAll;

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Funcionalidade[]))
      .toPromise();
    return data;
  }
}
