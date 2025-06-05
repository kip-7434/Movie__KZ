using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Overview { get; set; }
        public string Cast { get; set; }
        public string Crew { get; set; }
        public string Ratings { get; set; }
        [NotMapped]
        public IFormFile LogoFile { get; set; }
        public String Logo { get; set; }
    }
}
