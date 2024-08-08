using PuppeteerSharp;

namespace WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

public class DropdownOption
{
    public IElementHandle Node { get; set; }

    public string? Text { get; set; }

    public async Task SelectAndClickAsync()
    {
        await this.Node.FocusAsync();
        await Task.Delay(1000);
        await this.Node.ClickAsync();
    }
}
