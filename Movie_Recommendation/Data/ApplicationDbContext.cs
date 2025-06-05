using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Movie_Recommendation.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieCategories> MovieCategories { get; set; }
    }

}
