import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PesoFisico } from 'src/app/shared/models/pesofisico.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const pesofisicoRoute = environment.apiPaths.pesofisico;

@Injectable({
    providedIn: 'root'
})
export class PesoService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<PesoFisico[]> {
        const path = host + pesofisicoRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as PesoFisico[])).toPromise();
        return data;
    }
}
