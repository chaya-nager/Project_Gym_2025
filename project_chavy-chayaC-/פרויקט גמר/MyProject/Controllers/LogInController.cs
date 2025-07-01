using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
      
        private IService<UserDto> service;
        private readonly IConfiguration config;
        private readonly IRepository<User> userRepository;

        public LogInController(IService<UserDto> service, IConfiguration config, IRepository<User> userRepository)
        {
            this.service = service;
            this.config = config;
            this.userRepository = userRepository;
        }
        // GET: api/<LogInController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LogInController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LogInController>
        [HttpPost]
        public async Task Post([FromForm]UserDto user)
        {
            await service.AddItemAsync(user);
        }
        [HttpPost("login")]
        public async Task<string> LogIn([FromForm] UserLogIn value)
        {
            //var user = await Authenticat(value);
            //var token = Generate(user);
            //return token;
            var userDto = await Authenticat(value);
            if (userDto == null)
                return "Unauthorized"; 
            var user = await userRepository.GetByIdAsync(userDto.UserId); 
            //var planner = new CreateWorkoutPlan(userRepository, videoRepository, planRepository);
            //var plan = await planner.GenerateWorkoutPlan(user.UserId, 30, "בינוני"); 
            var token = Generate(userDto);
            return token;
        }
        [HttpGet("GetUserByToken")]
        [Authorize]
        public IActionResult GetUserByToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                var email = identity.FindFirst(ClaimTypes.Email)?.Value;
                var fullName = identity.FindFirst(ClaimTypes.Name)?.Value;
                var role = identity.FindFirst(ClaimTypes.Role)?.Value;

                return Ok(new
                {
                    Email = email,
                    FullName = fullName,
                    UserType = role
                });
            }

            return BadRequest("Invalid token");
        }
        private string Generate(UserDto user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.Email, user.Email),    
            new Claim(ClaimTypes.Role, user.UserType),
            new Claim(ClaimTypes.Name,user.FullName),
            new Claim("UserId", user.UserId.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<UserDto> Authenticat(UserLogIn value)
        {
            var users =await service.GetAllAsync();
            UserDto user= users.FirstOrDefault( x => x.Email == value.Email && x.FullName == value.UserName);
            if(user!=null)
                return user;
            return null;
        }
        // PUT api/<LogInController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<LogInController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
