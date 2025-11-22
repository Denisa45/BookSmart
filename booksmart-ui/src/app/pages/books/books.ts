import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { NgFor } from '@angular/common';
@Component({
  selector: 'app-books',
  standalone:true,
  imports: [NgFor],
  templateUrl: './books.html',
  styleUrls: ['./books.css'],
})
export class Books implements OnInit{
    books:any[]=[];

    constructor(private api:ApiService){}

    ngOnInit(): void {
        this.api.getBooks().subscribe({
          next:data => this.books=data,
          error:err=> console.error('error loading books:',err)
        });
    }
    
}
