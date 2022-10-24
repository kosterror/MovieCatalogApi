using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Dtos;

public class ReviewModifyDto
{
    public string reviewText { get; set; }
    [Range(0, 10)]
    public int rating { get; set; }
    public bool isAnonymous { get; set; }
}