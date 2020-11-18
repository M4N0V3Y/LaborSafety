import { TestBed } from '@angular/core/testing';

import { AprServiceService } from './apr-service.service';

describe('AprServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AprServiceService = TestBed.get(AprServiceService);
    expect(service).toBeTruthy();
  });
});
