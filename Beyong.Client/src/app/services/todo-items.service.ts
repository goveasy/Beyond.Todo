import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CreateTodoItemRequest,
  RegisterProgressionRequest,
  TodoItemDto,
  UpdateDescriptionRequest
} from '../models/todo-item.models';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TodoItemsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl ?? '';
  private readonly todoItemsUrl = `${this.baseUrl}/TodoItems`;

  getTodoItems(): Observable<TodoItemDto[]> {
    return this.http.get<TodoItemDto[]>(this.todoItemsUrl);
  }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.todoItemsUrl}/categories`);
  }

  createTodoItem(request: CreateTodoItemRequest): Observable<any> {
    return this.http.post(this.todoItemsUrl, request);
  }

  updateDescription(id: number, request: UpdateDescriptionRequest): Observable<any> {
    return this.http.put(`${this.todoItemsUrl}/${id}/description`, request);
  }

  registerProgression(id: number, request: RegisterProgressionRequest): Observable<any> {
    return this.http.post(`${this.todoItemsUrl}/${id}/progressions`, request);
  }

  removeTodoItem(id: number): Observable<any> {
    return this.http.delete(`${this.todoItemsUrl}/${id}`);
  }
}
