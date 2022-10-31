using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Entities;

public class GenreEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    public List<MovieEntity> Movies { get; set; }
}