import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';

export interface User {
  id: number;
  username: string;
  email: string;
} 

@Injectable({ providedIn: 'root' })
export class UserService { 
    private api = `${environment.apiUrl}/Users`;
    public users = signal<User[]>([]);
    
    constructor(private http: HttpClient) {}
    loadAll() {
      return this.http.get<User[]>(this.api).pipe(
      tap(users => this.users.set(users)) // mettre à jour le signal
    );
    }

    getById(id: number) {
      return this.users().find(u => u.id === id) ?? null;
    }
}