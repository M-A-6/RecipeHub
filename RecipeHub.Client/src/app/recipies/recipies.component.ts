import { Component, OnInit } from '@angular/core';
import { RecipeService } from 'src/services/Recipe.Service';
import { Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-recipies',
  templateUrl: './recipies.component.html',
  styleUrls: ['./recipies.component.css']
})
export class RecipiesComponent implements OnInit {
  recipiesList:any;
  sortLevel:string;
  levelList:any;
  sortByDate: 'ASC';
  filterByLevel:'';
  filename1:'';
  constructor(private recipeService: RecipeService, private router: Router) {
  }

  ngOnInit(): void {
    this.levelList=['Easy','Medium','Hard'];
    this.sortByDate ='ASC';
    this.filterByLevel='';
    this.getAllRecipies();
  }

  getAllRecipies(){     
    this.recipeService.getallRecipies(this.filterByLevel,this.sortByDate).subscribe(data => {
      if(data['code']=== 200){
        this.recipiesList=data['response'];
       }
    })
  }

  getFilterByLevel(e){
    this.filterByLevel = e.target.value;
    this.getAllRecipies();
  }
  getSortByDate(e){
    
    this.sortByDate = e.target.value;
    this.getAllRecipies();
  }
  viewDetail(recipie){
    let navigationExtras: NavigationExtras = {
      queryParams: {
        "id": recipie.id
      }
    };
    this.router.navigate(['/recipie-detail'], navigationExtras);
  }

}
