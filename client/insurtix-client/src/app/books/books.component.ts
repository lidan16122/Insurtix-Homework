import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Book } from '../models/book.model';
import { BookService } from '../services/book.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css'],
})
export class BooksComponent {
  private readonly PAGE_SIZE = 10;

  books = signal<Book[]>([]);
  page = signal(1);
  showForm = signal(false);
  editMode = signal(false);
  currentIsbn = signal<string | null>(null);

  form!: any;

  pagedBooks = computed(() => {
    const all = this.books();
    const p = this.page();
    const start = (p - 1) * this.PAGE_SIZE;
    return all.slice(start, start + this.PAGE_SIZE);
  });

  totalPages = computed(() => Math.max(1, Math.ceil(this.books().length / this.PAGE_SIZE)));

  constructor(private bookService: BookService, private fb: FormBuilder) {
    this.form = this.fb.nonNullable.group({
      isbn: ['', [Validators.required]],
      title: ['', [Validators.required]],
      category: [''],
      cover: [''],
      lang: ['en'],
      authors: [''],
      year: [new Date().getFullYear(), [Validators.min(0)]],
      price: [0, [Validators.min(0)]],
    });

    this.load();
  }

  load() {
    this.bookService.getAll(this.page()).subscribe({
      next: (res) => {
        this.books.set(res || []);
        this.page.set(1);
      },
      error: (err) => console.error('Failed to load books', err),
    });
  }

  openAdd() {
    this.editMode.set(false);
    this.currentIsbn.set(null);
    this.form.reset({ isbn: '', title: '', category: '', cover: '', lang: 'en', authors: '', year: new Date().getFullYear(), price: 0 });
    this.showForm.set(true);
  }

  openEdit(b: Book) {
    this.editMode.set(true);
    this.currentIsbn.set(b.isbn);
    this.form.setValue({
      isbn: b.isbn,
      title: b.title,
      category: b.category,
      cover: b.cover ?? '',
      lang: b.lang ?? 'en',
      authors: (b.authors || []).join(', '),
      year: b.year,
      price: b.price,
    });
    this.showForm.set(true);
  }



  cancel() {
    this.showForm.set(false);
  }

  save() {
    const value = this.form.getRawValue ? this.form.getRawValue() : this.form.value;
    const book: Book = {
      isbn: (value.isbn || '').trim(),
      title: (value.title || '').trim(),
      category: (value.category || '').trim(),
      cover: value.cover || undefined,
      lang: value.lang || 'en',
      authors: (value.authors || '')
        .split(',')
        .map((a: string) => a.trim())
        .filter((a: string) => a.length > 0),
      year: Number(value.year) || 0,
      price: Number(value.price) || 0,
    };

    if (!book.isbn) {
      alert('ISBN is required');
      return;
    }

    if (this.editMode()) {
      this.bookService.update(book).subscribe({
        next: () => {
          this.showForm.set(false);
          this.load();
        },
        error: (err) => alert('Update failed: ' + err.message || err),
      });
    } else {
      this.bookService.create(book).subscribe({
        next: () => {
          this.showForm.set(false);
          this.load();
        },
        error: (err) => alert('Create failed: ' + err.message || err),
      });
    }
  }

  confirmDelete(isbn: string) {
    if (!confirm('Delete book ' + isbn + '?')) return;
    this.bookService.delete(isbn).subscribe({
      next: () => this.load(),
      error: (err) => alert('Delete failed: ' + err.message || err),
    });
  }

  goToPage(n: number) {
    const tp = this.totalPages();
    if (n < 1) n = 1;
    if (n > tp) n = tp;
    this.page.set(n);
  }
}
