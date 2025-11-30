import {Routes} from '@angular/router';
import {TodoItemsPageComponent} from './pages/todo-items-page/todo-items-page';

export const routes: Routes = [
  {
    path: '',
    component: TodoItemsPageComponent,
  },
  {path: '**', redirectTo: ''}
];
