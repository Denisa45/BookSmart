import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { NgFor } from '@angular/common';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [NgFor,DatePipe],
  templateUrl: './orders.html',
  styleUrls: ['./orders.css']
})
export class Orders implements OnInit {

  orders: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getOrders().subscribe({
      next: data => this.orders = data,
      error: err => console.error("Error loading orders:", err)
    });
  }
}
