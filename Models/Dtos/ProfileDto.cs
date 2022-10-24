namespace MovieCatalogApi.Models.Dtos;

public class ProfileDto
{
    public Guid id { get; set; }
    public string nickName { get; set; }
    public string email { get; set; }
    public string avatarLink { get; set; }
    public string name { get; set; }
    public DateTime birthDate { get; set; }
    public Gender gender { get; set; }
}