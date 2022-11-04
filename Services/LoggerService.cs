namespace MovieCatalogApi.Services.Implementations;

//обязательно должен быть scoped!
public class LoggerService : ILoggerService
{
    public IServiceProvider ServiceProvider { get; }
    private const string fileDirectory = "./Logs/";
    private string pathFile;
    private string time;

    public LoggerService()
    {
        var shortDate = DateTime.UtcNow.ToShortDateString();
        pathFile = $"{fileDirectory}{shortDate}.txt";
        time = DateTime.UtcNow.ToLongTimeString();
    }

    public async Task LogInfo(string message)
    {
        CreateLogFile(pathFile);                      
        await Log($"{time} |INF| {message}");          
    }
    
    public async Task LogException(string message)
    {
        CreateLogFile(pathFile);
        await Log($"{time} |ERR| {message}");
    }


    //само логирование в файл
    private async Task Log(string message)
    {
        await using var writer = new StreamWriter(pathFile, true);
        await writer.WriteLineAsync(message);
        writer.Close();
    }

    //создаем файл, если он не создан
    private void CreateLogFile(string pathFile)
    {
        var fileInfo = new FileInfo(pathFile);
        
        if (fileInfo.Exists) return;
        
        var fileStream = File.Create(pathFile);
        fileStream.Close();
    }
}