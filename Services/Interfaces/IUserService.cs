using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IUserService
{
    ProfileDto GetProfile(string nickname);
    void UpdateProfile(ProfileDto profileDto, string userName);
}