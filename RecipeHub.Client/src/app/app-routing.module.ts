import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { RecipiesComponent } from './recipies/recipies.component';
import { HomeComponent } from './home/home.component';
import { RecipieDetailComponent } from './recipie-detail/recipie-detail.component';


const routes: Routes = [
  { path: 'home', component:HomeComponent },
  { path: 'recipies', component:RecipiesComponent },
  { path: 'recipie-detail', component:RecipieDetailComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
