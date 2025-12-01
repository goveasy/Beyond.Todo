import { Component, OnInit, computed, inject, signal } from '@angular/core';
import {
  CreateTodoItemRequest,
  RegisterProgressionRequest,
  TodoItemDto,
  UpdateDescriptionRequest
} from '../../models/todo-item.models';
import { TodoItemsService } from '../../services/todo-items.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize, take } from 'rxjs';
import { TodoItemDialogComponent, TodoItemDialogData } from '../../dialogs/todo-item-dialog/todo-item-dialog';
import { ProgressionDialogComponent, ProgressionDialogData } from '../../dialogs/progression-dialog/progression-dialog';
import {
  EditDescriptionDialogComponent,
  EditDescriptionDialogData
} from '../../dialogs/edit-description-dialog/edit-description-dialog';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../dialogs/confirm-dialog/confirm-dialog';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { TodoItemListComponent } from '../../components/todo-item-list/todo-item-list';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-todo-items-page',
  standalone: true,
  imports: [
    MatIcon,
    MatProgressSpinner,
    TodoItemListComponent,
    MatButton,
    MatDivider,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './todo-items-page.html',
  styleUrl: './todo-items-page.scss',
})
export class TodoItemsPageComponent implements OnInit {
  private readonly todoItemsService = inject(TodoItemsService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly todoItems = signal<TodoItemDto[]>([]);
  readonly categories = signal<string[]>([]);

  readonly loadingList = signal(false);
  readonly creatingTodo = signal(false);
  readonly registeringMap = signal<Record<number, boolean>>({});
  readonly updatingMap = signal<Record<number, boolean>>({});
  readonly deletingMap = signal<Record<number, boolean>>({});
  readonly refreshingMap = signal<Record<number, boolean>>({});

  readonly hasItems = computed(() => this.todoItems().length > 0);

  ngOnInit(): void {
    this.loadCategories();
    this.loadTodoItems();
  }

  loadTodoItems(showLoader = true): void {
    if (showLoader) {
      this.loadingList.set(true);
    }

    this.todoItemsService
      .getTodoItems()
      .pipe(
        take(1),
        finalize(() => this.loadingList.set(false))
      )
      .subscribe({
        next: (items) => this.todoItems.set(items ?? []),
        error: (error) => this.showError(error),
      });
  }

  loadCategories(): void {
    this.todoItemsService.getCategories().subscribe({
      next: (categories) => this.categories.set(categories ?? []),
      error: (error) => this.showError(error)
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open<TodoItemDialogComponent, TodoItemDialogData>(TodoItemDialogComponent, {
      width: '520px',
      data: {categories: this.categories()},
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe((value?: CreateTodoItemRequest) => {
      if (!value) {
        return;
      }
      this.createTodoItem(value);
    });
  }

  createTodoItem(request: CreateTodoItemRequest): void {
    this.creatingTodo.set(true);
    this.todoItemsService
      .createTodoItem(request)
      .pipe(
        take(1),
        finalize(() => this.creatingTodo.set(false))
      )
      .subscribe({
        next: () => {
          this.snackBar.open('Tarea creada', 'Cerrar', {duration: 2500});
          this.loadTodoItems(false);
        },
        error: (error) => this.showError(error),
      });
  }

  onRegisterProgression(todo: TodoItemDto): void {
    const dialogRef = this.dialog.open<ProgressionDialogComponent, ProgressionDialogData>(ProgressionDialogComponent, {
      width: '420px',
      data: {todoTitle: todo.title},
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe((result?: RegisterProgressionRequest) => {
      if (!result) {
        return;
      }
      this.registerProgression(todo.id, result);
    });
  }

  onEditDescription(todo: TodoItemDto): void {
    const dialogRef = this.dialog.open<EditDescriptionDialogComponent, EditDescriptionDialogData>(
      EditDescriptionDialogComponent,
      {
        width: '420px',
        data: {title: todo.title, description: todo.description},
        autoFocus: false
      }
    );

    dialogRef.afterClosed().subscribe((result?: UpdateDescriptionRequest) => {
      if (!result) {
        return;
      }
      this.updateDescription(todo.id, result);
    });
  }

  onDelete(todo: TodoItemDto): void {
    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmDialogData>(ConfirmDialogComponent, {
      width: '360px',
      data: {
        title: 'Eliminar tarea',
        message: `¿Deseas eliminar "${todo.title}"?`,
        confirmText: 'Eliminar'
      },
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }
      this.deleteTodo(todo.id);
    });
  }

  private registerProgression(id: number, request: RegisterProgressionRequest): void {
    this.registeringMap.update((map) => ({...map, [id]: true}));
    this.todoItemsService
      .registerProgression(id, request)
      .pipe(
        take(1),
        finalize(() => this.registeringMap.update((map) => ({...map, [id]: false})))
      ).subscribe({
      next: () => {
        this.snackBar.open('Progreso registrado', 'Cerrar', {duration: 2500});
        this.refreshItem(id);
      },
      error: (error) => this.showError(error),
    });
  }

  private updateDescription(id: number, request: UpdateDescriptionRequest): void {
    this.updatingMap.update((map) => ({...map, [id]: true}));
    this.todoItemsService
      .updateDescription(id, request)
      .pipe(
        take(1),
        finalize(() => this.updatingMap.update((map) => ({...map, [id]: false})))
      )
      .subscribe({
        next: () => {
          this.snackBar.open('Descripción actualizada', 'Cerrar', {duration: 2500});
          this.refreshItem(id);
        },
        error: (error) => this.showError(error),
      });
  }

  private deleteTodo(id: number): void {
    this.deletingMap.update((map) => ({...map, [id]: true}));
    this.todoItemsService
      .removeTodoItem(id)
      .pipe(
        take(1),
        finalize(() => this.deletingMap.update((map) => ({...map, [id]: false})))
      )
      .subscribe({
        next: () => {
          this.snackBar.open('Tarea eliminada', 'Cerrar', {duration: 2500});
          this.todoItems.update((items) => items.filter((item) => item.id !== id));
        },
        error: (error) => this.showError(error),
      });
  }

  private refreshItem(id: number): void {
    this.refreshingMap.update((map) => ({...map, [id]: true}));
    this.todoItemsService.getTodoItems()
      .pipe(
        take(1),
        finalize(() => this.refreshingMap.update((map) => ({...map, [id]: false})))
      )
      .subscribe({
        next: (items) => this.todoItems.set(items ?? []),
        error: (error) => this.showError(error),
      });
  }

  private showError(error: any): void {
    const message = error?.error?.message || error?.message || 'Ha ocurrido un error';
    this.snackBar.open(message, 'Cerrar', {duration: 4000});
    this.loadingList.set(false);
  }
}
