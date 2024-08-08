namespace WebScrappingDemo.Domain.Entities;

public class OutageSchedule
{
    public List<OutageScheduleDay>? ScheduleDays { get; set; } = new List<OutageScheduleDay>();
}
