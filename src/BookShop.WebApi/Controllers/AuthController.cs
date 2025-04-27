
using BookShop.Application.Features.User.Commands.Login;
using BookShop.Domain.Common;
using BookShop.Domain.Entities;
using BookShop.Domain.IRepositories;
using BookShop.WebApi.Services;
using Microsoft.AspNetCore.DataProtection;

namespace BookShop.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class AuthController : BaseController
{
    #region constructor

    private readonly JwtService _jwtService;
    private string _refreshTokenCookie = "refreshToken";
    private readonly IRefreshTokenRepository _userTokenRepository;
    private readonly IUserRepository _userRepository;
    public AuthController(IMediator mediator, JwtService jwtService, IRefreshTokenRepository userTokenRepository, IUserRepository userRepository) : base(mediator)
    {
        _jwtService = jwtService;
        _userTokenRepository = userTokenRepository;
        _userRepository = userRepository;
    }

    #endregion




    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        var result = await _mediator.Send(loginCommand);
        if (result.IsSuccess)
        {
            var userToken = new RefreshToken
            {
                ExpiredAt = DateTime.UtcNow.AddDays(3),
                Revoked = false,
                TokenValue = Guid.NewGuid().ToString(),
                UserId = result.Data!.Id,
            };
            Response.Cookies.Append(_refreshTokenCookie, userToken.TokenValue, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = userToken.ExpiredAt,
            });
            var token = _jwtService.GenerateToken(result.Data!.Id, result.Data.Username, result.Data.Role);
            return Ok(new { token });
        }
        return Unauthorized(result.Error);
    }




    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshTokenValue = Request.Cookies[_refreshTokenCookie];
        if (string.IsNullOrWhiteSpace(refreshTokenValue) == false)
        {
            RefreshToken? refreshToken = await _userTokenRepository.GetIfValid(refreshTokenValue, new RefreshTokenQueryOption { IncludeUser = true });
            if (refreshToken != null)
            {
                var userTokenRequirments = await _userRepository.GetUserTokenRequirements(refreshToken.UserId);
                if (userTokenRequirments != null)
                {
                    var accessToken = _jwtService.GenerateToken(userTokenRequirments.Value.id, userTokenRequirments.Value.username, userTokenRequirments.Value.role);
                    return Ok(new { accessToken });
                }
            }
        }
        return Unauthorized("Invalid or revoked refresh token");
    }





}
