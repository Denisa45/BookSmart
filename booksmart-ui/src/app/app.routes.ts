import { Routes } from '@angular/router';
import { Customers } from './pages/customers/customers';
import { Orders } from './pages/orders/orders';
import { Rentals } from './pages/rentals/rentals';
import { Books } from './pages/books/books';
import { LayoutComponent } from './layout/layout';

export const routes: Routes = [
    { path: '', 
        component:LayoutComponent,
        children:[
    {path:'books', component:Books},
    {path:"customers", component:Customers},
    {path:"orders", component:Orders},
    {path:"rentals",component:Rentals},
    {path:"",redirectTo:"/books",pathMatch:'full'}
        ]}
];
