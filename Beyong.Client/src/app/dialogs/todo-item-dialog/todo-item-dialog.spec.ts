import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoItemDialog } from './todo-item-dialog';

describe('TodoItemDialog', () => {
  let component: TodoItemDialog;
  let fixture: ComponentFixture<TodoItemDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoItemDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoItemDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
