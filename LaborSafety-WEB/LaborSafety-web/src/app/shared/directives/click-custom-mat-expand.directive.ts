import { Directive, HostListener } from '@angular/core';
import { MatExpansionPanel } from '@angular/material';

@Directive({
    selector: '[clickCustomMaxExpand]'
})
export class ClickCustomMatExpandDirective {

    private closed?: boolean;
    constructor(private matExpansionPanel: MatExpansionPanel) {
    }
    @HostListener('click', ['$event'])
    public onClick(event: Event): void {

        event.stopPropagation(); // Preventing event

        if (this.closed === undefined) {
            this.closed = true;
        }

        if (!this._isExpansionIndicator(event.target)) {
            if (this.closed !== undefined) {
                if (!this.closed) {
                    this.matExpansionPanel.close();
                } else {
                    this.matExpansionPanel.open();
                }
            }

            this.closed = !this.closed;
        }
    }

    private _isExpansionIndicator(target: EventTarget): boolean {
        const expansionIndicatorClass = 'mat-expansion-indicator';
        const targetCast = target as HTMLElement;
        return (targetCast.classList && targetCast.classList.contains(expansionIndicatorClass));
    }
}
