import { Component, OnInit, signal, Signal } from '@angular/core';
import { ProjectService } from '../../core/services/project';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TitleService } from '../../core/services/titleService';
import { TaskService } from '../../core/services/task';
import { ProjectForm } from './project-form';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, ProjectForm],
  templateUrl: './projects-list.html',
  styleUrls: ['./projects.css']
})
export class ProjectsList implements OnInit {
  projects: ProjectService['projects'];
  projectCount: Signal<number>;
 

  open(id?: any, isEdit?:boolean) {
    this.projectService.ProjectID.set(id)
    this.projectService.isEdit.set(isEdit)
    this.projectService.isOpen.set(true);
  }


  NoProjectTitle = () => this.projectCount() === 0 ? `Aucun projet pour l'instant` : `Total Projects: ${this.projectCount()}`;

  constructor(protected projectService: ProjectService, 
              private router: Router, 
              private titleService: TitleService,
              private taskService: TaskService
            ) {
    this.projects = this.projectService.projects;
    this.projectCount = this.projectService.projectsCount;
  }

  ngOnInit() {
    this.projectService.loadAll().subscribe();
    this.taskService.loadAll().subscribe();
    this.titleService.setTitle(`Projets`);
  }

  create() { 
    this.open(null, false)
    // this.router.navigate(['/projects/new']); 
  }
  edit(id: number) { 
    this.projectService.ProjectID.set(id)
    this.open(id, true);
  }
  remove(id: number) {
    if (!confirm('Delete project?')) return;
    this.projectService.delete(id).subscribe();
  }

  getProjectTasks(id: number) {
    const tasks = this.taskService.getTasksByProject(id);
    return tasks.length > 0 ? `${tasks.length} tâches` : 'Aucune tâche';
  }

  editTask(id:number) {
    this.router.navigate(['/tasks']);
    this.projectService.selectedProjectId.set(id);
  }

  enableBtnTask(id:number) {
    const tasks = this.taskService.getTasksByProject(id);
    return tasks.length > 0
  }

  
}
