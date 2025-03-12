import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SkipComponent } from './skip.component';
import { RouterModule } from '@angular/router';

describe('SkipComponent', () => {
  let component: SkipComponent;
  let fixture: ComponentFixture<SkipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkipComponent, RouterModule.forRoot([])]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SkipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
