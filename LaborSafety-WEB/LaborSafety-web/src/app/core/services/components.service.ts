import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class ComponentsService {
    constructor(
        private route: ActivatedRoute,
        private router: Router) {
    }

    getCurrentComponentName() {
        return this.route.component.toString();
    }

    getCurrentComponent() {
        return this.route.component;
    }

    forceInitComponent() {
        // const actRoute: ActivatedRoute = this.getLastChild(this.route);
        // actRoute.url.subscribe(u => {
        //     this.router.navigateByUrl(u.toString());
        // });

        this.router.navigate([this.router.url]);


        // const componentAct = actRoute.component as any;
        // if (componentAct && componentAct.prototype && componentAct.prototype.ngOnInit) {
        //     componentAct.prototype.ngOnInit();
        // }
    }

    getLastChild(actRoute: ActivatedRoute): ActivatedRoute {
        if (actRoute.firstChild) {
            return this.getLastChild(actRoute.firstChild);
        } else {
            return actRoute;
        }
    }
}
