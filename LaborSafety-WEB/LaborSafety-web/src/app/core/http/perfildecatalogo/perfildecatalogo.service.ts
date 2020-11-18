import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PerfilDeCatalogo } from 'src/app/shared/models/perfildecatalogo.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const perfildecatalogoRoute = environment.apiPaths.perfildecatalogo;

@Injectable({
    providedIn: 'root'
})
export class PerfilDeCatalogoService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<PerfilDeCatalogo[]> {
        const path = host + perfildecatalogoRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as PerfilDeCatalogo[])).toPromise();
        return data;
    }
}
