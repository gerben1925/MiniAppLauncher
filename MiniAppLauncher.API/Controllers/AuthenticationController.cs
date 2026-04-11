using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Features.Auth.UseCases;
using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Features.User.UseCases;

namespace MiniAppLauncher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly VerifyLoginOtpUseCase _verifyLoginOtpUseCase;
        public AuthenticationController(LoginUseCase loginUseCase, VerifyLoginOtpUseCase verifyLoginOtpUseCase)
        {
            _loginUseCase = loginUseCase;
            _verifyLoginOtpUseCase = verifyLoginOtpUseCase;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {

            var result = await _loginUseCase.ExecuteAsync(loginRequest);
            if (result.StatusCode == 200)
            {
                return Ok(new { Message = result.Data });
            }

            return StatusCode(500, new { Message = "Failed to generate OTP." });
        }

        [AllowAnonymous]
        [HttpPost("validate-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyLoginOtpRequest verifyLoginOtpRequest)
        {
            if (verifyLoginOtpRequest == null)
                return BadRequest(new { message = "Invalid OTP or UserReference Format!" });

            var result  =  await _verifyLoginOtpUseCase.ExecuteAsync(verifyLoginOtpRequest);

            if (result.StatusCode == 200)
            {
                return Ok(new { Message = result.Data });
            }

            return StatusCode(500, new { Message = "Failed to generate Token." });
        }


    }
}
