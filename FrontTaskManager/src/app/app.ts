import { Component, signal, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Task Management App');
  public auth = inject(AuthService);

  constructor(private router: Router, auth: AuthService) {
    this.auth = auth;
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

}
