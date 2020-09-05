using RecipeHub.Model;
using RecipeHub.Model.ViewModel;
using System;
using System.Linq;

namespace RecipeHub.Business
{
    public interface IRecipeService
    {
        IQueryable<Recipe> GetRecipes();
        IQueryable<vmRecipe> GetRecipes(int page, int pageSize);
        vmRecipe GetRecipeById(int id);
        Recipe GetById(int id);
        Recipe CreateRecipe(Recipe recipe);
        Recipe UpdateRecipe(Recipe recipe);
        void Seeding(int number);
    }
}
