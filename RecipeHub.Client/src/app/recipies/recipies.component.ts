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
  sortByDate:any = 'ASC';
  filterByLevel: any = '';
  filename1:any = '';
  pageSize: number  = 20;
  totalRecords: number = 0;
  page:number = 1;
  constructor(private recipeService: RecipeService, private router: Router) {    
    this.page=1;
    this.pageSize =20;
  }

  ngOnInit(): void {
    this.levelList=['Easy','Medium','Hard'];
    this.sortByDate ='ASC';
    this.filterByLevel='';
    this.getAllRecipies();
  }

  getAllRecipies(){     
    this.recipeService.getallRecipies(this.filterByLevel,this.sortByDate,this.page,this.pageSize).subscribe(data => {
      if(data['code']=== 200){
        this.recipiesList = data['response'];
        this.totalRecords = data['count'];
       }
    });
  }

  getFilterByLevel(e){
    this.filterByLevel = e.target.value;
    this.getAllRecipies();
  }
  getSortByDate(e){    
    this.sortByDate = e.target.value;
    this.getAllRecipies();
  }
  getPageSize(e){        
    this.pageSize = e.target.value;
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

  get numberOfPages()
  {
    return Math.ceil(this.totalRecords/this.pageSize);
  }

  pageChanged(e)
  {   
   this.page=e;
   this.getAllRecipies();
  }
}
