using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Features.User.UseCases;

namespace MiniAppLauncher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GetUsersUseCase _getUsersUseCase;
        private readonly RegisterUserUseCase _registerUserUseCase;
        public UserController(GetUsersUseCase getUsersUseCase, RegisterUserUseCase registerUserUseCase) 
        {
            _getUsersUseCase = getUsersUseCase;
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result  =  await _getUsersUseCase.ExecuteTaskAsync();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser([FromBody] NewUserRequest newUserRequest)
        {
            var result = await _registerUserUseCase.ExecuteTaskAsync(newUserRequest);
            if (result > 0)
            {
                return Ok(new  { Message = "User created successfully." });
            }

            return StatusCode(500, new  { Message = "An error occurred while creating user." });
        }
    }
}
