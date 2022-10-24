namespace MovieCatalogApi.Models.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    public UserEntity User { get; set; }
    public MovieEntity Movie { get; set; }
    public string ReviewText { get; set; }
    public int Rating { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreatedDateTime { get; set; }
}