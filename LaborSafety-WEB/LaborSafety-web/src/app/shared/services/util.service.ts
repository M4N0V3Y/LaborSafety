import { Injectable } from '@angular/core';
import { trigger, AnimationTriggerMetadata, state, transition, animate, style } from '@angular/animations';

@Injectable({
    providedIn: 'root'
})
export class UtilService {
    constructor() { }

    public static getExpandAnimationTrigger(name: string = 'detailExpand',
        heightCollapse: string = '0px', minHeightCollpase: string = '0', heightExpand: string = '*'): AnimationTriggerMetadata {

        if (!name) {
            name = 'detailExpand';
        }

        if (!heightCollapse) {
            heightCollapse = '0px';
        }

        if (!minHeightCollpase) {
            minHeightCollpase = '0';
        }

        if (!heightExpand) {
            heightExpand = '*';
        }

        return trigger(name, [
            state('collapsed', style({ height: heightCollapse, minHeight: minHeightCollpase })),
            state('expanded', style({ height: heightExpand })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]);
    }
}
