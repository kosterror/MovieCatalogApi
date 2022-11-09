namespace MovieCatalogApi.Services;

public class LoggerService : ILoggerService
{
    private const string FileDirectory = "./Logs/";
    
    /*
     * Оболочка для Log, к сообщению добавится время и статус
     */
    public async Task LogInfo(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);                      
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |INF| {message}");          
    }
    
    /*
     * Оболочка для Log, к сообщению добавится время и статус
     */
    public async Task LogError(string message)
    {
        var path = GetPathFile();
        CreateLogFile(path);
        await Log(path, $"{DateTime.UtcNow.ToLongTimeString()} |ERR| {message}");
    }

    
    /*
     * Просто запись в файл с заданным путём и сообщением
     */
    private static async Task Log(string pathFile, string message)
    {
        await using var writer = new StreamWriter(pathFile, true);
        await writer.WriteLineAsync(message);
        writer.Close();
    }

    /*
     * Если необходимо, то создаст директорию для логов
     * Если файл с таким названием еще не существует, то создаст такой файл
     */
    private static void CreateLogFile(string pathFile)
    {
        if (!Directory.Exists(FileDirectory))
        {
            var directoryInfo = Directory.CreateDirectory(FileDirectory);
        }
        
        var fileInfo = new FileInfo(pathFile);
        
        if (fileInfo.Exists) return;
        
        var fileStream = File.Create(pathFile);
        fileStream.Close();
    }

    /*
     * Для каждой даты свой файлик с логами для более удобной навигации по ним
     * Название файла имеет следующий шаблон: "{дд.мм.гггг}.txt"
     */
    private static string GetPathFile()
    {
        return $"{FileDirectory}{DateTime.UtcNow.ToShortDateString()}.txt";
    }
}