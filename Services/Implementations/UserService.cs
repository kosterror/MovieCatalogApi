using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services.Implementations;

public class UserService : IUserService
{
    private ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public ProfileDto GetUserProfile(string userName)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.UserName == userName);

        if (userEntity == null)
        {
            throw new BadRequestException("User with this UserName not found");
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
}