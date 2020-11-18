import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { Subject } from 'rxjs';
import { LoaderService } from 'src/app/core/services/loader.service';

@Component({
  selector: 'app-loader',
  template: `
    <div *ngIf="isLoading | async" class="overlay">
      <mat-progress-spinner
        class="spinner"
        [color]="color"
        [mode]="mode"
        [value]="value"
      >
      </mat-progress-spinner>
    </div>
  `,
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  isLoading: Subject<boolean> = this.loaderService.isLoading;
  constructor(private loaderService: LoaderService) {}

  ngOnInit() {
  }
}
