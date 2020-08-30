using RecipeHub.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Business
{
   
    public interface IDb
    {
        RecipeHubDb db { get; }
    }
}
