import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TodoItemsPageComponent} from "./pages/todo-items-page/todo-items-page.component";

const routes: Routes = [
  {
    path: '',
    component: TodoItemsPageComponent,

  },
  {path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled',})],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
