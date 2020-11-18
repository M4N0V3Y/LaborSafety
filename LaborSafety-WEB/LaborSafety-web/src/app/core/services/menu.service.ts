import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class MenuService {
    private lastMenuClick = '';
    private currentMenuClick = '';

    public setCurrentMenuClick(name: string) {
        this.lastMenuClick = this.currentMenuClick;
        this.currentMenuClick = name;
    }
    public getCurrentMenuClick(): string {
        return this.currentMenuClick;
    }
    public getLastMenuClick(): string {
        return this.lastMenuClick;
    }
    public verifyMenuChange(): boolean {
        return this.lastMenuClick.trim().toLowerCase() !== this.currentMenuClick.trim().toLowerCase();
    }
}
