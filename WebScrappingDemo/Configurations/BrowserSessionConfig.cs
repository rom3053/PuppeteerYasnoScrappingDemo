namespace WebScrappingDemo.Configurations;

public sealed class BrowserSessionConfig
{
    public int SessionLifeInMinutes { get; set; }

    public int SessionRecycleJobInSeconds { get; set; }
}
