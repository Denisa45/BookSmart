import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { NgFor } from '@angular/common';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-rentals',
  standalone: true,
  imports: [NgFor,DatePipe],
  templateUrl: './rentals.html',
  styleUrls: ['./rentals.css']
})
export class Rentals implements OnInit {

  rentals: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getRentals().subscribe({
      next: data => this.rentals = data,
      error: err => console.error("Error loading rentals:", err)
    });
  }
}
