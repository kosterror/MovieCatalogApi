using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Dtos;

public class ProfileDto
{
    public Guid id { get; set; }
    [Required]
    [MinLength(5)]
    public string nickName { get; set; }
    [Required]
    [MinLength(6)]
    [MaxLength(32)]
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+")]
    public string email { get; set; }
    public string avatarLink { get; set; }
    [Required]
    public string name { get; set; }
    [Required]
    public DateTime birthDate { get; set; }
    [Required]
    public Gender gender { get; set; }
}