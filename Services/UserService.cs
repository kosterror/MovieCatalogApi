using MovieCatalogApi.Controllers;
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

    public ProfileDto GetProfile(string userName)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.UserName == userName);
        
        /*
         * вероятность попасть сюда почти нулевая, ну мне так кажется 
         * т.к. аннотация [Authorize] не пустит невалидный токен,
         * а userName оттуда и достается
        */

        if (userEntity == null)
        {
            /*
             * если мы сюда попали, то будтобы беда с токеном, значит
             * нужно заставить послать нам другой, валидный токен
             * а для этого нужно заставить получить новый токен, поэтому ошибка с 401 кодом 
             */
            throw new PermissionDeniedException("User with this token not found");
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

    public void UpdateProfile(ProfileDto profileDto, string userName)
    {
        /*
         * Передаем логин, т.к. фронт может подсунуть нам не тот userId, так мы избегаем лишнюю валидацию
         * вообще по-хорошему отдельную dto создать с полями, которые мы изменяем, а не со всем подряд
         */

        var userEntityWithSameEmail = _context.Users.FirstOrDefault(x => x.Email == profileDto.email);
        
        if (userEntityWithSameEmail != null && userEntityWithSameEmail.UserName != userName)
        {
            throw new UserAlreadyExistsException("User with such email has already exist");
        }

        var userEntity = _context.Users.FirstOrDefault(x => x.UserName == userName);
        
        //вероятность попасть сюда почти нулевая, ну мне так кажется
        //т.к. аннотация [Authorize] не должна пустить невалидный токен
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User with this token not found");
        }

        //с остальными полями не работаю, т.к. они не могут на фронте изменяться
        userEntity.Email = profileDto.email;
        userEntity.Avatar = profileDto.avatarLink;
        userEntity.Name = profileDto.name;
        userEntity.BirthDate = profileDto.birthDate;
        userEntity.Gender = profileDto.gender;
        
        _context.SaveChanges();
    }
}