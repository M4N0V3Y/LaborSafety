import { Injectable } from '@angular/core';
declare const getBrowser: any;

const browserName: string = getBrowser();

export enum BrowserType {
    safari = 0,
    chrome = 1,
    opera = 2,
    firefox = 3,
    IE,
    edge
}

@Injectable({
    providedIn: 'root'
})
export class BrowserSettingsService {
    public GetBrowser(): BrowserType {
        return BrowserType[browserName];
    }
}
