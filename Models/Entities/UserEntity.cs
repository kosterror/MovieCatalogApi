using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(5)]
    public string UserName { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    [MinLength(6)]
    [MaxLength(16)]
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+")]
    public string Email { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
    [Required]
    public Gender Gender { get; set; }
    public string Avatar { get; set; }
    public List<MovieEntity> FavoriteMovies { get; set; } = new();
}