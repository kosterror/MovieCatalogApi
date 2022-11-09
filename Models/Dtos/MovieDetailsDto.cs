namespace MovieCatalogApi.Models.Dtos;

public class MovieDetailsDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string poster { get; set; }
    public int year { get; set; }
    public string country { get; set; }
    public List<GenreDto> genres { get; set; } = new();
    public List<ReviewDto> reviews { get; set; } = new();
    public int time { get; set; }
    public string? tagline { get; set; }
    public string? description { get; set; }
    public string director { get; set; }
    public int? budget { get; set; }
    public int? fees { get; set; }
    public int ageLimit { get; set; }
}