import { Component, OnInit } from '@angular/core';
import { BookService, Book } from './book.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.css']
})
export class Appt implements OnInit {
  books: Book[] = [];
  loading = true;

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    this.bookService.getBooks().subscribe({
      next: (data) => {
        this.books = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading books: ', err);
        this.loading = false;
      }
    });
  }
}
