using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Movie_Recommendation.Servicces;
using Newtonsoft.Json.Linq;

namespace Movie_Recommendation.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "05ea412cd238ca149a9e9f653a6a5cd8";

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<List<JToken>> GetPopularMoviesAsync()
        //{
        //    string url = $"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&language=en-US&page=1";

        //    try
        //    {
        //        var response = await _httpClient.GetStringAsync(url);
        //        JObject json = JObject.Parse(response);
        //        return json["results"]?.ToList(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error fetching movies: " + ex.Message);
        //        return new List<JToken>();
        //    }
        //}

        public async Task<List<MovieDto>> GetPopularMoviesAsync()
        {
            string url = $"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&language=en-US&page=1";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            var movies = json["results"]
                .Select(result => new MovieDto
                {
                    Adult = (bool)result["adult"],
                    BackdropPath = (string)result["backdrop_path"],
                    GenreIds = result["genre_ids"].Select(g => (int)g).ToList(),
                    Id = (int)result["id"],
                    OriginalLanguage = (string)result["original_language"],
                    OriginalTitle = (string)result["original_title"],
                    Overview = (string)result["overview"],
                    Popularity = (double)result["popularity"],
                    PosterPath = (string)result["poster_path"],
                    ReleaseDate = (string)result["release_date"],
                    Title = (string)result["title"],
                    Video = (bool)result["video"],
                    VoteAverage = (double)result["vote_average"],
                    VoteCount = (int)result["vote_count"]
                })
                .ToList();

            return movies;
        }

    }
}
