using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogApi.Conmfigurations;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class AuthService : IAuthService
{
    private ApplicationDbContext _context;
    private IValidateTokenService _validateTokenService;

    public AuthService(ApplicationDbContext context, IValidateTokenService validateTokenService)
    {
        _context = context;
        _validateTokenService = validateTokenService;
    }

    public JsonResult RegisterUser(UserRegisterDto userRegisterDto)
    {
        //TODO: добавить валидацию

        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = userRegisterDto.userName,
            Name = userRegisterDto.name,
            Password = userRegisterDto.password,
            Email = userRegisterDto.email,
            BirthDate = userRegisterDto.birthDate,
            Gender = userRegisterDto.gender,
            IsAdmin = false, //в спеке не нашел, где этот атрибут играл бы важную роль
            Avatar = "none"
        };

        _context.Users.Add(userEntity);
        _context.SaveChanges();

        var loginCredentials = new LoginCredentials
        {
            password = userEntity.Password,
            username = userEntity.UserName
        };

        return LoginUser(loginCredentials);
    }

    public JsonResult LoginUser(LoginCredentials loginCredentials)
    {
        var identity = GetIdentity(loginCredentials.username, loginCredentials.password);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfigurations.Issuer,
            audience: JwtConfigurations.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(JwtConfigurations.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));


        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            token = encodeJwt
        };

        return new JsonResult(response);
    }

    public JsonResult LogoutUser(IHeaderDictionary headerDictionary)
    {
        var token = _validateTokenService.GetToken(headerDictionary);
        
        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token
        };

        _context.Tokens.Add(tokenEntity);
        _context.SaveChanges();

        var response = new
        {
            token = token,
            message = "Logged out"
        };

        return new JsonResult(response);
    }

    private ClaimsIdentity GetIdentity(string userName, string password)
    {
        var userEntity = _context.Users
            .FirstOrDefault(x => x.UserName == userName && x.Password == password);

        if (userEntity == null)
        {
            throw new WrongLoginCredentialsException("Login failed");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userEntity.UserName),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, userEntity.IsAdmin ? "Admin" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return claimsIdentity;
    }
}