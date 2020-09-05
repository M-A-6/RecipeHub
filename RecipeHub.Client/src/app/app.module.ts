import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RecipeService } from 'src/services/Recipe.Service';
import { HttpClientModule } from '@angular/common/http';
import { RecipiesComponent } from './recipies/recipies.component';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { RecipieDetailComponent } from './recipie-detail/recipie-detail.component';
import {NgxPaginationModule} from 'ngx-pagination';

@NgModule({
  declarations: [
    AppComponent,
    RecipiesComponent,
    HomeComponent,
    RecipieDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbModule,
    FormsModule,
    NgxPaginationModule      
  ],
  providers: [RecipeService],
  bootstrap: [AppComponent]
})
export class AppModule { }

