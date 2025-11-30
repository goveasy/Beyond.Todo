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
import {MatButton} from '@angular/material/button';

export interface EditDescriptionDialogData {
  title: string;
  description: string;
}

@Component({
  selector: 'app-edit-description-dialog',
  imports: [
    MatDialogTitle,
    MatDialogContent,
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatDialogActions,
    MatButton,
    MatDialogClose,
    MatInput
  ],
  templateUrl: './edit-description-dialog.html',
  styleUrl: './edit-description-dialog.scss',
})
export class EditDescriptionDialogComponent {
  form: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly dialogRef: MatDialogRef<EditDescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditDescriptionDialogData
  ) {
    this.form = this.fb.group({
      newDescription: [this.data.description, Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.value);
  }
}
