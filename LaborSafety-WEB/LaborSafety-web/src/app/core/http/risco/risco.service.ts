import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Risco } from 'src/app/shared/models/risco.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const riscoRoute = environment.apiPaths.risco;

@Injectable({
    providedIn: 'root'
})
export class RiscoService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<Risco[]> {
        const path = host + riscoRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as Risco[])).toPromise();
        return data;
    }

    async get(id: number): Promise<Risco[]> {
        const path = host + riscoRoute.get(id);
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as Risco[])).toPromise();
        return data;
    }
}
