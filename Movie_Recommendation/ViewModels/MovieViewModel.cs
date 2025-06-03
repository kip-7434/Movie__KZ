using Microsoft.AspNetCore.Http;
using Movie_Recommendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Overview { get; set; }
        public string Cast { get; set; }
        public string Crew { get; set; }
        public string Ratings { get; set; }
        public IFormFile LogoFile { get; set; }
        public String Logo { get; set; }
        public List<Movies> Movies { get; set; }
        public List<MovieCategories> MovieCategories { get; set; }
    }
}
