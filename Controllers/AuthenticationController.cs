using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoAPI.Authenication;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task <IActionResult> Login([FromBody] LoginModel Model)
        {
            var user = await userManager.FindByNameAsync(Model.Username);
            if(user != null && await userManager.CheckPasswordAsync(user, Model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim ("name", user.UserName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim("role", userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:secretKey"]));

                var token = new JwtSecurityToken(
                    issuer : _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register ([FromBody] RegisterModel Model)
        {
           var userexists = await userManager.FindByNameAsync(Model.Username);
            if (userexists != null)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Usuário já existe!" });
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = Model.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = Model.Email
            };

            var result = await userManager.CreateAsync(user, Model.Password);

            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao criar usuário" });
            }

            return Ok(new { message = "Usuário criado com sucesso!" });
        }

        [HttpPost]
        [Route("Register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel Model)
        {
            var userexists = await userManager.FindByNameAsync(Model.Username);
            if (userexists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Usuário já existe!" });
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = Model.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = Model.Email
            };
            var result = await userManager.CreateAsync(user, Model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao criar usuário" });
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.RoleExistsAsync("Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            return Ok(new { message = "Usuário criado com sucesso!" });
        }
    }
}
