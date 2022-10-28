using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogApi.Conmfigurations;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class AuthService : IAuthService
{
    private ApplicationDbContext _context;
    
    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    //TODO: поменять возвращаемый тип
    public async Task<IActionResult> RegisterUser(UserRegisterDto userRegisterDto)
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
            IsAdmin = false                         //в спеке не нашел, где этот атрибут играл бы важную роль
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

    //TODO: поменять возвращаемый тип
    public async Task<IActionResult> LoginUser(LoginCredentials loginCredentials)
    {
        var identity = await GetIdentity(loginCredentials.username, loginCredentials.password);

        var nowTime = DateTime.UtcNow;
        
        var now = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
            issuer: JwtConfigurations.Issuer,
            audience: JwtConfigurations.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(JwtConfigurations.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        
        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            token = encodeJwt
        };

        return new JsonResult(response);
    }

    //TODO: поменять возвращаемый тип
    public async Task LogoutUser(string token)
    {
        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token
        };

        await _context.Tokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();
    }
    
    private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
    {
        var userEntity = await _context.Users.Where(x => x.UserName == userName && x.Password == password)
            .FirstOrDefaultAsync();

        if (userEntity == null)
        {
            throw new ValidationException("Wrong username or password");
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