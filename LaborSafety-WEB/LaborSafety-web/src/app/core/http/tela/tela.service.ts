import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Tela } from 'src/app/shared/models/tela.model';
const host = environment.host;
const telaRoute = environment.apiPaths.tela;

@Injectable({
  providedIn: 'root'
})
export class TelaService {

  constructor(private http: HttpClient) { }
  async getAll(): Promise<Tela[]> {
    const path = host + telaRoute.getAll;

    const data = await this.http
      .get(path)
      .pipe(map((res: any) => res.Data as Tela[]))
      .toPromise();
    return data;
  }
}
