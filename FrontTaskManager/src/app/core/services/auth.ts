import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';

export interface User {
  username: string;
  email: string;
  token?: string;
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
  user = signal<User | ResponseError | null>(null);
  public _token = signal<string | null>(null);
  
  constructor(private http: HttpClient) {
    const token = localStorage.getItem('auth_token');
    if (token) {
      this._token.set(token);
    }
  }

  login(usernameOrEmail: string, password: string) {
    return this.http.post<User>(`${this.apiUrl}/Auth/login`, { usernameOrEmail, password })
      .pipe(tap(u => {
        this.user.set(u);
      }));
  }

  register(username: string, email: string, password: string) {
      return this.http.post<UserDTO>(`${this.apiUrl}/api/Auth/register`, { username, email, password })
          .pipe(tap(u => {
            this.user.set(u)
          }));
  }

  logout() { 
    this.user.set(null);
    localStorage.removeItem('auth_token');
    this._token.set(null);
  }

  isConnected() {
    const token = localStorage.getItem('auth_token');
  }

  get token() { 
    const user = this.user();
    return user && 'token' in user ? (user as User).token : this._token();
  }
}
