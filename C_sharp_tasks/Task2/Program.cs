namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = new("log.txt");
            log.Add(Logger.Severity.Critical, "Something happened");
            log.Add(Logger.Severity.Information, "Somewhere, IDK");
            log.Add(Logger.Severity.Trace, "Oh my...");
            log.Dispose();
        }
    }
}