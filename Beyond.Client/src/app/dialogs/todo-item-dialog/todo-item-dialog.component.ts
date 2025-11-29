import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface TodoItemDialogData {
  categories: string[];
}

@Component({
  selector: 'app-todo-item-dialog',
  templateUrl: './todo-item-dialog.component.html',
  styleUrls: ['./todo-item-dialog.component.scss']
})
export class TodoItemDialogComponent {
  form = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    category: ['', Validators.required]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly dialogRef: MatDialogRef<TodoItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TodoItemDialogData
  ) {}

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.value);
  }
}
