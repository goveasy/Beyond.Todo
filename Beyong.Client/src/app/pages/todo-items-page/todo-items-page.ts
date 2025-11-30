import {Component, OnInit} from '@angular/core';
import {
  CreateTodoItemRequest,
  RegisterProgressionRequest,
  TodoItemDto,
  UpdateDescriptionRequest
} from '../../models/todo-item.models';
import {TodoItemsService} from '../../services/todo-items.service';
import {MatDialog} from '@angular/material/dialog';
import {MatSnackBar} from '@angular/material/snack-bar';
import {finalize, take} from 'rxjs';
import {TodoItemDialogComponent, TodoItemDialogData} from '../../dialogs/todo-item-dialog/todo-item-dialog';
import {ProgressionDialogComponent, ProgressionDialogData} from '../../dialogs/progression-dialog/progression-dialog';
import {
  EditDescriptionDialogComponent,
  EditDescriptionDialogData
} from '../../dialogs/edit-description-dialog/edit-description-dialog';
import {ConfirmDialogComponent, ConfirmDialogData} from '../../dialogs/confirm-dialog/confirm-dialog';
import {MatIcon} from '@angular/material/icon';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {TodoItemListComponent} from '../../components/todo-item-list/todo-item-list';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-todo-items-page',
  standalone: true,
  imports: [
    MatIcon,
    MatProgressSpinner,
    TodoItemListComponent,
    MatButton
  ],
  templateUrl: './todo-items-page.html',
  styleUrl: './todo-items-page.scss',
})
export class TodoItemsPageComponent implements OnInit {
  todoItems: TodoItemDto[] = [];
  categories: string[] = [];

  loadingList = false;
  creatingTodo = false;
  registeringMap: Record<number, boolean> = {};
  updatingMap: Record<number, boolean> = {};
  deletingMap: Record<number, boolean> = {};
  refreshingMap: Record<number, boolean> = {};

  constructor(
    private readonly todoItemsService: TodoItemsService,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {
  }

  ngOnInit(): void {
    this.loadCategories();
    this.loadTodoItems();
  }

  loadTodoItems(showLoader = true): void {
    if (showLoader) {
      this.loadingList = true;
    }

    this.todoItemsService.getTodoItems()
      .pipe(
        take(1),
        finalize(() => {
          console.log('Loading TodoItems');
          this.loadingList = false;
        })
      )
      .subscribe({
        next: (items) => {
          this.todoItems = items;
        },
        error: (error) => this.showError(error),
      });
  }

  loadCategories(): void {
    this.todoItemsService.getCategories().subscribe({
      next: (categories) => (this.categories = categories ?? []),
      error: (error) => this.showError(error)
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open<TodoItemDialogComponent, TodoItemDialogData>(TodoItemDialogComponent, {
      width: '520px',
      data: {categories: this.categories}
    });

    dialogRef.afterClosed().subscribe((value?: CreateTodoItemRequest) => {
      if (!value) {
        return;
      }
      this.createTodoItem(value);
    });
  }

  createTodoItem(request: CreateTodoItemRequest): void {
    this.creatingTodo = true;
    this.todoItemsService
      .createTodoItem(request)
      .pipe(
        take(1),
        finalize(() => {
          this.creatingTodo = false
        })
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
      data: {todoTitle: todo.title}
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
        data: {title: todo.title, description: todo.description}
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
      }
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }
      this.deleteTodo(todo.id);
    });
  }

  private registerProgression(id: number, request: RegisterProgressionRequest): void {
    this.registeringMap = {...this.registeringMap, [id]: true};
    this.todoItemsService
      .registerProgression(id, request)
      .pipe(
        take(1),
        finalize(() => {
          this.registeringMap = {...this.registeringMap, [id]: false};
        })
      ).subscribe({
      next: () => {
        this.snackBar.open('Progreso registrado', 'Cerrar', {duration: 2500});
        this.refreshItem(id);
      },
      error: (error) => this.showError(error),
    });
  }

  private updateDescription(id: number, request: UpdateDescriptionRequest): void {
    this.updatingMap = {...this.updatingMap, [id]: true};
    this.todoItemsService
      .updateDescription(id, request)
      .pipe(
        take(1),
        finalize(() => {
          this.updatingMap = {...this.updatingMap, [id]: false};
        })
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
    this.deletingMap = {...this.deletingMap, [id]: true};
    this.todoItemsService
      .removeTodoItem(id)
      .pipe(
        take(1),
        finalize(() => {
          this.deletingMap = {...this.deletingMap, [id]: false};
        })
      )
      .subscribe({
        next: () => {
          this.snackBar.open('Tarea eliminada', 'Cerrar', {duration: 2500});
          this.todoItems = this.todoItems.filter((item) => item.id !== id);
        },
        error: (error) => this.showError(error),
      });
  }

  private refreshItem(id: number): void {
    this.refreshingMap = {...this.refreshingMap, [id]: true};
    this.todoItemsService.getTodoItems()
      .pipe(
        take(1),
        finalize(() => {
          this.refreshingMap = {...this.refreshingMap, [id]: false};
        })
      )
      .subscribe({
        next: (items) => (this.todoItems = items),
        error: (error) => this.showError(error),
      });
  }

  private showError(error: any): void {
    const message = error?.error?.message || error?.message || 'Ha ocurrido un error';
    this.snackBar.open(message, 'Cerrar', {duration: 4000});
    this.loadingList = false;
  }
}
