import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
const host = environment.host;
const mapaRoute = environment.apiPaths.apr;

@Injectable({
    providedIn: 'root'
})
export class MapaService {

    constructor(private httpClient: HttpClient) { }
    async sendGetOrdens(ordens): Promise<any> {
        const path = host + mapaRoute.getMapaBase64(ordens);
        const data = await this.httpClient.post(path, ordens).pipe(map((res: any) => res.Data as any )).toPromise();
        return data;
    }

}
