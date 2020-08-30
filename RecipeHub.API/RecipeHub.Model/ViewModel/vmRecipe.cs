using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeHub.Model.ViewModel
{
    public class vmRecipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Levels { get; set; }
        public string Filename1 { get; set; }
        public string Filename2 { get; set; }
        public string Filename3 { get; set; }
        public string RecipeStep { get; set; }
        public string RecipeIngredient { get; set; }
    }

    public class vmRecipeRequest
    {
        [Required]
        public string Title { get; set; }        
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Levels { get; set; }
        public  string RecipeStep { get; set; }
        public  string RecipeIngredient { get; set; }
        public IFormFile? File1 { get; set; }
        public IFormFile? File2 { get; set; }
        public IFormFile? File3 { get; set; }

    }
}
