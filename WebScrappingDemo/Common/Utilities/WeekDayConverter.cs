using WebScrappingDemo.Common.Constants;

namespace WebScrappingDemo.Common.Utilities;

public static class WeekDayConverter
{
    private static readonly Dictionary<string, int> DaysOfWeek = new Dictionary<string, int>()
    {
        { OutageScheduleConstants.WeekDays.MONDAY, 1 },
        { OutageScheduleConstants.WeekDays.TUESDAY, 2 },
        { OutageScheduleConstants.WeekDays.WEDNESDAY, 3 },
        { OutageScheduleConstants.WeekDays.THURSDAY, 4 },
        { OutageScheduleConstants.WeekDays.FRIDAY, 5 },
        { OutageScheduleConstants.WeekDays.SATURDAY, 6 },
        { OutageScheduleConstants.WeekDays.SUNDAY, 7 }
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
