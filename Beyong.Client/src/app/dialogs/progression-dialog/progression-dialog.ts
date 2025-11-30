import {Component, Inject} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogActions, MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from '@angular/material/dialog';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {MatButton} from '@angular/material/button';

export interface ProgressionDialogData {
  todoTitle: string;
}

@Component({
  selector: 'app-progression-dialog',
  imports: [
    MatDialogTitle,
    MatDialogContent,
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatInput,
    MatDatepicker,
    MatDialogActions,
    MatButton,
    MatDialogClose
  ],
  templateUrl: './progression-dialog.html',
  styleUrl: './progression-dialog.scss',
})
export class ProgressionDialogComponent {
  form: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly dialogRef: MatDialogRef<ProgressionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProgressionDialogData
  ) {
    this.form = this.fb.group({
      date: [new Date(), Validators.required],
      percent: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.value);
  }
}
