using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IUserService
{
    ProfileDto GetProfile(string id);
    void UpdateProfile(ProfileDto newProfileDto, string id);
}