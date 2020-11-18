import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListagemUsuariosComponent } from './listagem-usuarios.component';

describe('ListagemUsuariosComponent', () => {
  let component: ListagemUsuariosComponent;
  let fixture: ComponentFixture<ListagemUsuariosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListagemUsuariosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListagemUsuariosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
