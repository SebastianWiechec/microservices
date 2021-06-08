using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserApi.IServices;
using UserApi.Models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService { get; }
        private UserManager<IdentityUser> userManager { get; }
        private readonly IConfiguration configuration;

        public UserController(IUserService _userService, UserManager<IdentityUser> _userManager,
            IConfiguration _configuration)
        {
            userService = _userService;
            userManager = _userManager;
            configuration = _configuration;
        }

        /// <summary>
        /// metoda obsługująca request GET dla api/User
        /// </summary>
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<IdentityUser>> Get()
        {
            return await userService.GetUsers();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IDictionary<string, object>> GetUserByIdAsync(string id)
        {
            return await userService.GetUserById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("register")]
        public async Task<string> Post([FromBody] IdentityUser user, string password)
        {
            return await userService.AddUser(user, password);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    role = userRoles[0]
                });
            }
            return Unauthorized();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<ExpandoObject> Put([FromBody] ExpandoObject user)
        {
            return userService.UpdateUser(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(string id)
        {
            return await userService.DeleteUser(id);
        }
    }
}
