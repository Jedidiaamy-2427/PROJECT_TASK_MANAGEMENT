import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';

export interface AuthResponse {
  username: string;
  email: string;
  token: string;
  refreshToken: string;
}
export interface ResponseError {
  status: number;
  title: string;
  traceID: string;
  type: string;
}

export interface UserDTO {
    username: string;
    email: string;
    password: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = environment.apiUrl;
  user = signal<AuthResponse | ResponseError | null>(null);
  public _token = signal<string | null>(null);
  public _refresh = signal<string | null>(null);
  
  constructor(private http: HttpClient) {
    const token = localStorage.getItem('auth_token');
    const refresh = localStorage.getItem('refresh_token');
    if (token) this._token.set(token);
    if (refresh) this._refresh.set(refresh);
  }

  login(usernameOrEmail: string, password: string) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/login`, { usernameOrEmail, password })
      .pipe(tap(res => {
        this._token.set(res.token);
        this._refresh.set(res.refreshToken);
        localStorage.setItem('auth_token', res.token);
        localStorage.setItem('refresh_token', res.refreshToken);
        this.user.set({ username: res.username, email: res.email, token: res.token, refreshToken: res.refreshToken });
      }));
  }

  register(username: string, email: string, password: string) {
      return this.http.post<AuthResponse>(`${this.apiUrl}/api/Auth/register`, { username, email, password })
          .pipe(tap(res => {
            this.user.set({ username: res.username, email: res.email, token: res.token, refreshToken: res.refreshToken });
          }));
  }

  logout() {
    this.http.post(`${this.apiUrl}/Auth/logout`, { refreshToken: this._refresh() }).subscribe({
      next: () => {
        this.user.set(null);
        this._token.set(null);
        this._refresh.set(null);
        localStorage.removeItem('auth_token');
        localStorage.removeItem('refresh_token');
      }, 
      error: (err) => {
        console.error('Error logging out:', err);
      }     
    });
  }

  isConnected() {
    const token = localStorage.getItem('auth_token');
  }

  get token() { 
    return this._token();
  }

  get refreshToken() {
    return this._refresh();
  }

  refresh() {
    const rt = this._refresh();
    if (!rt) return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/refresh`, { refreshToken: '' });
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/refresh`, { refreshToken: rt })
      .pipe(tap(res => {
        this._token.set(res.token);
        this._refresh.set(res.refreshToken);
        localStorage.setItem('auth_token', res.token);
        localStorage.setItem('refresh_token', res.refreshToken);
      }));
  }
}
