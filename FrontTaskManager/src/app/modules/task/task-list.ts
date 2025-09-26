import { Component, signal, computed } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule } from '@angular/forms';
import { Task, TaskService } from '../../core/services/task';
import { CommonModule } from '@angular/common';
import { Project, ProjectService } from '../../core/services/project';
import { AuthService } from '../../core/services/auth';
import { jwtDecode } from 'jwt-decode';
import { User, UserService } from '../../core/services/user';
import { TitleService } from '../../core/services/titleService';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskListComponent {
  // Simuler projets (tu pourras remplacer par API plus tard)
  projects = signal(<Project[]>([]));
  selectedProjectId = signal<number | null>(null);
  
  selectedProject = computed(() =>
    this.projects().find(proj => proj.id === this.selectedProjectId()!) ?? null
  );

  newTaskTitle = signal('');
  newTaskDescription = signal('');
  newDateFin = signal(null);

  tasks = computed(() => {
    return this.taskService.getTasksByProject(this.selectedProjectId());
  });

  selectedStatus = signal<number | null>(0);
  isOpen = signal(false);
  data = signal<any>(null);
  form!: FormGroup;

  open(data?: any) {
    this.data.set(data);
    this.isOpen.set(true);
  }
  constructor(private taskService: TaskService, 
              private projectService: ProjectService,
              private userService: UserService,
              private titleService: TitleService,
              private fb: FormBuilder) {
              }


  ngOnInit() {
    this.loadprojects(); 
    this.taskService.loadAll().subscribe();       
    this.userService.loadAll().subscribe();
    
    console.log('Selected Project:', this.selectedProject());
    this.titleService.setTitle(`Liste des tâches`);

    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      duration: [''],
      projectId: [null],
      userId: [null]
    });
  } 

  getUserName(userId: number | undefined): string {
    if (!userId) return 'Unknown';
    const users = this.userService.users();
    const user = users.find(u => u.id === userId);
    if (user) return user.username;
    return 'Unknown';
  }

  loadprojects() {
    this.projectService.loadAll().subscribe(projects => {
      this.projects.set(projects);

      if(projects.length > 0 && this.projectService.selectedProjectId()) {
        this.selectedProjectId.set(this.projectService.selectedProjectId())
      }
      
      if (projects.length > 0 && !this.selectedProjectId()) {
        this.selectedProjectId.set(projects[0].id);
      }
    });
  }

  addTask() {
    if (!this.newTaskTitle().trim()) return;
    this.taskService.addTask({
      title: this.newTaskTitle(),
      description: this.newTaskDescription(), 
      duration: this.newDateFin(),
      projectId: this.selectedProjectId()!
    }).subscribe({
      next: (resp) => {
        console.log('Task added', resp);

        this.newTaskTitle.set('');
        this.newTaskDescription.set('');
        this.newDateFin.set(null);
      },
      error: (err) => {
          console.error('Error adding task', err);
      }
    });
  
  }

  deleteTask(id: number) {
    this.taskService.removeTask(id).pipe().subscribe({
      next: () => {
        console.log('Task deleted');
      },
      error: (err) => {
        console.error('Error deleting task', err);
      }
    }); 
  }

  EditTask(task:Task) { 
    console.log('Editing task:', task);
    this.selectedStatus.set(task.isCompleted);
    this.form.patchValue({
      title: task.title,
      description: task.description,
      duration: task.duration?.toString().split('T')[0],
      projectId: task.projectId,
      userId: task.userId
    });
    this.open(task);
  }

  isLate(date: string | Date | null, isCompleted:number): boolean {
    if (!date) return false;
    return (new Date(date).getTime() < Date.now() && isCompleted !== 2);
  }

  changeStatus() {
    console.log('✅ Nouveau statut sélectionné :', this.selectedStatus());
    this.taskService.updateStatus(this.data().id, this.selectedStatus()!).subscribe({
      next: () => {
        console.log('Status updated');
      },
      error: (err) => {
        console.error('Error updating status', err);
      }
    });
    this.close();
  }


  save() {
    if (this.form.valid) {
      const updated = this.form.value;
      this.taskService.updateTask(this.data().id, updated).subscribe({
        next: () => {
          console.log('Task updated');
        },
        error: (err) => {
          console.error('Error updating task', err);
        }
      });
      this.close();
    }
  }

  close() {
    this.isOpen.set(false);
    this.data.set(null);
  }



}
