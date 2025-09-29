import { Component, Input, OnInit, signal } from '@angular/core';
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
  @Input() isEdit? = false;
  @Input() id?: number | null;

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

    //const id = Number(this.route.snapshot.paramMap.get('id'));

    if (this.id) {
      this.isEdit = true;
      const project = this.projectService.getById(this.id);
      if (project) this.form.patchValue(project);
    }
  }

  save() {
    if (this.form.invalid) return;
    const dto = this.form.value;
    if (this.isEdit && this.id) {
      this.projectService.update(this.id, dto).subscribe(() => this.close());
    } else {
      this.projectService.create(dto).subscribe(() => this.close());
      
    }
  }

  close() {
    this.projectService.isOpen.set(false)
    this.projectService.ProjectID.set(null)
    this.projectService.isEdit.set(false)
  }
}
