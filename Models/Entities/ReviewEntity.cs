using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    public virtual UserEntity User { get; set; }
    public virtual MovieEntity Movie { get; set; }
    public string ReviewText { get; set; }
    [Range(0, 10)]
    public int Rating { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreatedDateTime { get; set; }
}