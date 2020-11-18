import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportacaoComponent } from './importacao.component';

describe('ImportacaoComponent', () => {
  let component: ImportacaoComponent;
  let fixture: ComponentFixture<ImportacaoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportacaoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportacaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
