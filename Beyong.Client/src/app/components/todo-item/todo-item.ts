import {Component, EventEmitter, Input, Output} from '@angular/core';
import {TodoItemDto} from '../../models/todo-item.models';
import {MatCard} from '@angular/material/card';
import {MatAccordion, MatExpansionPanel, MatExpansionPanelHeader} from '@angular/material/expansion';
import {MatProgressBar} from '@angular/material/progress-bar';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {MatIcon} from '@angular/material/icon';
import {MatIconButton} from '@angular/material/button';
import {DatePipe, NgIf} from '@angular/common';

@Component({
  selector: 'app-todo-item',
  imports: [
    MatCard,
    MatAccordion,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatProgressBar,
    MatProgressSpinner,
    MatIcon,
    MatIconButton,
    NgIf,
    DatePipe
  ],
  templateUrl: './todo-item.html',
  styleUrl: './todo-item.scss',
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
