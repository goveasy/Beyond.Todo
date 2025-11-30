import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoItemsPage } from './todo-items-page';

describe('TodoItemsPage', () => {
  let component: TodoItemsPage;
  let fixture: ComponentFixture<TodoItemsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoItemsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoItemsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
