// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;
// using API.Infrastructure.Persistence.Context;
// using Domain.Entities.Authentication;
// using Artikelsystem.Shared.DTOs.User.Request;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;

// namespace API.Features.Authentication.Services;

// public class AuthService : IAuthService
// {
//     private readonly AppDbContext _context;
//     private readonly IConfiguration _configuration;

//     public AuthService(AppDbContext context, IConfiguration configuration)
//     {
//         _context = context;
//         _configuration = configuration;
//     }

//     private async Task<User?> ValidateRefreshTokenAsync(int userID, string refreshToken)
//     {
//         var user = await _context.Users.FindAsync(userID);
//         if(user is null || user.RefreshToken != refreshToken 
//         || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
//         {
//             return null;
//         }
//         return user;
//     }

//     public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
//     {
//         var user = await ValidateRefreshTokenAsync(request.UserID, request.RefreshToken);
//         if(user == null)
//             return null;

//         return await CreateTokenResponse(user);
//     }

//     public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
//     {
//         var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
//         if(user == null)
//         {
//             return null!;
//         }
//         if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, request.Password)
//             == PasswordVerificationResult.Failed)
//             {
//                 return null!;
//             }
//         return await CreateTokenResponse(user);
//     }


//     private async Task<TokenResponseDto> CreateTokenResponse(User user)
//     {
//         return new TokenResponseDto
//         {
//             AccessToken = CreateToken(user),
//             RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
//         };
//     }

//     public async Task<User?> RegisterAsync(UserDto request)
//     {
//         if(await _context.Users.AnyAsync(u => u.UserName == request.Username))
//         {
//             return null;
//         }

//         var user = new User() {
//             UserName = string.Empty,
//             PasswordHash = "",
//             Name = string.Empty,
//             Nachname = string.Empty,
//             Email = string.Empty
//         };
        
//         if(request.Email is null || request.Nachname is null || request.Name is null)
//         {
//             return null;
//         }

//         var hashedPassword = new PasswordHasher<User>()
//             .HashPassword(user, request.Password);

//         user.UserName = request.Username;
//         user.PasswordHash = hashedPassword;
//         user.Email = request.Email;
//         user.Nachname = request.Nachname;
//         user.Name = request.Name;

//         _context.Users.Add(user);

//         await _context.SaveChangesAsync();

//         return user;
//     }

//     private string GenerateRefreshToken()
//     {
//         var randomNumber = new byte[32];
//         using var rng = RandomNumberGenerator.Create();
//         rng.GetBytes(randomNumber);
//         return Convert.ToBase64String(randomNumber);
//     }

//     private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
//     {
//         var refreshToken = GenerateRefreshToken();
//         user.RefreshToken = refreshToken;
//         user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//         _context.Users.Update(user);
//         await _context.SaveChangesAsync();
//         return refreshToken;
//     }

//     private string CreateToken(User user)
//     {
//         var claims = new List<Claim> 
//         {
//             new Claim(ClaimTypes.Name, user.UserName),
//             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//         };

//         var userGruppen = _context.UserGruppenUsers
//             .Include(ugu => ugu.UserGruppen)
//             .Where(ugu => ugu.UserID == user.Id)
//             .Select(ugu => ugu.UserGruppen.Name)
//             .ToList();

//         foreach(var gruppenName in userGruppen)
//         {
//             claims.Add(new Claim(ClaimTypes.Role, gruppenName));
//         }

//         var key = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

//         var tokenDescriptor = new JwtSecurityToken(
//             issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
//             audience: _configuration.GetValue<string>("AppSettings:Audience"),
//             claims: claims,
//             expires: DateTime.UtcNow.AddDays(1),
//             signingCredentials: creds
//         );

//         return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//     }

// }
