using PuppeteerSharp;

namespace WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

public sealed class DropdownOption
{
    public IElementHandle Node { get; set; }

    public string? Text { get; set; }

    public async Task SelectAndClickAsync()
    {
        await this.Node.FocusAsync();
        await this.Node.ClickAsync();
        await Task.Delay(1000);
    }
}
