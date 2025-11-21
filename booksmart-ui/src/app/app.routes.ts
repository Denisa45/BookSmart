import { Routes } from '@angular/router';
import { Books } from './books/books';

export const routes: Routes = [
    {path:'books', component: Books},
    {path:'', redirectTo:'books', pathMatch:'full'}
];
