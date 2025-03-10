import { TestBed } from '@angular/core/testing';

import { EditorAlertService } from './editor-alert.service';

describe('EditorAlertService', () => {
  let service: EditorAlertService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditorAlertService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
