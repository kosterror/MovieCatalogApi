using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogApi.Conmfigurations;
using MovieCatalogApi.Controllers;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto)
    {
        userRegisterDto.email = NormalizeAyttribute(userRegisterDto.email);
        userRegisterDto.userName = NormalizeAyttribute(userRegisterDto.userName);

        await CheckUniqueFields(userRegisterDto);

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

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        var loginCredentials = new LoginCredentials
        {
            password = userEntity.Password,
            username = userEntity.UserName
        };

        return await LoginUser(loginCredentials);
    }

    public async Task<TokenDto> LoginUser(LoginCredentials loginCredentials)
    {
        /*
         * логин должен быть в нижнем регистре и без пробелов
         * т.к. в бд мы его таким и храним
         */

        loginCredentials.username = NormalizeAyttribute(loginCredentials.username);

        var identity = await GetIdentity(loginCredentials.username, loginCredentials.password);

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

    public async Task<LoggedOutDto> LogoutUser(HttpContext httpContext)
    {
        var token = GetToken(httpContext.Request.Headers);

        var handler = new JwtSecurityTokenHandler();
        var expiredDate = handler.ReadJwtToken(token).ValidTo;

        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiredDate = expiredDate
        };

        await _context.Tokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();


        var result = new LoggedOutDto()
        {
            token = token,
            message = "Logged out"
        };
        return result;
    }

    private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.UserName == userName && x.Password == password)
            .FirstOrDefaultAsync();

        if (userEntity == null)
        {
            throw new WrongLoginCredentialsException("Login failed");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userEntity.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, userEntity.IsAdmin ? "Admin" : "User")
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        return claimsIdentity;
    }

    private static string GetToken(IHeaderDictionary headerDictionary)
    {
        var requestHeaders = new Dictionary<string, string>();

        foreach (var header in headerDictionary)
        {
            requestHeaders.Add(header.Key, header.Value);
        }

        var authorizationString = requestHeaders["Authorization"];


        const string pattern = @"\S+\.\S+\.\S+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(authorizationString);

        if (matches.Count <= 0)
        {
            throw new CanNotGetTokenException("Can not get the token from headers");
        }

        return matches[0].Value;
    }

    private static string NormalizeAyttribute(string attribute)
    {
        var result = attribute.ToLower();
        result = result.TrimEnd();

        return result;
    }

    private async Task CheckUniqueFields(UserRegisterDto userRegisterDto)
    {
        var checkUniqueUserName = await _context
            .Users
            .Where(x => userRegisterDto.userName == x.UserName)
            .FirstOrDefaultAsync();

        if (checkUniqueUserName != null)
        {
            throw new UserAlreadyExistsException($"UserName '{userRegisterDto.userName}' is already taken");
        }


        var checkUniqueEmail = await _context
            .Users
            .Where(x => userRegisterDto.email == x.Email)
            .FirstOrDefaultAsync();

        if (checkUniqueEmail != null)
        {
            throw new UserAlreadyExistsException($"Email '{userRegisterDto.email}' is already taken");
        }
    }
}