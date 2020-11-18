import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroInvAtividadeComponent } from './cadastro-inv-atividade.component';

describe('CadastroInvAtividadeComponent', () => {
  let component: CadastroInvAtividadeComponent;
  let fixture: ComponentFixture<CadastroInvAtividadeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CadastroInvAtividadeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CadastroInvAtividadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
