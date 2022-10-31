using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IUserService
{
    ProfileDto GetUserProfile(string nickname);
}