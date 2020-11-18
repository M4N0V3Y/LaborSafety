import { TemplateRef } from '@angular/core';

export class DisplayColumnModel {
    prop: string;
    displayName?: string;
    dateFormat?: string;
    width?: string;
    template?: TemplateRef<any>;
    isTemplateMenu?: boolean;
    isStream?: boolean;
    editable?: boolean;
}
