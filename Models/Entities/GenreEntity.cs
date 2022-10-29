namespace MovieCatalogApi.Models.Entities;

public class GenreEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<MovieEntity> Movies { get; set; }
}