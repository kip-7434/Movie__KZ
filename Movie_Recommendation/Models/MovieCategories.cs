using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Models
{
    public class MovieCategories
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
