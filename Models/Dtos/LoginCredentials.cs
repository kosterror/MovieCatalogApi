namespace MovieCatalogApi.Models.Dtos;

public class LoginCredentials
{
    //считаю, что делать здесь валидацию атрибутов - неправильное решение
    //т.к. во время авторизации пользователю не нужно думать о допустимом формате логина и пароля
    //т.е. забрутфорсить можно будет легче
    
    public string username { get; set; }
    public string password { get; set; }
}