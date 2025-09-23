import { Injectable, inject } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  auth = inject(AuthService);
  router = inject(Router);

  canActivate(): boolean {
    
    if (!localStorage.getItem('auth_token')) {
      this.router.navigate(['/login']);
      return false; 
    }
    return true;
  }
}     
