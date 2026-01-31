using System;
using System.Collections.Generic;
using System.Text;

namespace Insurtix_Server.Models.Entities
{
    public class Book
    {
        public string Category { get; set; }
        public string? Cover { get; set; } 
        public int ISBN { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; } = "en"; 
        public List<string> Authors { get; set; } = new List<string>();
        public int Year { get; set; }
        public double Price { get; set; }
    }
}
