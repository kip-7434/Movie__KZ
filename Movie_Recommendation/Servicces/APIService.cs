using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Recommendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Servicces
{

    public interface IAPIService
    {
     Task <IEnumerable<Movies>> GetAll();
     Task<Movies> GetMovie(int id);
     Task<Movies> Create(Movies model, IFormFile? file);
     Task<Movies> Update(int id, Movies model);
     Task<Movies> Delete(int id);

    }
    public class APIService : IAPIService
    {
        private readonly ApplicationDbContext _context;

        public APIService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<IEnumerable<Movies>> GetAll()
        {
            var allMovies =  _context.Movies.ToList();
            return allMovies; 
        }
        public async Task<Movies> GetMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            if(movie != null)
            {
                return movie;
            }
            return null;
        }

        public async Task<Movies> Create(Movies model, IFormFile? file)
        {
           
            Movies newMovie = new Movies
            {
                
                Title = model.Title,
                Poster = model.Poster,
                Overview = model.Overview,
                Cast = model.Cast,
                Crew = model.Crew,
                Ratings = model.Ratings,
                LogoFile = model.LogoFile,
                Logo = model.Logo

            };
            _context.Movies.Add(newMovie);
            _context.SaveChanges();
            return newMovie;
        }

        public async Task<Movies>Update(int id, Movies model)
        {
            var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            if (movie != null)
            {
                movie.Title = model.Title;
                movie.Poster = model.Poster;
                movie.Overview = model.Overview;
                movie.Cast = model.Cast;
                movie.Crew = model.Crew;
                movie.Ratings = model.Ratings;
                movie.LogoFile = model.LogoFile;
                movie.Logo = model.Logo;
                _context.SaveChanges();
                return model;
          }
            return null;

        }
        public async Task<Movies> Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return null;
        }
     }
}
