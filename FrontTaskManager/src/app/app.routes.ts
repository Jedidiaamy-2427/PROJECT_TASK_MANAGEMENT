import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./modules/auth/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./modules/auth/register').then(m => m.Register) },
  { path: 'projects', loadComponent: () => import('./modules/projects/projects-list').then(m => m.ProjectsList), canActivate: [AuthGuard] },
  { path: 'projects/new', loadComponent: () => import('./modules/projects/project-form').then(m => m.ProjectForm), canActivate: [AuthGuard] },
  { path: 'projects/:id/edit', loadComponent: () => import('./modules/projects/project-form').then(m => m.ProjectForm), canActivate: [AuthGuard] },
  { path: 'tasks', loadComponent: () => import('./modules/task/task-list').then(m => m.TaskListComponent), canActivate: [AuthGuard] },
//   { path: 'tasks/new', loadComponent: () => import('./tasks/task-form').then(m => m.TaskForm), canActivate: [AuthGuard] },
//   { path: 'tasks/:id/edit', loadComponent: () => import('./tasks/task-form').then(m => m.TaskForm), canActivate: [AuthGuard] },
];
