using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RecipeHub.Data;
using RecipeHub.Model;
using RecipeHub.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHub.Business
{
    public class RecipeService : IRecipeService, IDb
    {
        public RecipeHubDb db { get; }
        public RecipeService(RecipeHubDb dbContext)
        {
            this.db = dbContext;
        }

        public IQueryable<vmRecipe> GetRecipes()
        {
            var partialResult = (from r in this.db.Recipe
                                 select new vmRecipe
                                 {
                                     Id = r.Id,
                                     Levels = r.Levels,
                                     Title = r.Title,
                                     Filename1 = r.Filename1,
                                     Filename2 = r.Filename2,
                                     Filename3 = r.Filename3,
                                     CreatedDate = r.CreatedDate,
                                     RecipeStep = string.Join(",", this.db.RecipeStep.Where(i => i.RecipeId == r.Id).Select(x => x.StepName)),
                                     RecipeIngredient = string.Join(",", this.db.RecipeIngredient.Where(i => i.RecipeId == r.Id).Select(x => x.IngredientName))
                                 });
            return partialResult;
        }

        public vmRecipe GetRecipeById(int id)
        {
            Recipe existingrecipe = this.db.Recipe.Where(r => r.Id == id).FirstOrDefault();

            vmRecipe recipe = new vmRecipe();
            recipe.Id = existingrecipe.Id;
            recipe.Title = existingrecipe.Title;
            recipe.Levels = existingrecipe.Levels;
            recipe.Filename1 = existingrecipe.Filename1;
            recipe.Filename2 = existingrecipe.Filename2;
            recipe.Filename3 = existingrecipe.Filename3;
            recipe.CreatedDate = existingrecipe.CreatedDate;
            recipe.RecipeStep = string.Join(",", this.db.RecipeStep.Where(r => r.RecipeId == id).ToList().Select(x => x.StepName));
            recipe.RecipeIngredient = string.Join(",", this.db.RecipeIngredient.Where(r => r.RecipeId == id).Select(x => x.IngredientName));
            return recipe;
        }

        public Recipe GetById(int id)
        {
            Recipe existingrecipe = this.db.Recipe.Where(r => r.Id == id).FirstOrDefault();

            Recipe recipe = new Recipe();
            recipe.Id = existingrecipe.Id;
            recipe.Title = existingrecipe.Title;
            recipe.Levels = existingrecipe.Levels;
            recipe.Filename1 = existingrecipe.Filename1;
            recipe.Filename2 = existingrecipe.Filename2;
            recipe.Filename3 = existingrecipe.Filename3;
            recipe.CreatedDate = existingrecipe.CreatedDate;
            recipe.RecipeStep = this.db.RecipeStep.Where(r => r.RecipeId == id).ToList();
            recipe.RecipeIngredient = this.db.RecipeIngredient.Where(r => r.RecipeId == id).ToList();
            return recipe;
        }

        public Recipe CreateRecipe(Recipe recipe)
        {
            this.db.Recipe.Add(recipe);
            this.db.SaveChanges();
            return recipe;
        }
        public Recipe UpdateRecipe(Recipe recipe)
        {
            Recipe existingRecipe= this.db.Recipe.Where(r => r.Id == recipe.Id).FirstOrDefault();
            if (existingRecipe != null)
            {
                existingRecipe.Title = recipe.Title;
                existingRecipe.Levels = recipe.Levels;

                if (recipe.Filename1 != "")
                {
                    existingRecipe.Filename1 = recipe.Filename1;
                }
                if (recipe.Filename2 != "")
                {
                    existingRecipe.Filename2 = recipe.Filename2;
                }
                if (recipe.Filename3 != "")
                {
                    existingRecipe.Filename3 = recipe.Filename3;
                }
                this.db.Attach(existingRecipe);
                this.db.Entry(existingRecipe).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            foreach (var step in recipe.RecipeStep)
            {
                if (step.Id == 0 && step.RecipeId == 0)
                {
                    step.RecipeId = recipe.Id;
                    CreateRecipeStep(step);
                }
                else if (step.Id > 0 )
                {
                    RecipeStep restep = this.db.RecipeStep.Where(r => r.Id == step.Id).FirstOrDefault();
                    if (restep != null)
                    {
                        restep.StepName = step.StepName;
                        UpdateRecipeStep(restep);
                    }
                }
            }
            foreach (var ingredient in recipe.RecipeIngredient)
            {
                if (ingredient.Id == 0 && ingredient.RecipeId == 0)
                {
                    ingredient.RecipeId = recipe.Id;
                    CreateRecipeIngredient(ingredient);
                }
                else if (ingredient.Id > 0)
                {
                    RecipeIngredient rIngredient = this.db.RecipeIngredient.Where(r => r.Id == ingredient.Id).FirstOrDefault();
                    if (rIngredient != null)
                    {
                        rIngredient.IngredientName = ingredient.IngredientName;
                        UpdateRecipeIngredient(rIngredient);
                    }
                }
            }
            return recipe;
        }

        public RecipeStep UpdateRecipeStep(RecipeStep recipeStep)
        {
            this.db.Attach(recipeStep);
            this.db.Entry(recipeStep).State = EntityState.Modified;
            this.db.SaveChanges();
            return recipeStep;
        }
        public RecipeStep CreateRecipeStep(RecipeStep recipeStep)
        {
            this.db.Add(recipeStep);            
            this.db.SaveChanges();
            return recipeStep;
        }

        public RecipeIngredient CreateRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            this.db.Add(recipeIngredient);         
            this.db.SaveChanges();
            return recipeIngredient;
        }
        public RecipeIngredient UpdateRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            this.db.Attach(recipeIngredient);
            this.db.Entry(recipeIngredient).State = EntityState.Modified;
            this.db.SaveChanges();
            return recipeIngredient;
        }

        public void Seeding(int number)
        {           
            for (int i = 1; i <= number; i++)
            {
                this.CreateRecipe(new Recipe
                {
                    //Id = i,
                    Title = "Tile " + i.ToString(),
                    Levels = getLevel(i),
                    CreatedDate = Convert.ToDateTime(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd")),
                    RecipeStep = new List<RecipeStep>() { new RecipeStep {  StepName = i.ToString() + "Step 01" }, new RecipeStep { StepName = i.ToString() + "Step 02" } },
                    RecipeIngredient = new List<RecipeIngredient>() { new RecipeIngredient { IngredientName = i.ToString() + "Ing 01" }, new RecipeIngredient { IngredientName = i.ToString() + "Ing 02" } } 
                });
            }            
        }

        public string getLevel(int i)
        {
            string retVal = "Easy";
            if (i % 3 == 1)
            {
                retVal = "Medium";
            }
            else if (i % 3 == 2)
            {
                retVal = "Hard";
            }
            return retVal;
        }

    }

}
