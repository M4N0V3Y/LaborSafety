import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroAprptComponent } from './cadastro-aprpt.component';

describe('CadastroAprptComponent', () => {
  let component: CadastroAprptComponent;
  let fixture: ComponentFixture<CadastroAprptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CadastroAprptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CadastroAprptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
