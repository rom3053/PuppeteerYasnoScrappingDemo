namespace WebScrappingDemo.Configurations;

public sealed class YasnoScrappingConfig
{
    public static string ConfigName => "YasnoScrapping";

    public BrowserSessionConfig? BrowserSession {  get; set; }
}
