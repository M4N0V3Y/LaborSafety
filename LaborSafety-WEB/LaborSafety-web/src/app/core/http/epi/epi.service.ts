import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { EPI } from 'src/app/shared/models/EPI.model';
const host = environment.host;
const epiRoute = environment.apiPaths.epi;

@Injectable({
    providedIn: 'root'
})
export class EPIService {
    constructor(private httpClient: HttpClient) { }

    async ListarTodosEPIPorNivel(nome: string): Promise<EPI[]> {
        const path = host + epiRoute.getPerLevel(nome);
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as EPI[])).toPromise();
        return data;
    }

    async ListarTodosEPIs(): Promise<EPI[]> {
        const path = host + epiRoute.getAll();
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as EPI[])).toPromise();
        return data;
    }
}
