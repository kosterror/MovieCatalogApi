using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    public TokenDto RegisterUser(UserRegisterDto userRegisterDto)
    {
        userRegisterDto.email = NormalizeAyttribute(userRegisterDto.email);
        userRegisterDto.userName = NormalizeAyttribute(userRegisterDto.userName);

        CheckUniqueFields(userRegisterDto);

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

    public TokenDto LoginUser(LoginCredentials loginCredentials)
    {
        //логин должен быть в одном регистре и без пробелов
        //т.к. в бд мы его таким и храним
        loginCredentials.username = NormalizeAyttribute(loginCredentials.username);

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

        var result = new TokenDto()
        {
            token = encodeJwt
        };

        return result;
    }

    public LoggedOutDto LogoutUser(IHeaderDictionary headerDictionary)
    {
        var token = _validateTokenService.GetToken(headerDictionary);

        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token
        };

        _context.Tokens.Add(tokenEntity);
        _context.SaveChanges();

        var result = new LoggedOutDto()
        {
            token = token,
            message = "Logged out"
        };

        return result;
    }

    private ClaimsIdentity GetIdentity(string userName, string password)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.UserName == userName && x.Password == password);

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

    private string NormalizeAyttribute(string attribute)
    {
        var result = attribute.ToLower();
        result = result.Replace(" ", "");

        return result;
    }

    private void CheckUniqueFields(UserRegisterDto userRegisterDto)
    {
        var checkUniqueEmail = _context.Users.FirstOrDefault(x => userRegisterDto.email == x.Email);

        if (checkUniqueEmail != null)
        {
            //TODO поменять тип исключения
            throw new Exception();
        }


        var checkUniqueUserName = _context.Users.FirstOrDefault(x => userRegisterDto.userName == x.UserName);

        if (checkUniqueEmail != null)
        {
            //TODO поменять тип исключения
            throw new Exception();
        }
    }
}