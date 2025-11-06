using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.DTO.LoginDto;
using RathnaBookStore.API.Repositories.Auth_Repository;

namespace RathnaBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //Register an user
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDto.Email, //this is mandatory
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            var identituResult = await userManager.CreateAsync(IdentityUser, registerRequestDto.Password);

            if (identituResult.Succeeded)
            {
                return Ok("Successfully Register User. Please Login.");
            }

            return BadRequest("Something Went Wrong");
        }

        //Login User
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginRequestDto.Email);

                if (user != null && await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                {
                    var jwtToken = tokenRepository.CreateJwtToken(user);
                    return Ok(new LoginResponseDto { JwtToken = jwtToken });
                }

                return BadRequest("Invalid email or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        //check database is connected
        [HttpGet("test-db-connection")]
        public async Task<IActionResult> TestDbConnection([FromServices] BookStoreAuthDbContext context)
        {
            try
            {
                await context.Database.CanConnectAsync();
                return Ok("✅ Database connection successful!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }
    }
}
