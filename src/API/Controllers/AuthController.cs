// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using Domain.Entities.Authentication;
// using API.Features.Authentication.Services;
// using API.Common.Controllers;
// using Artikelsystem.Shared.DTOs.User.Request;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

// namespace API.Controller;
// public class AuthController() : BaseController
// {

// }
//     [HttpPost("register")]
//     public async Task<ActionResult<User>> Register(UserDto request)
//     {
//         var user = await authService.RegisterAsync(request);

//         if(user is null)
//         {
//             return BadRequest("Username already exists");
//         }
//         return Ok(user);
//     }
//     [HttpPost("login")]
//     public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
//     {
//         var result = await authService.LoginAsync(request);
//         if(result is null)
//             return BadRequest("Falscher Username oder Passwort.");

//         return Ok(result);
//     }


//     [HttpPost("refresh-token")]
//     public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
//     {
//         var result = await authService.RefreshTokensAsync(request);
//         if(result is null || result.AccessToken is null || result.RefreshToken is null)
//         {
//             return Unauthorized("Invalid refresh token");
//         }

//         return Ok(result);
//     }
//     [Authorize]
//     [HttpGet]
//     public IActionResult AuthenticateOnlyEndpoint()
//     {
//         return Ok("du bist berechtigt!");
//     }

//     [Authorize(Roles = "Admin")]
//     [HttpGet("admin-only")]
//     public IActionResult AdminOnlyEndpoint()
//     {
//         return Ok("du bist admin!");
//     }

//     [Authorize]
//     [HttpGet("current-user")]
//     public IActionResult GetCurrentUser()
//     {
//         var userId = GetCurrentUserId();
//         var userName = GetCurrentUserName();
//         return Ok(new { UserId = userId, UserName = userName });
//     }
// }

