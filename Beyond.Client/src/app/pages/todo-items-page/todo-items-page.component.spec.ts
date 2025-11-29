import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoItemsPageComponent } from './todo-items-page.component';

describe('TodoItemsPageComponent', () => {
  let component: TodoItemsPageComponent;
  let fixture: ComponentFixture<TodoItemsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoItemsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoItemsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
