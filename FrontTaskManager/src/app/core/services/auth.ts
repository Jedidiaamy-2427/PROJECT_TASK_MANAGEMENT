import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

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
  user = signal<User | ResponseError | null>(null);
  public _token = signal<string | null>(null);
  
  constructor(private http: HttpClient) {
    const token = localStorage.getItem('auth_token');
    if (token) {
      this._token.set(token);
    }
  }

  login(usernameOrEmail: string, password: string) {
    return this.http.post<User>('http://localhost:8080/api/Auth/login', { usernameOrEmail, password })
      .pipe(tap(u => {
        this.user.set(u);
      }));
  }

  register(username: string, email: string, password: string) {
      return this.http.post<UserDTO>('http://localhost:8080/api/Auth/register', { username, email, password })
          .pipe(tap(u => {
            this.user.set(u)
          }));
  }

  logout() { 
    this.user.set(null);
    localStorage.removeItem('auth_token');
  }

  isConnected() {
    const token = localStorage.getItem('auth_token');
  }

  get token() { 
    const user = this.user();
    return user && 'token' in user ? (user as User).token : undefined;
  }
}
