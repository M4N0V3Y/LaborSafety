import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],

})
export class ConfirmDialogComponent {

  modalHeader: string;
  modalContent = ``;

  showCancel = false;
  showCloseButton = true;

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (data.showCancel) {
      this.showCancel = data.showCancel;
    }

    if (data.showCloseButton !== undefined) {
      this.showCloseButton = data.showCloseButton;
    }

    if (data.modalContent) {
      this.modalContent = data.modalContent;
    }

    if (data.modalHeader) {
      this.modalHeader = data.modalHeader.trim();
    }
  }

  close(): void {
    this.dialogRef.close(false);
  }

  confirm(): void {
    this.dialogRef.close(true);
  }
}

