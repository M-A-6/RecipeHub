using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeHub.Model
{
    [NotMapped]
    public class ResponseItem<T> 
    {
        public int code { get; set; }
        public string message { get; set; }
        public T response { get; set; }
    }
    [NotMapped]
    public class ResponseList<T>
    {
        public int code { get; set; }
        public int count { get; set; }
        public string message { get; set; }
        public List<T> response { get; set; }
    }
    [NotMapped]
    public class ResponseResult
    {
        public int code { get; set; }
        public string message { get; set; }
        public string response { get; set; }
    }
}
