import { Component, OnInit, Input, ViewEncapsulation, AfterViewInit } from '@angular/core';
@Component({
  selector: 'message-div-box',
  templateUrl: './message-div-box.component.html',
  styleUrls: ['./message-div-box.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class MessageDivBoxComponent implements OnInit, AfterViewInit {

  @Input('type') type = 'success';
  @Input('message') message = '';

  private readonly backgroundColorSuccess = 'rgba(64, 165, 33, 0.863)';
  private readonly backgroundColorWarning = 'rgba(215, 208, 15, 0.49)';
  private readonly backgroundColorDanger = 'rgba(199, 35, 35, 0.884)';

  private colorFontSuccess = '#fff';
  private colorFontDanger = '#fff';
  private colorFontWarning = '#000';

  backgroundColor = this.backgroundColorSuccess;
  colorFont = this.colorFontSuccess;

  typeIcon = 'check_circle';

  constructor() {
  }

  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    switch (this.type) {
      case 'success':
        this.typeIcon = 'check_circle';
        this.backgroundColor = this.backgroundColorSuccess;
        this.colorFont = this.colorFontSuccess;
        break;
      case 'warning':
        this.typeIcon = 'warning';
        this.backgroundColor = this.backgroundColorWarning;
        this.colorFont = this.colorFontWarning;
        break;
      case 'danger':
        this.typeIcon = 'error';
        this.backgroundColor = this.backgroundColorDanger;
        this.colorFont = this.colorFontDanger;
        break;
    }
  }
}
