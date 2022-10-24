namespace MovieCatalogApi.Models.Dtos;

public class ReviewDto
{
    public Guid id { get; set; }
    public int rating { get; set; }
    public string reviewText { get; set; }
    public bool isAnonymous { get; set; }
    public DateTime createDateTime { get; set; }
    public UserShortDto author { get; set; }
}