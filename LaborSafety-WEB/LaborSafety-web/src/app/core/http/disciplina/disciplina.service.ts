import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Disciplina } from 'src/app/shared/models/disciplina.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const disciplinaRoute = environment.apiPaths.disciplina;

@Injectable({
    providedIn: 'root'
})
export class DisciplinaService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<Disciplina[]> {
        const path = host + disciplinaRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map( (res: any) => res.Data as Disciplina[] )).toPromise();
        return data;
    }
}
