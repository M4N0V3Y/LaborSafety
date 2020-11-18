import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestaoInvAtividadeComponent } from './gestao-inv-atividade.component';

describe('GestaoInvAtividadeComponent', () => {
  let component: GestaoInvAtividadeComponent;
  let fixture: ComponentFixture<GestaoInvAtividadeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestaoInvAtividadeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestaoInvAtividadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
