namespace MovieCatalogApi.Models.Dtos;

public class UserRegisterDto
{
    public string userName { get; set; }
    public string name { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public DateTime birthDate { get; set; }
    public Gender gender { get; set; }
}