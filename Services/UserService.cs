﻿using MovieCatalogApi.Controllers;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public ProfileDto GetProfile(string id)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.Id.ToString() == id);

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User by with token not found");
        }

        var userProfile = new ProfileDto
        {
            id = userEntity.Id,
            nickName = userEntity.UserName,
            email = userEntity.Email,
            avatarLink = userEntity.Avatar,
            name = userEntity.Name,
            birthDate = userEntity.BirthDate,
            gender = userEntity.Gender
        };

        return userProfile;
    }

    public void UpdateProfile(ProfileDto newProfileDto, string id)
    {
        /*
         * Считаю, что использовать одну и ту же дто для получения информации о пользователи и её изменении - кринж
         * ВАЖНО! поскольку мне не ответили в дискорде предоставлять ли возможность изменять ID пользователя,
         * то этой возможности просто не будет. То есть данные в атрибуте ID будут просто игнорироваться
         */

        var user = _context.Users.FirstOrDefault(x => x.Id.ToString() == id);

        /*
        * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его Id из валидного токена
        * считаю, что это ошибка 401
        */
        if (user == null)
        {
            throw new PermissionDeniedException("User by with token not found");
        }

        //TODO сделать красивый возврат ошибок
        
        var userEntityWithSameEmail = _context.Users.FirstOrDefault(x => x.Email == newProfileDto.email);
        if (userEntityWithSameEmail != null && userEntityWithSameEmail.UserName != user.UserName)
        {
            throw new UserAlreadyExistsException("User with this email has already exists");
        }

        var userEntityWithSameUserName = _context.Users.FirstOrDefault(x => x.UserName == newProfileDto.nickName);
        if (userEntityWithSameUserName != null && user.UserName != userEntityWithSameUserName.UserName)
        {
            throw new UserAlreadyExistsException("User with this UserName already exists");
        }
        
        user.UserName = newProfileDto.nickName;
        user.Email = newProfileDto.email;
        user.Avatar = newProfileDto.avatarLink;
        user.Name = newProfileDto.name;
        user.BirthDate = newProfileDto.birthDate;
        user.Gender = newProfileDto.gender;
        
        _context.SaveChanges();
    }
}