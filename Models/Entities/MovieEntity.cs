using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Entities;

public class MovieEntity
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Poster { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public int Time { get; set; }
    [Required]
    public string Tagline { get; set; }
    [Required]
    public string Director { get; set; }
    [Required]
    public int Budget { get; set; }
    [Required]
    public int Fees { get; set; }
    [Required]
    public int AgeLimit { get; set; }
    public List<GenreEntity> Genres { get; set; }
    public List<UserEntity> LikedUsers { get; set; }
}