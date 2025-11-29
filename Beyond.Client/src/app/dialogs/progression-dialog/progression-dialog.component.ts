import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface ProgressionDialogData {
  todoTitle: string;
}

@Component({
  selector: 'app-progression-dialog',
  templateUrl: './progression-dialog.component.html',
  styleUrls: ['./progression-dialog.component.scss']
})
export class ProgressionDialogComponent {
  form = this.fb.group({
    date: [new Date(), Validators.required],
    percent: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly dialogRef: MatDialogRef<ProgressionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProgressionDialogData
  ) {}

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.value);
  }
}
