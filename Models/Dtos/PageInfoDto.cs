namespace MovieCatalogApi.Models.Dtos;

public class PageInfoDto
{
    public int pageSize { get; set; }
    public int pageCount { get; set; }
    public int currentPage { get; set; }
}