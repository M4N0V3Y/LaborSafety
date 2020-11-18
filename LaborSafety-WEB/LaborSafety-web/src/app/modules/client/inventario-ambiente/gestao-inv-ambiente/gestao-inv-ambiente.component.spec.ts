import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestaoInvAmbienteComponent } from './gestao-inv-ambiente.component';

describe('GestaoInvAmbienteComponent', () => {
  let component: GestaoInvAmbienteComponent;
  let fixture: ComponentFixture<GestaoInvAmbienteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestaoInvAmbienteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestaoInvAmbienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
