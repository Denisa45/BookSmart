import { Component, OnInit } from '@angular/core';
import { NgFor } from '@angular/common';
import { ApiService } from '../../services/api.service';
@Component({
  selector: 'app-customers',
  standalone:true,
  imports: [NgFor],
  templateUrl: './customers.html',
  styleUrls: ['./customers.css'],
})
export class Customers implements OnInit {
      customers:any[]=[];

      constructor(private api:ApiService){}

      ngOnInit(): void {
          this.api.getCustomers().subscribe({
            next:data=>this.customers=data,
            error:err => console.error("error loading customers:",err)
          });
      }
}
