using WebScrappingDemo.Common.Constants;

namespace WebScrappingDemo.Common.Utilities;

public static class WeekDayConverter
{
    private static readonly Dictionary<string, int> DaysOfWeek = new Dictionary<string, int>()
    {
        { OutageScheduleConstants.WeekDays.MONDAY, (int)DayOfWeek.Monday },
        { OutageScheduleConstants.WeekDays.TUESDAY, (int)DayOfWeek.Tuesday },
        { OutageScheduleConstants.WeekDays.WEDNESDAY, (int)DayOfWeek.Wednesday },
        { OutageScheduleConstants.WeekDays.THURSDAY, (int)DayOfWeek.Thursday },
        { OutageScheduleConstants.WeekDays.FRIDAY, (int)DayOfWeek.Friday },
        { OutageScheduleConstants.WeekDays.SATURDAY, (int)DayOfWeek.Saturday },
        { OutageScheduleConstants.WeekDays.SUNDAY, (int)DayOfWeek.Sunday }
    };

    public static int GetDayOfWeekInt(string dayName)
    {
        if (DaysOfWeek.TryGetValue(dayName, out int dayOfWeekInt))
        {
            return dayOfWeekInt;
        }
        else
        {
            return default;
        }
    }
}
