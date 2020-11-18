import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImpressaoMassaComponent } from './impressao-massa.component';

describe('ImpressaoMassaComponent', () => {
  let component: ImpressaoMassaComponent;
  let fixture: ComponentFixture<ImpressaoMassaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImpressaoMassaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImpressaoMassaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
