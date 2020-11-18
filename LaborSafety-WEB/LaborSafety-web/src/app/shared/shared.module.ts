import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { MatProgressSpinnerModule, MAT_DATE_LOCALE, DateAdapter, MatButtonModule } from '@angular/material';
import { DownloadCsvComponent } from './components/download-csv/download-csv.component';
import {MatIconModule} from '@angular/material/icon'
import { CustomDateAdapter } from './custom-data-adapter';
import { UploadCsvComponent } from './components/upload-csv/upload-csv.component';
import { DataTableColumnDirective } from '@swimlane/ngx-datatable/release/components/columns/column.directive';
import { DataTableColumnHeaderDirective, DataTableColumnCellDirective,
   DataTableColumnCellTreeToggle } from '@swimlane/ngx-datatable/release/components/columns';
import { PopupComponent } from './components/popup/popup.component';
import {MatDialogModule } from '@angular/material';
import {MatTableModule} from '@angular/material/table';

@NgModule({
  imports: [CommonModule, MatProgressSpinnerModule, MatIconModule, MatButtonModule, MatDialogModule, MatTableModule],
  declarations: [ConfirmDialogComponent, DownloadCsvComponent, UploadCsvComponent,
    DataTableColumnDirective, DataTableColumnHeaderDirective, DataTableColumnCellDirective, DataTableColumnCellTreeToggle, PopupComponent],
  entryComponents: [ConfirmDialogComponent, DownloadCsvComponent],
  exports: [ConfirmDialogComponent, DownloadCsvComponent],
  providers: [
    {provide: DateAdapter, useClass: CustomDateAdapter }
  ],
})
export class SharedModule {constructor(private dateAdapter: DateAdapter<Date>) {
  this.dateAdapter.setLocale('pt-br');
}	}
