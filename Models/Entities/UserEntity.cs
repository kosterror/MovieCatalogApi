namespace MovieCatalogApi.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public Gender Gender { get; set; }
    public IEnumerable<MovieEntity> FavoriteMovies { get; set; }
}