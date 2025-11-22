import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({providedIn:'root'})
export class ApiService{
    constructor (private http:HttpClient){}

    getBooks():Observable<any[]>{
        return this.http.get<any[]>('/api/books');
    }
    getCustomers():Observable<any[]>{
        return this.http.get<any[]>('/api/customers');
    }
    getRentals():Observable<any[]>{
        return this.http.get<any[]>('/api/rentals');
    }
    getOrders():Observable<any[]>{
        return this.http.get<any[]>('/api/orders');
    }
}