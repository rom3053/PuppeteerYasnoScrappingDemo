using WebScrappingDemo.Domain.Enums;

namespace WebScrappingDemo.Common.Dtos;

public class SelectedDropdownOptionDto : DropdownOptionDto
{
    public SelectedOutageInputType SelectedOutageInputType { get; set; }
}
