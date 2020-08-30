import { Component, OnInit } from '@angular/core';
import { RecipeService } from 'src/services/Recipe.Service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from "@angular/forms";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit { 
  recipieDetail:any;
  recipeForm : FormGroup;
  levelList: any;
  fileToUpload1: File = null;
  fileToUpload2: File = null;
  fileToUpload3: File = null;
  recipeId: any;  
  constructor(private recipeService: RecipeService, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
    this.levelList = ['Easy', 'Medium', 'Hard'];
    this.route.queryParams.subscribe(params => {
      this.recipeId = params["id"];
    });         
  }

  ngOnInit() {   
    if(this.recipeId>0)
    {
       this.recipeForm = new FormGroup({
        id :  new FormControl(0),
        title: new FormControl(),
        levels: new FormControl(),
        recipeIngredient: new FormArray([ ]),
        recipeStep: new FormArray([ ]),    
      });       
      this.getRecipieDetail(this.recipeId);
    }
    else{       
      this.recipeForm  =this.fb.group({
        id: ['', Validators.required],
        title: ['', Validators.required],
        recipeIngredient: this.fb.array([this.fb.group({ingredientName: '' })]),
        recipeStep: this.fb.array([this.fb.group({ stepName: '' })]),
        levels: ['',Validators.required]   
      });    
    }    
  }

  addnewIngredient(id,ingredientname,recipeid) : FormGroup
  {
    return  new FormGroup({
      id:  new FormControl(id),
      ingredientName: new FormControl(ingredientname),
      recipeId : new FormControl(recipeid)      
     });
  }

  addnewStep(id,stepname,recipeid) : FormGroup
  {
    return  new FormGroup({
      id:  new FormControl(id),
      stepName: new FormControl(stepname),
      recipeId : new FormControl(recipeid)      
     });
  }
  
  onPatch(): void {
    this.recipeForm.patchValue({ 
      title: this.recipieDetail.title,
      levels: this.recipieDetail.levels,
      id: this.recipieDetail.id  
    });
    this.recipieDetail.recipeIngredient.forEach(element => {
      (<FormArray>this.recipeForm.get('recipeIngredient'))
      .push(this.addnewIngredient(element.id,element.ingredientName,element.recipeId));
    });
    this.recipieDetail.recipeStep.forEach(element => {
      (<FormArray>this.recipeForm.get('recipeStep'))
      .push(this.addnewStep(element.id,element.stepName,element.recipeId));
      });    
  }
  createUpdateRecipie() {    
   const formData: FormData = new FormData();
    formData.append('File1', this.fileToUpload1);
    formData.append('File2', this.fileToUpload2);
    formData.append('File3', this.fileToUpload3);
    let request: any = {};
    for (let key in this.recipeForm.controls) {
      request[key] = this.recipeForm.controls[key].value;
    };
    formData.append('id', this.recipeId);
    formData.append('levels', request.levels);
    formData.append('title', request.title);
    formData.append('RecipeStep', JSON.stringify(request.recipeStep));
    formData.append('RecipeIngredient',JSON.stringify(request.recipeIngredient));
    
    if(this.recipeId>0)
    {
      this.recipeService.updateRecipe(this.recipeId ,formData).subscribe(res => {
        if (res['code'] === 200) {
          this.router.navigateByUrl('/recipies');
        }     
      }, error => {      
      });
    }else
    {
      this.recipeService.createRecipies(formData).subscribe(res => {
        if (res['code'] === 201) {
          this.router.navigateByUrl('/recipies');
        }     
      }, error => {      
      });
    }
  }

   get validations() {
    return this.recipeForm.controls;
  }
   get stepsList()  {
    return this.recipeForm.get('recipeStep') as FormArray;
  }
   get ingredientList()  {         
    return this.recipeForm.get("recipeIngredient") as FormArray;
  }
  addSteps() {
    (<FormArray>this.recipeForm.get('recipeStep')).push(this.addnewStep(0,'',0));
  }
  addIngredient() {    
    (<FormArray>this.recipeForm.get('recipeIngredient')).push(this.addnewIngredient(0,'',0));
  }

  handleFileInput1(files: FileList) {
    this.fileToUpload1 = files.item(0);
  }
  handleFileInput2(files: FileList) {
    this.fileToUpload2 = files.item(0);
  }
  handleFileInput3(files: FileList) {
    this.fileToUpload3 = files.item(0);
  }
  getRecipieDetail(id){    
    if(id>0)
    {
    this.recipeService.getById(id).subscribe(data =>{
      if(data['code']=== 200){        
        this.recipieDetail=data['response'];
        this.initForm(data['response']);
      }
    })
   }
  }

  initForm(recipieObj) {        
      this.onPatch();   
  }


}
