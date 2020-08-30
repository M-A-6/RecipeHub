using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Model
{
    public class ResponseItem<T> 
    {
        public int code { get; set; }
        public string message { get; set; }
        public T response { get; set; }
    }
    public class ResponseList<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<T> response { get; set; }
    }
}
