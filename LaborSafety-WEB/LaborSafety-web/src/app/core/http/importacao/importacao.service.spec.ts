import { TestBed } from '@angular/core/testing';

import { ImportacaoService } from './importacao.service';

describe('ImportacaoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ImportacaoService = TestBed.get(ImportacaoService);
    expect(service).toBeTruthy();
  });
});
