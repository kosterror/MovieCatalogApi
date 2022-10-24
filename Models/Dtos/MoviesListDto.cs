namespace MovieCatalogApi.Models.Dtos;

public class MoviesListDto
{
    public List<MovieElementDto> movies { get; set; } = new();
}