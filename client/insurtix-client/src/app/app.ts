import { Component, signal } from '@angular/core';
import { BooksComponent } from './books/books.component';

@Component({
  selector: 'app-root',
  imports: [BooksComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('insurtix-client');
}
