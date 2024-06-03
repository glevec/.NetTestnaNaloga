using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestnaNaloga.DTO;
using TestnaNaloga.Models;

namespace TestnaNaloga.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // POST: /api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO request)
        {
            string roleName = Enum.GetName(typeof(UserRole), request.Role);
            var user = new User { UserName = request.Email, Email = request.Email, Name = request.Name };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(roleResult.Errors);
            }

            var token = GenerateJwtToken(user.Email, user.Id, new List<string> { roleName });

            return Ok(new { Token = token });
        }


        // POST: /api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user.Email, user.Id, roles.ToList());
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string email, string userId, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),  // Use userId here
                new Claim(ClaimTypes.Email, email)  // Explicitly use email claim for email
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
