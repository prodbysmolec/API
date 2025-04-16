using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities.Authentication;
using Infrastructure.Context;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenGenerator(
    IConfiguration configuration,
    IUserService userService,
    IUserGruppenService userGruppenService,
    IPermissionService permissionService
) : IJwtTokenGenerator
{
    private readonly IUserService _userService = userService;
    private readonly IUserGruppenService _userGruppenervice = userGruppenService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IPermissionService _permissionService = permissionService;

    public async Task<string> CreateAccessTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        // Benutzergruppen des Users abrufen
        var userGruppen = await _userGruppenervice.GetUserGroupNamesAsync(user.Id);

        if(!userGruppen.Any())
        {
            return null!;
        }

        // Gruppen als Claims hinzufügen
        foreach (string gruppenName in userGruppen)
        {
            claims.Add(new Claim(ClaimTypes.Role, gruppenName));
        }

        // Die Permissions des Users abrufen
        var permissionCodes = await _permissionService.GetUserPermissionCodesAsync(user.Id);

        // Permission-Codes als Claims hinzufügen
        foreach(var permissionCode in permissionCodes)
        {
            claims.Add(new Claim("permission", permissionCode));
        }

        var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _configuration["AppSettings:Issuer"],
            audience: _configuration["AppSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = await GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userService.UpdateAsync(user);
        return refreshToken;   
    }

    private async Task<string> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return await Task.FromResult(Convert.ToBase64String(randomNumber));
    }
}
