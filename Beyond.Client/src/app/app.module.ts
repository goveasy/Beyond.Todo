import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MaterialModule} from "./material/material.module";
import {HttpClientModule} from "@angular/common/http";
import {DatePipe} from "@angular/common";
import {DEFAULT_DIALOG_CONFIG} from "@angular/cdk/dialog";
import { TodoItemsPageComponent } from './pages/todo-items-page/todo-items-page.component';
import { TodoItemComponent } from './components/todo-item/todo-item.component';
import { TodoItemListComponent } from './components/todo-item-list/todo-item-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TodoItemDialogComponent } from './dialogs/todo-item-dialog/todo-item-dialog.component';
import { ProgressionDialogComponent } from './dialogs/progression-dialog/progression-dialog.component';
import { EditDescriptionDialogComponent } from './dialogs/edit-description-dialog/edit-description-dialog.component';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';




@NgModule({
  declarations: [
    AppComponent,
    TodoItemsPageComponent,
    TodoItemComponent,
    TodoItemListComponent,
    TodoItemDialogComponent,
    ProgressionDialogComponent,
    EditDescriptionDialogComponent,
    ConfirmDialogComponent
  ],
  imports: [
    MaterialModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,

  ],
  providers: [DatePipe,
    {provide: DEFAULT_DIALOG_CONFIG, useValue: {hasBackdrop: true}}
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
