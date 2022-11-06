namespace MovieCatalogApi.Services;

public class LoggerService : ILoggerService
{
    private const string FileDirectory = "./Logs/";
    
    public async Task LogInfo(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);                      
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |INF| {message}");          
    }
    
    public async Task LogError(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |ERR| {message}");
    }

    
    private static async Task Log(string pathFile, string message)
    {
        await using var writer = new StreamWriter(pathFile, true);
        await writer.WriteLineAsync(message);
        writer.Close();
    }

    private static void CreateLogFile(string pathFile)
    {
        var fileInfo = new FileInfo(pathFile);
        
        if (fileInfo.Exists) return;
        
        var fileStream = File.Create(pathFile);
        fileStream.Close();
    }

    private static string GetPathFile()
    {
        return $"{FileDirectory}{DateTime.UtcNow.ToShortDateString()}.txt";
    }
}