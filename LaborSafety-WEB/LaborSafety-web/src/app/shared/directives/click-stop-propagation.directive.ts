import { Directive, HostListener } from '@angular/core';
import { MatExpansionPanel } from '@angular/material';

@Directive({
    selector: '[clickStopPropagation]'
})
export class ClickStopPropagationDirective {

    constructor(private matExpansionPanel: MatExpansionPanel) {
    }

    @HostListener('click', ['$event'])
    public onClick(event: any): void {
        event.stopPropagation();

        if (this.matExpansionPanel.closed.closed) {
            this.matExpansionPanel.open();
        } else {
            this.matExpansionPanel.close();
        }
    }
}
