using WebScrappingDemo.Domain.Enums;

namespace WebScrappingDemo.Domain.Entities;

public class OutageHour
{
    public int Hour { get; set; }

    public OutageStatus Status { get; set; }
}
