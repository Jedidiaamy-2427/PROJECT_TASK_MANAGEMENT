import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService, ResponseError, AuthResponse } from '../../core/services/auth';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  form: FormGroup;
  protected readonly title = signal('Task Management App');
  protected loginError = signal<string | null>(null);

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  submit() {
    if (this.form.invalid) return;
    const { username, password } = this.form.value as { username: string; password: string };
    this.auth.login(username, password).subscribe({
      next: (user: AuthResponse | ResponseError) => {
        this.loginError.set(null);
        if ('token' in user) {
          this.router.navigate(['/projects']);
        } else {
          this.form.setErrors({ invalidLogin: true });
        }
      },
      error: (err) => {
        this.loginError.set(err.error?.title);
        this.form.setErrors({ invalidLogin: true });
      }
    });  
  }
}
