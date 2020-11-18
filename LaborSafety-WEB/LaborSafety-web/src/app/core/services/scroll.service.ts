import { Injectable, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ScrollService {
    private scrollEl: ElementRef<HTMLElement>;
    public scrollChange: Subject<boolean> = new Subject<boolean>();

    public setScroll(scroll: ElementRef<HTMLElement>) {
        this.scrollEl = scroll;
        this.scrollChange.next(true);
    }
    public getScroll(): ElementRef<HTMLElement> {
        return this.scrollEl;
    }
}
