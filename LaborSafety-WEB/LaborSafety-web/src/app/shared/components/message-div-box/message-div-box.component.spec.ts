import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageDivBoxComponent } from './message-div-box.component';

describe('MessageDivBoxComponent', () => {
  let component: MessageDivBoxComponent;
  let fixture: ComponentFixture<MessageDivBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessageDivBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageDivBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
