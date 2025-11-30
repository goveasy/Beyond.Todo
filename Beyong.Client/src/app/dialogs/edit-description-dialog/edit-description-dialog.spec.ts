import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDescriptionDialog } from './edit-description-dialog';

describe('EditDescriptionDialog', () => {
  let component: EditDescriptionDialog;
  let fixture: ComponentFixture<EditDescriptionDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditDescriptionDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDescriptionDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
