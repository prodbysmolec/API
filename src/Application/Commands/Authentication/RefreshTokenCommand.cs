using System;
using Application.Authentication;
using Application.Interfaces;
using Application.Interfaces.UnitOfWork;
using Artikelsystem.Shared.DTOs.User.Request;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using FluentValidation;
using MediatR;

namespace Application.Commands.Authentication;

public class RefreshTokenCommand : RefreshTokenRequestDto, IRequest<Result<TokenResponseDto>>
{

}

public class RefreshTokenCommandValidation : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidation()
    {
        RuleFor(x => x.UserID)
            .NotEmpty()
            .WithMessage("UserID is required.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IAuthenticationService _authenticationService;
    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtTokenGenerator tokenGenerator,
        IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _tokenGenerator = tokenGenerator;
        _authenticationService = authenticationService;
    }
    public async Task<Result<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Validate the refresh token and generate new tokens
        var result = await _authenticationService.RefreshTokensAsync(request);
        if (result == null)
        {
            return Result<TokenResponseDto>.Failure(BaseError.NotFound("Invalid refresh token.", "The provided refresh token is invalid or expired."));
        }

        // Save the new refresh token to the database
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserID);
        if (user == null)
        {
            return Result<TokenResponseDto>.Failure(BaseError.NotFound("User not found.", "The user associated with the refresh token was not found."));
        }

        user.Value.RefreshToken = result.RefreshToken;
        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}
