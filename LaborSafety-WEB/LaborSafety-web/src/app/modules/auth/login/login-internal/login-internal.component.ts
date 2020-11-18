import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { ToasterService } from 'angular2-toaster';
import { appSettings } from 'src/app/configs/app-settings.config';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { PopupComponent } from 'src/app/shared/components/popup/popup.component';


@Component({
  selector: 'app-login-internal',
  templateUrl: './login-internal.component.html',
  styleUrls: ['./login-internal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
// export var teste;
export class LoginInternalComponent implements OnInit {
  msgError = '';
  loginForm: FormGroup;
  isSubmit: boolean;
  hide: boolean;
  returnUrl: string;
  appSettings: any;
  show: boolean;
  isAPR: boolean;
  codAPR: number;
  // tslint:disable-next-line:no-input-rename
  @Input('is-modal') isModal = false;
  // tslint:disable-next-line:no-output-rename
  @Output('logged') logged = new EventEmitter<boolean>();

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private toasterService: ToasterService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.show = false;
    this.appSettings = appSettings;
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || null;
    this.hide = true;
    this.loginForm = new FormGroup({
      id: new FormControl('', [Validators.required]),
      password: new FormControl('', [
        Validators.required
      ]),
      remember: new FormControl()
    });
    this.verificaAprInURL();
  }
  verificaAprInURL() {
    this.route.queryParams.subscribe((queryParams) => {
      if ((queryParams.CodAPR !== undefined)) {
        this.isAPR = true;
        this.codAPR = queryParams.CodAPR;
      } else {
        this.isAPR = false;
      }
    });
  }
  get email() {
    return this.loginForm.get('email');
  }
  get password() {
    return this.loginForm.get('password');
  }
  get remember() {
    return this.loginForm.get('remember');
  }

  onSubmit() {
    this.isSubmit = true;
    if (this.loginForm.invalid) {
      return false;
    } else {
      this.authenticationService
        .login(this.loginForm.value)
        .then((data) => {
          if (!this.returnUrl) {
            if (!this.isModal) {
             /* const returnUrl = this.authenticationService.getUrlRedirect();*/
              const user = this.authenticationService.getUser();
              let perfil;
              const userObject = {
                Usuario8ID: user
              };
              this.authenticationService.getPerfilCom8id(userObject).then((data) => {
                perfil = data[0];
                window.sessionStorage.setItem('perfil', perfil.CodPerfil);
                if (this.isAPR) {
                  const model = {
                    CodAPR: this.codAPR,
                    isLinkImpressao: true,
                  };
                  this.router.navigate(['/client/aprpt/cadastro'],
                  {queryParams: model});
                } else {
                  this.router.navigate(['client/dashboard']);
                }
              });
            } else {
              if (this.logged) {
                this.logged.emit(true);
              }
            }

            return true;
          }
        })
        .catch();
    }
  }

  showPassword() {
    this.show = !this.show;
  }

  openPhonePopup() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.minWidth = '400px';
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.data = { tipoPopup: 3, textoRecebido: 'Favor entrar em contato com o ramal (21)2141-2550' };
    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
  }
}


