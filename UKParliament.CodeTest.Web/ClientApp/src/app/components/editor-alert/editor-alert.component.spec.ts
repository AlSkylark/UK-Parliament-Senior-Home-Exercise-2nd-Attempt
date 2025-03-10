import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditorAlertComponent } from './editor-alert.component';

describe('EditorAlertComponent', () => {
  let component: EditorAlertComponent;
  let fixture: ComponentFixture<EditorAlertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditorAlertComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditorAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
