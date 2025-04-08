using System;

namespace Artikelsystem.Shared.DTOs.User.Request;

public class RefreshTokenRequestDto
{
    public int UserID { get; set; }
    public required string RefreshToken { get; set; }
}
