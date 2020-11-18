import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapaBloqueioComponent } from './mapa-bloqueio.component';

describe('MapaBloqueioComponent', () => {
  let component: MapaBloqueioComponent;
  let fixture: ComponentFixture<MapaBloqueioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapaBloqueioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapaBloqueioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
