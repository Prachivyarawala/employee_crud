using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserRepositories _userrepo;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiController(IUserRepositories userrepo, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userrepo = userrepo;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromForm] LoginRequest request)
        {
            try
            {
                User loginUser = _userrepo.Login(request.Email, request.Password);
                if (loginUser != null)
                {
                    HttpContext.Session.SetInt32("userid", loginUser.c_userid); // Assuming c_userid is int
                    HttpContext.Session.SetString("username", loginUser.c_username);

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("userid", loginUser.c_userid.ToString()),
                        new Claim("username", loginUser.c_username),
                        new Claim("email", loginUser.c_useremail)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signIn
                    );

                    return Ok(new { userid = loginUser.c_userid, username = loginUser.c_username });
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromForm] User User)
        {
            bool ans = _userrepo.Register(User);
            return Ok(ans);
        }

        // Other actions...

    }
}
