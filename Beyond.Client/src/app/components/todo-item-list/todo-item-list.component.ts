import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoItemDto } from '../../models/todo-item.models';

@Component({
  selector: 'app-todo-item-list',
  templateUrl: './todo-item-list.component.html',
  styleUrls: ['./todo-item-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
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
