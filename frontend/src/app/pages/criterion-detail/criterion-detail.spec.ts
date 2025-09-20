import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CriterionDetail } from './criterion-detail';

describe('CriterionDetail', () => {
  let component: CriterionDetail;
  let fixture: ComponentFixture<CriterionDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CriterionDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CriterionDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
