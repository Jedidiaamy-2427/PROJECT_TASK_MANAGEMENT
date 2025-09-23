import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

export interface Project {
  id: number;
  name: string;
  description?: string;
}

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private api = 'http://localhost:8080/api/Projects';
  projects = signal<Project[]>([]);
  projectsCount = computed(() => this.projects().length);

  constructor(private http: HttpClient) {}

  loadAll() {
    return this.http.get<Project[]>(this.api).pipe(
      tap(list => this.projects.set(list))
    );
  }

  create(dto: { name: string; description?: string }) {
    return this.http.post<Project>(this.api, dto).pipe(
      tap(p => this.projects.update(arr => [...arr, p]))
    );
  }

  update(id: number, dto: { name: string; description?: string }) {
    return this.http.put<void>(`${this.api}/${id}`, dto).pipe(
      tap(() =>
        this.projects.update(arr => arr.map(a => a.id === id ? { ...a, ...dto } : a))
      )
    );
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.api}/${id}`).pipe(
      tap(() => this.projects.update(arr => arr.filter(p => p.id !== id)))
    );
  }

  getById(id: number) {
    return this.projects().find(p => p.id === id) ?? null;
  }
}
