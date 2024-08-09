namespace WebScrappingDemo.Domain.Entities;

public sealed class OutageScheduleDay
{
    public OutageScheduleDay(string dayTitle, int numberWeekDay)
    {
        DayTitle = dayTitle;
        NumberWeekDay = numberWeekDay;
        OutageHours = new List<OutageHour>(24);
        for (int i = 0; i < 24; i++)
        {
            OutageHours.Add(new OutageHour { Hour = i });
        }
    }

    public string DayTitle { get; set; }

    public int NumberWeekDay { get; set; }

    public List<OutageHour> OutageHours { get; set; }
}
