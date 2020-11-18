import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Duracao } from 'src/app/shared/models/duracao.model';
const host = environment.host;
const duracaoRoute = environment.apiPaths.duracao;

@Injectable({
  providedIn: 'root'
})
export class DuracaoService {

  constructor(private http: HttpClient) { }
  async getAll(): Promise<Duracao[]> {
    const path = host + duracaoRoute.getAll;

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Duracao[]))
      .toPromise();
    return data;
  }
}
