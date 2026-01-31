import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { environment } from '../../environments/env';

@Injectable({ providedIn: 'root' })
export class BookService {
    private baseUrl = '';
    private readonly PAGE_SIZE = 10;

    constructor(private http: HttpClient) {
        this.baseUrl = environment.apiUrl + '/api/Books';
    }

    getAll(pageNumber: number): Observable<Book[]> {
        const params = new HttpParams()
            .set('pageNumber', pageNumber.toString())
            .set('pageSize', this.PAGE_SIZE.toString());

        return this.http.get<Book[]>(this.baseUrl, { params });
    }

    create(book: Book): Observable<Book> {
        return this.http.post<Book>(this.baseUrl, book);
    }

    update(book: Book): Observable<Book> {
        return this.http.put<Book>(this.baseUrl, book);
    }

    delete(isbn: string): Observable<any> {
        return this.http.delete(`${this.baseUrl}/${isbn}`);
    }
}
