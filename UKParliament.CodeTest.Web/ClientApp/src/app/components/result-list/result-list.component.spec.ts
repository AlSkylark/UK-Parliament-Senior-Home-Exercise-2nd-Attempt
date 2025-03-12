import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { ResultListComponent } from './result-list.component';
import { provideHttpClient } from '@angular/common/http';

describe('ResultListComponent', () => {
  let component: ResultListComponent;
  let fixture: ComponentFixture<ResultListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResultListComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), { provide: 'BASE_URL', useValue: 'http://localhost' }]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ResultListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
