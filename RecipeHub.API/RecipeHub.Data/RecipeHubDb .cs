using Microsoft.EntityFrameworkCore;
using RecipeHub.Model;
using System;
using System.Collections.Generic;

namespace RecipeHub.Data
{
    public class RecipeHubDb : DbContext
    {
        public RecipeHubDb(DbContextOptions<RecipeHubDb> options) : base(options)
        {
        }
        //public RecipeHubDb()
        //{ 
        
        //}
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipeLevel> RecipeLevel { get; set; }
        public DbSet<RecipeStep> RecipeStep { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            int recipeStepId = 1;
            int recipeIngredientId = 1;
            for (int i = 1; i <= 10000; i++)
            {
                modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    Id = i,
                    Title = "Tile " + i.ToString(),
                    Levels = getLevel(i),
                    CreatedDate = Convert.ToDateTime(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"))
                });
                
                modelBuilder.Entity<RecipeStep>().HasData(new RecipeStep { Id= recipeStepId++, RecipeId = i, StepName = i.ToString() + "Step 01" });
                modelBuilder.Entity<RecipeStep>().HasData(new RecipeStep { Id= recipeStepId++, RecipeId = i, StepName = i.ToString() + "Step 02" });
                modelBuilder.Entity<RecipeIngredient>().HasData(new RecipeIngredient { Id = recipeIngredientId++, RecipeId = i, IngredientName = i.ToString() + "Ing 01" }) ;
                modelBuilder.Entity<RecipeIngredient>().HasData(new RecipeIngredient { Id = recipeIngredientId++, RecipeId = i, IngredientName = i.ToString() + "Ing 02" });
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-F84SMUP\\SQLEXPRESS;Initial Catalog=RECIPEHUB;Trusted_Connection=True;User ID=sa;Password=123;");
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
