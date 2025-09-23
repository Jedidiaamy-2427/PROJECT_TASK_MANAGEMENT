import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService, ResponseError, User } from '../../core/services/auth';
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
    const { username, password } = this.form.value;
    this.auth.login(username, password).subscribe({
      next: (user: User | ResponseError) => {
        this.loginError.set(null);
        if ('token' in user) {
          console.log('LOGIN SUCCESS', user);
          localStorage.setItem('auth_token', user.token ?? '');
          this.auth._token.set(user.token ?? null);
          this.router.navigate(['/projects']);
        } else {
          console.error('LOGIN ERROR', user);
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
