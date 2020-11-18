import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TipoRisco } from 'src/app/shared/models/tiporisco.model';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const tiporiscoRoute = environment.apiPaths.tiposrisco;

@Injectable({
    providedIn: 'root'
})
export class TipoRiscoService {

    constructor(private httpClient: HttpClient) { }
    async getAll(): Promise<TipoRisco[]> {
        const path = host + tiporiscoRoute.getAll;
        const data = await this.httpClient.get(path).pipe(map((res: any) => res.Data as TipoRisco[])).toPromise();
        return data;
    }
}
