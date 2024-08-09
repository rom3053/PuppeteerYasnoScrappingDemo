namespace WebScrappingDemo.Domain.Entities;

public sealed class OutageSchedule
{
    public List<OutageScheduleDay>? ScheduleDays { get; set; } = new List<OutageScheduleDay>();
}
