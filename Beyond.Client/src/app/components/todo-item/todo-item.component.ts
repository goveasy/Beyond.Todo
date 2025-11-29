import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoItemDto } from '../../models/todo-item.models';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TodoItemComponent {
  @Input() todo!: TodoItemDto;
  @Input() registeringProgression = false;
  @Input() updatingDescription = false;
  @Input() deleting = false;
  @Input() refreshing = false;

  @Output() addProgression = new EventEmitter<TodoItemDto>();
  @Output() editDescription = new EventEmitter<TodoItemDto>();
  @Output() delete = new EventEmitter<TodoItemDto>();

  get progressLabel(): string {
    const percent = Math.min(100, this.todo?.cumulativePercent ?? 0);
    return `${percent}%`;
  }
}
