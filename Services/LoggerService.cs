namespace MovieCatalogApi.Services.Implementations;

public class LoggerService : ILoggerService
{
    private const string fileDirectory = "./Logs/";
    
    public async Task LogInfo(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);                      
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |INF| {message}");          
    }
    
    public async Task LogException(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |ERR| {message}");
    }


    //само логирование в файл
    private async Task Log(string pathFile, string message)
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

    private string GetPathFile()
    {
        return $"{fileDirectory}{DateTime.UtcNow.ToShortDateString()}.txt";
    }
}