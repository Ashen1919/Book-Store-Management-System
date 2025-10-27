using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user != null) 
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var jwtToken = tokenRepository.CreateJwtToken(user);

                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };

                    return Ok(response);
                }
            }

            return BadRequest("Email or Password in incorrect");
        }
    }
}
