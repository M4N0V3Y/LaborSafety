import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopUpPessoasComponent } from './pop-up-pessoas.component';

describe('PopUpPessoasComponent', () => {
  let component: PopUpPessoasComponent;
  let fixture: ComponentFixture<PopUpPessoasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PopUpPessoasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopUpPessoasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
