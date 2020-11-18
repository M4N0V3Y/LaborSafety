import { ToasterConfig } from 'angular2-toaster';
import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel } from '@angular/router';
import { LoaderService } from './core/services/loader.service';
import { environment } from '../environments/environment';
import {enableProdMode} from '@angular/core';

enableProdMode();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, AfterViewInit {

  title = 'Ternium-Entregas-Coletas';
  public config: ToasterConfig = new ToasterConfig({
    tapToDismiss: true,
    animation: 'fade',
    positionClass: 'toast-top-right',
    showCloseButton: true,
    timeout: 5000
  });

  constructor(
    private router: Router,
    private loaderService: LoaderService
  ) {
    this.loaderService.show();
  }

  ngOnInit(): void {
    if (environment.onlyHttps) {
      if (this.router.url && location.href.trim().toLowerCase().indexOf('https') === -1) {
        this.router.navigate(['browser-error']);
      }
    }
  }

  ngAfterViewInit(): void {
    this.router.events
      .subscribe((event) => {
        if (event instanceof NavigationStart) {
          this.loaderService.show();
        } else if (
          event instanceof NavigationEnd ||
          event instanceof NavigationCancel
        ) {
          this.loaderService.hide();
        }
      });
  }
}
