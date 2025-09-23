import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectService } from '../../core/services/project';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './project-form.html',
  styleUrls: ['./projects.css']
})
export class ProjectForm implements OnInit {
  form!: FormGroup;
  isEdit = false;
  id?: number;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });

    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.isEdit = true;
      this.id = id;
      const project = this.projectService.getById(id);
      if (project) this.form.patchValue(project);
    }
  }

  save() {
    if (this.form.invalid) return;
    const dto = this.form.value;
    if (this.isEdit && this.id) {
      this.projectService.update(this.id, dto).subscribe(() => this.router.navigate(['/projects']));
    } else {
      this.projectService.create(dto).subscribe(() => this.router.navigate(['/projects']));
    }
  }
}
