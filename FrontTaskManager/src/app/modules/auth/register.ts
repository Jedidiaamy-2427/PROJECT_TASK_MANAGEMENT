import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrls: ['./login.css']
})

export class Register {
    private authService = inject(AuthService);
    private router = inject(Router);
    form: FormGroup;
    
    constructor(private fb: FormBuilder) {
        this.form = this.fb.group({
            username: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required]
        });
    }

    submit() {
        if (this.form.invalid){
            this.form.markAllAsTouched();
            return; 
        };
    
        const { username, email, password } = this.form.value;

        console.log('FORM DATA', this.form.value);
        const newUser = this.authService.register(username, email, password)
        newUser.subscribe({
            next: (user) => {
                console.log('REGISTER SUCCESS', user);
                this.router.navigate(['/login']);
            },
            error: (err) => {
                console.error('REGISTER ERROR', err);
                this.form.setErrors({ invalidRegister: true });
            }
        }); 
      
    }
}