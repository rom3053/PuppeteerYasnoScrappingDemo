using WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

namespace WebScrappingDemo.Common.Exstensions;

public static class ListExtensions
{
    public static async Task SelectByIndexAndClickAsync(this List<DropdownOption> list, int index)
    {
        //DoTo index validation
        await list[index].SelectAndClickAsync();
    }
}
