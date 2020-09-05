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
        public IActionResult Get(string filterLevel,  string sortByDate, int page=1, int pageSize=20)
        {
            ResponseList<vmRecipe> retVal = new ResponseList<vmRecipe>()
            {
                code = (int)HttpStatusCode.BadRequest,
                message = "bad request",
                count = 0
            };
            try
            {
                if (string.IsNullOrEmpty(filterLevel) && sortByDate == "ASC")
                {
                    retVal.count = this.recipeService.GetRecipes().Count();
                    retVal.response = this.recipeService.GetRecipes(page, pageSize).ToList().OrderBy(d => d.CreatedDate).ToList();
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "sort by date asc";
                }
                else if (string.IsNullOrEmpty(filterLevel) && sortByDate == "DESC")
                {
                    retVal.count = this.recipeService.GetRecipes().Count();
                    retVal.response = this.recipeService.GetRecipes(page, pageSize).ToList().OrderByDescending(d => d.CreatedDate).ToList();
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "sort by date desc";
                }
                else if (!string.IsNullOrEmpty(filterLevel) && sortByDate == "ASC")
                {
                    retVal.count = this.recipeService.GetRecipes().Where(o => o.Levels == filterLevel).Count();
                    retVal.response = this.recipeService.GetRecipes(page, pageSize).Where(o => o.Levels == filterLevel).OrderBy(d => d.CreatedDate).ToList();
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "filter by level and sort by date asc";
                }
                else if (!string.IsNullOrEmpty(filterLevel) && sortByDate == "DESC")
                {
                    retVal.count = this.recipeService.GetRecipes().Where(o => o.Levels == filterLevel).Count();
                    retVal.response = this.recipeService.GetRecipes(page, pageSize).Where(o => o.Levels == filterLevel).OrderByDescending(d => d.CreatedDate).ToList();
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "filter by level and sort by date desc";
                }
                else
                {
                    retVal.count = this.recipeService.GetRecipes().Count();
                    retVal.response = this.recipeService.GetRecipes(page,pageSize).ToList();
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "recipe list";
                }
            }
            catch (Exception ex)
            {
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return Ok(retVal);
        }
    

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public ResponseItem<vmRecipe> Get(int id)
        {
            ResponseItem<vmRecipe> retVal = new ResponseItem<vmRecipe>()
            {
                code = (int)HttpStatusCode.BadRequest,
                message ="bad request"                
            };
            try
            {
                retVal.response= this.recipeService.GetRecipeById(id);
                retVal.code = (int)HttpStatusCode.OK;
                retVal.message = "recipe Item"; 
                
            } catch (Exception ex)
            {                
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return retVal;
        }

        [HttpGet("getrecipe/{id}")]
        public IActionResult GetRecipe(int id)
        {
            ResponseItem<Recipe> retVal = new ResponseItem<Recipe>()
            {
                code = (int)HttpStatusCode.BadRequest,
                message = "bad request"
            };
            try
            {
                retVal.response = this.recipeService.GetById(id);
                retVal.code = (int)HttpStatusCode.OK;
                retVal.message = "recipe Item";                
            }
            catch (Exception ex)
            {
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return Ok(retVal);
        }

        // POST api/<RecipeController>
        [HttpPost]
        public IActionResult Post([FromForm] vmRecipeRequest model)
        {
            ResponseItem<Recipe> retVal = new ResponseItem<Recipe>()
            {
                code = (int)HttpStatusCode.BadRequest,
                message = "bad request"
            };
            try
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
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtenstion; //Guid.NewGuid().ToString() + "_" + model.File1.FileName;
                        string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (FileStream fileStream = System.IO.File.Create(filepath))
                        {
                            model.File1.CopyTo(fileStream);
                            fileStream.Flush();
                            recipe.Filename1 = uniqueFileName;
                        }
                    }
                    if (model.File2 != null)
                    {
                        string fileExtenstion = Path.GetExtension(model.File2.FileName);
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtenstion;  //Guid.NewGuid().ToString() + "_" + model.File2.FileName;
                        string filepath = Path.Combine(uploadsFolder, uniqueFileName);
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
                        string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (FileStream fileStream = System.IO.File.Create(filepath))
                        {
                            model.File3.CopyTo(fileStream);
                            fileStream.Flush();
                            recipe.Filename3 = uniqueFileName;
                        }

                    }
                    retVal.response = this.recipeService.CreateRecipe(recipe);
                    retVal.code = (int)HttpStatusCode.Created;
                    retVal.message = "recipe Item";
                }
                else
                {

                    retVal.code = (int)HttpStatusCode.BadRequest;
                    retVal.message = "invalid form";
                }
            }
            catch (Exception ex)
            {
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return Ok(retVal);            
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] vmRecipeRequest model)
        {
            ResponseItem<Recipe> retVal = new ResponseItem<Recipe>()
            {
                code = (int)HttpStatusCode.BadRequest,
                message = "bad request"
            };
            try
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
                        string uniqueFileName = (!string.IsNullOrEmpty(existingRecipe.Filename1) ? Path.GetFileNameWithoutExtension(existingRecipe.Filename1) : DateTime.Now.ToString("yyyyMMddHHmmssfff"))
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
                    retVal.response = this.recipeService.UpdateRecipe(existingRecipe);
                    retVal.code = (int)HttpStatusCode.OK;
                    retVal.message = "recipe Item";
                }
                else
                {
                    retVal.code = (int)HttpStatusCode.NotFound; 
                    retVal.message = "recipe Item";
                }
            }
            catch (Exception ex)
            {
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return Ok(retVal);
        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("seeding/{id}")]
        public ResponseResult Seeding(int id)
        {
            ResponseResult retVal = new ResponseResult()
            {
                code = (int)HttpStatusCode.BadRequest,
                message = "bad request",
                response = "false"                
            };
            try
            {
                this.recipeService.Seeding(id);
                retVal.code = (int)HttpStatusCode.OK;
                retVal.response = "true";
            }
            catch (Exception ex)
            {
                retVal.code = (int)HttpStatusCode.InternalServerError;
                retVal.message = "Exception" + ex.Message;
            }
            return retVal;
        }
    }
}
