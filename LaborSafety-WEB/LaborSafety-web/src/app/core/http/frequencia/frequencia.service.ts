import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Frequencia } from 'src/app/shared/models/frequencia.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const frequenciaRoute = environment.apiPaths.frequencia;

@Injectable({
    providedIn: 'root'
})
export class FrequenciaService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<Frequencia[]> {
        const path = host + frequenciaRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as Frequencia[])).toPromise();
        return data;
    }
}
