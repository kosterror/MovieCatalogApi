namespace MovieCatalogApi.Models.Dtos;

public class MoviesPagedListDto
{
    public List<MovieElementDto> movies { get; set; }
    public PageInfoDto pageInfo { get; set; }
}