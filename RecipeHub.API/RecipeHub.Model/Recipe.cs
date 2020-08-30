using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHub.Model
{
    public class Recipe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Levels { get; set; }
        public string Filename1 { get; set; }
        public string Filename2 { get; set; }
        public string Filename3 { get; set; }   
        public List<RecipeStep> RecipeStep {get; set;}
        public List<RecipeIngredient> RecipeIngredient { get; set; }

    }
}
