import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressionDialog } from './progression-dialog';

describe('ProgressionDialog', () => {
  let component: ProgressionDialog;
  let fixture: ComponentFixture<ProgressionDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgressionDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProgressionDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
