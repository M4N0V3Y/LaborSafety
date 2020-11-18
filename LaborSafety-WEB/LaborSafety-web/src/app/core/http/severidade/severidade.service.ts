import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Severidade } from 'src/app/shared/models/severidade.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const severidadeRoute = environment.apiPaths.severidade;

@Injectable({
    providedIn: 'root'
})
export class SeveridadeService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<Severidade[]> {
        const path = host + severidadeRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as Severidade[])).toPromise();
        return data;
    }
}
