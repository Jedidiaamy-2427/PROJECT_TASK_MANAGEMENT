import { Component, signal, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { TitleService } from './core/services/titleService';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Task Management');
  public auth = inject(AuthService);
  titleService = inject(TitleService);

  constructor(private router: Router, auth: AuthService) {
    this.auth = auth;
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  getUsernameToken() {
    const token = localStorage.getItem('auth_token');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken.unique_name || 'Unknown User';
      } catch (error) {
        console.error('Error decoding token:', error);
        return 'Invalid Token';
      }
    }
  }
}
