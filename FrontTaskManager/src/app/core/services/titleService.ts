import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TitleService {
  private _title = signal<string>(''); // default empty or app name
  title = this._title.asReadonly();

  setTitle(newTitle: string) {
    this._title.set(newTitle);
  }
}
