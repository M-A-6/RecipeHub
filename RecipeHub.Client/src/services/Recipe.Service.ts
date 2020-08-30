import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class RecipeService {
    private  BASE_API: string = environment.baseApiUrl;
    private  FILE_PATH: string = environment.filePath;
    constructor(private http: HttpClient) { }

    getHeaders(){
       // this.currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
    //const headers = new HttpHeaders().set('Authorization', 'Bearer ' + this.currentUser.token);
       //return headers;
       return '';
   }

    getallRecipies(filterLevel, sortByDate) {
        console.log(filterLevel,sortByDate);
        return this.http.get(this.BASE_API + "/recipe?filterLevel="+filterLevel + "&sortByDate="+sortByDate);
    }

    getRecipeById(recipeId) {
       return this.http.get(`${this.BASE_API}/recipe/${recipeId}`);      
    }

    getById(recipeId) {
        return this.http.get(`${this.BASE_API}/recipe/getrecipe/${recipeId}`);      
    }
    
    updateRecipe(recipeId,data){
        return this.http.put(`${this.BASE_API}/recipe/${recipeId}`, data);      
    }

    createRecipies(data) {
        return this.http.post(`${this.BASE_API}/recipe/`, data);
    }



    getFilePath()
    {
        console.log(this.FILE_PATH);
        console.log(`${this.FILE_PATH}`);
        return `${this.FILE_PATH}`;
    }
    
}