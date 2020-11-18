import { TestBed } from '@angular/core/testing';

import { InvAmbienteService } from './inv-ambiente.service';

describe('InvAmbienteService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InvAmbienteService = TestBed.get(InvAmbienteService);
    expect(service).toBeTruthy();
  });
});
