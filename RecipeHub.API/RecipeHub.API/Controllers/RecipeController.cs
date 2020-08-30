using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeHub.Business;
using RecipeHub.Model;
using RecipeHub.Model.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RecipeController(IRecipeService recipeService,IWebHostEnvironment IWebHostEnvironment)
        {
            this.recipeService = recipeService;
            webHostEnvironment = IWebHostEnvironment;
        }

        // GET: api/<RecipeController>
        [HttpGet]

        //public ResponseList<vmRecipe> Get()
        //{
        //    return new ResponseList<vmRecipe>(){ code = (int)HttpStatusCode.OK, message = "recipe list", response = this.recipeService.GetRecipes().ToList() };
        //}
        //[HttpGet]
        //[Route("api/getby")]
        public ResponseList<vmRecipe> Get(string filterLevel,  string sortByDate)
        {
            if (string.IsNullOrEmpty(filterLevel) && sortByDate == "ASC")
                return new ResponseList<vmRecipe>() { code = (int)HttpStatusCode.OK, message = "sort by date asc", response = this.recipeService.GetRecipes().ToList().OrderBy(d => d.CreatedDate).ToList() };
            else if (string.IsNullOrEmpty(filterLevel) && sortByDate == "DESC")
                return new ResponseList<vmRecipe>() { code = (int)HttpStatusCode.OK, message = "sort by date desc", response = this.recipeService.GetRecipes().ToList().OrderByDescending(d => d.CreatedDate).ToList()    };
            else if (!string.IsNullOrEmpty(filterLevel) && sortByDate == "ASC")
                return new ResponseList<vmRecipe>() { code = (int)HttpStatusCode.OK, message = "sort by level asc", response = this.recipeService.GetRecipes().Where(o => o.Levels==filterLevel).OrderBy(d=>d.CreatedDate).ToList() };
            else if(!string.IsNullOrEmpty(filterLevel) && sortByDate == "DESC")
                return new ResponseList<vmRecipe>() { code = (int)HttpStatusCode.OK, message = "sort by level desc", response = this.recipeService.GetRecipes().Where(o => o.Levels==filterLevel).OrderByDescending(d => d.CreatedDate).ToList() };
            else
                return new ResponseList<vmRecipe>() { code = (int)HttpStatusCode.OK, message = "recipe list", response = this.recipeService.GetRecipes().ToList() };
        }
    

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public ResponseItem<vmRecipe> Get(int id)
        {
            return new ResponseItem<vmRecipe>() { code = (int) HttpStatusCode.OK, message = "recipe Item", response = this.recipeService.GetRecipeById(id) };
        }

        [HttpGet("getrecipe/{id}")]
        public ResponseItem<Recipe> GetRecipe(int id)
        {
            return new ResponseItem<Recipe>() { code = (int)HttpStatusCode.OK, message = "recipe Item", response = this.recipeService.GetById(id) };
        }

        // POST api/<RecipeController>
        [HttpPost]
        public ResponseItem<Recipe> Post([FromForm] vmRecipeRequest model)
        {
            if (ModelState.IsValid)
            {
                Recipe recipe = new Recipe(); 
                recipe.Title = model.Title;
                recipe.CreatedDate = DateTime.Now;
                recipe.Levels = model.Levels;

                recipe.RecipeStep = JsonConvert.DeserializeObject<List<RecipeStep>>(model.RecipeStep);
                recipe.RecipeIngredient = JsonConvert.DeserializeObject<List<RecipeIngredient>>(model.RecipeIngredient);
                               
                if (!Directory.Exists(webHostEnvironment.ContentRootPath + "\\Upload\\"))
                {
                    Directory.CreateDirectory(webHostEnvironment.ContentRootPath + "\\Upload\\");
                };
                string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Upload");
                if (model.File1 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File1.FileName);
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff")+ fileExtenstion; //Guid.NewGuid().ToString() + "_" + model.File1.FileName;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File1.CopyTo(fileStream);
                        fileStream.Flush();
                        recipe.Filename1 = uniqueFileName ;
                    }
                }
                if (model.File2 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File2.FileName);
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtenstion;  //Guid.NewGuid().ToString() + "_" + model.File2.FileName;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName );
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File2.CopyTo(fileStream);
                        fileStream.Flush();
                        recipe.Filename2 = uniqueFileName;
                    }

                }
                if (model.File3 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File3.FileName);
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtenstion;//Guid.NewGuid().ToString() + "_" + model.File3.FileName;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName );
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File3.CopyTo(fileStream);
                        fileStream.Flush();
                        recipe.Filename3 = uniqueFileName;
                    }

                }
                return new ResponseItem<Recipe>() { code = (int)HttpStatusCode.Created, message = "recipe Item", response = this.recipeService.CreateRecipe(recipe) };
            }
            else
            { 
                return new ResponseItem<Recipe>() { code = (int)HttpStatusCode.BadRequest, message = "recipe Item", response = null };
            }
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public ResponseItem<Recipe> Put(int id, [FromForm] vmRecipeRequest model)
        {
            Recipe existingRecipe = this.recipeService.GetById(id);
            if (existingRecipe != null)
            {
                existingRecipe.RecipeStep = JsonConvert.DeserializeObject<List<RecipeStep>>(model.RecipeStep);
                existingRecipe.RecipeIngredient = JsonConvert.DeserializeObject<List<RecipeIngredient>>(model.RecipeIngredient);   
                existingRecipe.Title = model.Title;
                existingRecipe.Levels = model.Levels;
                //recipe.CreatedDate = DateTime.Now;

                if (!Directory.Exists(webHostEnvironment.ContentRootPath + "\\Upload\\"))
                {
                    Directory.CreateDirectory(webHostEnvironment.ContentRootPath + "\\Upload\\");
                };
                string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Upload");
                if (model.File1 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File1.FileName);
                    string uniqueFileName = (!string.IsNullOrEmpty(existingRecipe.Filename1) ?  Path.GetFileNameWithoutExtension(existingRecipe.Filename1): DateTime.Now.ToString("yyyyMMddHHmmssfff")) 
                                              + fileExtenstion;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File1.CopyTo(fileStream);
                        fileStream.Flush();
                        existingRecipe.Filename1 = uniqueFileName;
                    }
                }
                if (model.File2 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File2.FileName);
                    string uniqueFileName = (!string.IsNullOrEmpty(existingRecipe.Filename2) ? Path.GetFileNameWithoutExtension(existingRecipe.Filename2) : DateTime.Now.ToString("yyyyMMddHHmmssfff"))
                                                   + fileExtenstion; 
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File2.CopyTo(fileStream);
                        fileStream.Flush();
                        existingRecipe.Filename2 = uniqueFileName;
                    }

                }
                if (model.File3 != null)
                {
                    string fileExtenstion = Path.GetExtension(model.File3.FileName);
                    string uniqueFileName = (!string.IsNullOrEmpty(existingRecipe.Filename3) ? Path.GetFileNameWithoutExtension(existingRecipe.Filename3) : DateTime.Now.ToString("yyyyMMddHHmmssfff"))
                                              + fileExtenstion;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filepath))
                    {
                        model.File3.CopyTo(fileStream);
                        fileStream.Flush();
                        existingRecipe.Filename3 = uniqueFileName;
                    }

                }

                return new ResponseItem<Recipe>() { code = (int)HttpStatusCode.OK, message = "recipe Item", response = this.recipeService.UpdateRecipe(existingRecipe) };
            }
            else
            {
                return new ResponseItem<Recipe>() { code = (int)HttpStatusCode.NotFound, message = "recipe Item", response = null };

            }

        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("seeding/{id}")]
        public void Seeding(int id)
        {
            this.recipeService.Seeding(id);
        }
    }
}
