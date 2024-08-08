using WebScrappingDemo.Common.Dtos;

namespace WebScrappingDemo.Common.Mapping.DropdownOption;

public static class MappingToDtos
{
    public static List<DropdownOptionDto> MapToDto(this List<Domain.PuppeteerModels.OutageModels.DropdownOption> dropdownOptions)
    {
        return dropdownOptions.Select((x, index) => new DropdownOptionDto
        {
            Text = x.Text,
            Index = index,
        }).ToList();
    }
}
