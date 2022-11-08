using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IUserService
{
    Task<ProfileDto> GetProfile(string id);
    Task UpdateProfile(ProfileDto newProfileDto, string id);
}