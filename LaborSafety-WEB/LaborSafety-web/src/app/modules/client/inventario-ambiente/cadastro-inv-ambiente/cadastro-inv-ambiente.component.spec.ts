import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroInvAmbienteComponent } from './cadastro-inv-ambiente.component';

describe('CadastroInvAmbienteComponent', () => {
  let component: CadastroInvAmbienteComponent;
  let fixture: ComponentFixture<CadastroInvAmbienteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CadastroInvAmbienteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CadastroInvAmbienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
