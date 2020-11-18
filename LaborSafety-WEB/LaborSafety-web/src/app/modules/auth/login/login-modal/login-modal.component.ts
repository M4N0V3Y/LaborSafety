import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.scss'],

})
export class LoginModalComponent {

  constructor(
    public dialogRef: MatDialogRef<LoginModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

  }

  loginCallback(event) {
    if (event) {
      this.close();
    }
  }

  close(): void {
    this.dialogRef.close(false);
  }
}

