namespace MovieCatalogApi.Models.Dtos;

public class MovieElementDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string poster { get; set; }
    public int year { get; set; }
    public string country { get; set; }
    public List<GenreDto> genres { get; set; } = new();
    public List<ReviewShortDto> reviews { get; set; } = new();
}