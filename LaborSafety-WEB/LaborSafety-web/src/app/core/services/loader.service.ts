import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoaderService {
    state = false;
    isLoading = new Subject<boolean>();
    show() {
        setTimeout(() => {
            if (this.state) {
                return;
            }

            this.state = true;
            this.isLoading.next(true);
        });
    }
    hide() {
        setTimeout(() => {
            this.state = false;
            this.isLoading.next(false);
        });
    }
}
