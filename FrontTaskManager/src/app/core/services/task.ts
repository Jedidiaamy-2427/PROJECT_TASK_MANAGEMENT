import { Injectable, signal } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from './auth';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';

export interface Task {
  id:number;
  title: string;
  description: string;
  duration: Date | null;
  createdAt: Date;
  isCompleted: number;
  projectId: number;
  userId?: number;
}

@Injectable({ providedIn: 'root' })
export class TaskService {
  private api = `${environment.apiUrl}/TaskItems`;
  private _tasks = signal<Task[]>([]);
  tasks = this._tasks.asReadonly();

  constructor(private authService: AuthService, 
              private http: HttpClient
  ) {}

  addTask(task: { title: string; description: string; duration: Date | null; projectId: number }) {
    const parseTask = { 
      ...task,  
      userId: (jwtDecode(this.authService.token || '') as any).sub 
    };
    return this.http.post<Task>(this.api, parseTask).pipe(
      tap(() => this.loadAll().subscribe())
    );
  }

  loadAll() { 
    return this.http.get<Task[]>(this.api).pipe(
      tap(list => {
        const sorted = [...list].sort(
        (a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
      );
      this._tasks.set(sorted);
      })
    );
  }

  updateTask(id: number, dto: { title: string; description: string; duration: Date | null; projectId: number; userId?: number }) {
    return this.http.put<Task>(`${this.api}/${id}`, dto).pipe(
    tap(() => this.loadAll().subscribe())
  );
  }

  removeTask(id: number) {
    return this.http.delete<void>(`${this.api}/${id}`).pipe(
      tap(() => this.loadAll().subscribe())
    );
  }

  updateStatus(id: number, status: Task['isCompleted']) {
    return this.http.put<void>(`${this.api}/${id}/status?isCompleted=${status}`, {}).pipe(
      tap(() => this.loadAll().subscribe())
    );
  }

  getTasksByProject(projectId: number | null) {
    console.log('Filtering tasks for projectId:', this._tasks());
    return this._tasks().filter(t => t.projectId === projectId);
  }
}
