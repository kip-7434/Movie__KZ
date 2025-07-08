using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movie_Recommendation.Models;
using Movie_Recommendation.Servicces;
using Movie_Recommendation.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Recommendation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieApiController : ControllerBase
    {
        private readonly IAPIService _iAPIService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public MovieApiController(IAPIService iAPIService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _iAPIService = iAPIService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("{Login}")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("AB2004B7-B8C7-4E7D-A135-02E7CEBF0BE7");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Name, user.Id),
                            new Claim(ClaimTypes.GivenName, user.Email)
                        }),
                        Expires = DateTime.UtcNow.AddHours(3),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    var data = new
                    {
                        access_Token = tokenString,
                    };
                    return CreateSucessResponse("Success", data);
                }
                return CreateErrorResponse("User not Found", 422);
            }
            return CreateErrorResponse("Invalid Login Attempt", 422);

        }
        private JsonResult CreateSucessResponse(string message, object data = null)
        {
            var result = new JsonResult(new MReturnData<object>
            {
                Success = true,
                Message = message,
                Data = data,
            });
            result.StatusCode = 200;
            return result;
        }
        private JsonResult CreateErrorResponse(string message, int statusCode)
        {
            var result = new JsonResult(new MReturnData<string>
            {
                Success = false,
                Message = message
            });
            result.StatusCode = statusCode;
            return result;
        }
        public static string GetEmailFromToken(string loginToken)
        {
            var authorizationHeader = loginToken;
            if (authorizationHeader != null)
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "unique_name");
                return emailClaim?.Value;
            }
            return null;
        }

        [HttpGet("GetAllMovies")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetAllMovies()
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var email = GetEmailFromToken(authorizationHeaders);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Sorry, you have not logged in");
            }

            var movies = await _iAPIService.GetAll();
            return Ok(movies);
        }

        [HttpGet("GetMovie/{id}")]
        public async Task<ActionResult<Movies>> GetMovie(int id)
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var email = GetEmailFromToken(authorizationHeaders);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Sorry, you have nt loggged in/session expired");
            }
            var movie = _iAPIService.GetMovie(id);
            if(movie != null)
            {
                return Ok(movie);
            }
            return NotFound();
        }
       [HttpPost("Create")]
        public async Task<ActionResult<Movies>> Create([FromForm] Movies movie, [FromForm] IFormFile? file)
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var email = GetEmailFromToken(authorizationHeaders);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Sorry, You have not logged in/Loggin expired");
            }
            var data =  _iAPIService.Create(movie,file);
            return CreatedAtAction(nameof(GetMovie), new  { id = data.Id }, data);

        }
        [HttpPost("Update/{id}")]
        public async  Task<ActionResult<Movies>> Update(int id, Movies movies)
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var email = GetEmailFromToken(authorizationHeaders);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Sorry, You have not logged in/session expired");
            }
            var data = _iAPIService.Update(id, movies);
            if(data != null)
            {
                return Ok(data);
            }
            return NotFound();
        }
      
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var email = GetEmailFromToken(authorizationHeaders);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Sorry, you have not logged in/session expired");
            }
            var result = await _iAPIService.Delete(id);
            if (result ==null) return NotFound();
            return NoContent();
        }
    }
}
