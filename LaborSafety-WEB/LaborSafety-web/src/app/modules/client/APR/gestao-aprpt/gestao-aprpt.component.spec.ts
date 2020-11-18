import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestaoAprptComponent } from './gestao-aprpt.component';

describe('GestaoAprptComponent', () => {
  let component: GestaoAprptComponent;
  let fixture: ComponentFixture<GestaoAprptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestaoAprptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestaoAprptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
