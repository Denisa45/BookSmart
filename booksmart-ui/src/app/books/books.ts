import { Component, OnInit } from '@angular/core';
import { BookService ,Book} from '../services/book.services';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './books.html',
  styleUrls: ['./books.css'],
})
export class Books implements OnInit {
    books: Book[]=[];
    loading=true;

    constructor(private bookService:BookService){}

    ngOnInit(): void {
        this.bookService.getBooks().subscribe({
          next : (data)=>{
            this.books=data;
            this.loading=false;
          },
          error:(err)=>{
            console.log("Api error",err);
            this.loading=false;
          }
        });
    }
}
