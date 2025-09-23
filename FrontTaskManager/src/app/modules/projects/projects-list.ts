import { Component, OnInit, Signal } from '@angular/core';
import { ProjectService } from '../../core/services/project';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './projects-list.html',
  styleUrls: ['./projects.css']
})
export class ProjectsList implements OnInit {
  projects: ProjectService['projects'];
  projectCount: Signal<number>;
  NoProjectTitle = () => this.projectCount() === 0 ? `Aucun projet pour l'instant` : `Total Projects: ${this.projectCount()}`;

  constructor(private projectService: ProjectService, private router: Router) {
    this.projects = this.projectService.projects;
    this.projectCount = this.projectService.projectsCount;
  }

  ngOnInit() {
    this.projectService.loadAll().subscribe();
  }

  create() { this.router.navigate(['/projects/new']); }
  edit(id: number) { this.router.navigate(['/projects', id, 'edit']); }
  remove(id: number) {
    if (!confirm('Delete project?')) return;
    this.projectService.delete(id).subscribe();
  }
}
