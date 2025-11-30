import {Component, EventEmitter, Input, Output} from '@angular/core';
import {TodoItemDto} from '../../models/todo-item.models';
import {TodoItemComponent} from '../todo-item/todo-item';

@Component({
  selector: 'app-todo-item-list',
  standalone: true,
  imports: [
    TodoItemComponent
  ],
  templateUrl: './todo-item-list.html',
  styleUrl: './todo-item-list.scss',
})
export class TodoItemListComponent {
  @Input() todoItems: TodoItemDto[] | null = [];
  @Input() registeringMap: Record<number, boolean> = {};
  @Input() updatingMap: Record<number, boolean> = {};
  @Input() deletingMap: Record<number, boolean> = {};
  @Input() refreshingMap: Record<number, boolean> = {};

  @Output() addProgression = new EventEmitter<TodoItemDto>();
  @Output() editDescription = new EventEmitter<TodoItemDto>();
  @Output() delete = new EventEmitter<TodoItemDto>();
}
