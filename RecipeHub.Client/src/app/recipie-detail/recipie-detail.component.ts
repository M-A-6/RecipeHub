import { Component, OnInit } from '@angular/core';
import { RecipeService } from 'src/services/Recipe.Service';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormArray } from "@angular/forms";
import * as jspdf from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-recipie-detail',
  templateUrl: './recipie-detail.component.html',
  styleUrls: ['./recipie-detail.component.css']
})
export class RecipieDetailComponent implements OnInit {
  recipeForm: FormGroup;
  recipieDetail:any;
  path: string;
  constructor(private recipeService: RecipeService, private router: Router,private route: ActivatedRoute,private formBuilder: FormBuilder) { 
    this.path='';
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.getRecipieDetail(params["id"]);
    });
    this.path = this.recipeService.getFilePath();   
  }
  exportAsPDF(divId)
  {
      let data = document.getElementById('divRecipeDetails');  
      html2canvas(data).then(canvas => {
      const contentDataURL = canvas.toDataURL('image/png')  
      let pdf = new jspdf.jsPDF('l', 'cm', 'a4'); //Generates PDF in landscape mode
      // let pdf = new jspdf('p', 'cm', 'a4'); Generates PDF in portrait mode
      pdf.addImage(contentDataURL, 'PNG', 0, 0, 29.7, 21.0);  
      pdf.save('Filename.pdf');         
    }); 
  }
  getRecipieDetail(id){
    this.recipeService.getRecipeById(id).subscribe(data =>{
      if(data['code']=== 200){    
        this.recipieDetail=data['response'];
        this.initForm(data['response']);
      }
    })
  }
  defautImage(imgsrc)
  {
    if(imgsrc)
    { 
      return imgsrc;
    }
    else 
    {
      return "no-image-box.png";
    }
  }
  
  initForm(recipieObj) {   
    this.recipeForm = this.formBuilder.group({
      id: [recipieObj.id, Validators.required],
      title: [recipieObj.title, Validators.required],
      recipeStep: recipieObj.recipeStep.split(",").join("\n"),
      recipeIngredient:  recipieObj.recipeIngredient.split(",").join("\n"),//items, //recipieObj.recipeIngredient,
      levels: recipieObj.levels,
      filename1: this.defautImage(recipieObj.filename1),
      filename2: this.defautImage(recipieObj.filename2),
      filename3: this.defautImage(recipieObj.filename3)
    });
  }

  get validations() {
    return this.recipeForm.controls;
  }

  editRecipe(recipieId){    
    let navigationExtras: NavigationExtras = {
      queryParams: {
        "id": recipieId
      }
    };
    this.router.navigate(['/home'], navigationExtras);
  }
}
