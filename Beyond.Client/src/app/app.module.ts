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




@NgModule({
  declarations: [
    AppComponent,
    TodoItemsPageComponent
  ],
  imports: [
    MaterialModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,

  ],
  providers: [DatePipe,
    {provide: DEFAULT_DIALOG_CONFIG, useValue: {hasBackdrop: true}}
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
