import { TestBed } from '@angular/core/testing';

import { TreeviewService } from './treeview.service';

describe('TreeviewService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TreeviewService = TestBed.get(TreeviewService);
    expect(service).toBeTruthy();
  });
});
