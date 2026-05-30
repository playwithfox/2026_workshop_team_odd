public static class EventDaySchedule
{
    private static int currentDay = 1;
    private static int remainingEvents = 1;

    public static int CurrentDay => currentDay;
    public static int RemainingEvents => remainingEvents;

    public static void BeginDay(int day)
    {
        currentDay = day;
        remainingEvents = GetEventCountForDay(day);
    }

    public static void ConsumeOne()
    {
        if (remainingEvents > 0)
        {
            remainingEvents--;
        }
    }

    public static void ForceFinishCurrentDay()
    {
        remainingEvents = 0;
    }

    public static int GetEventCountForDay(int day)
    {
        if (day <= 2)
        {
            return 1;
        }

        if (day == 3)
        {
            return 2;
        }

        if (day == 4)
        {
            return 1;
        }

        if (day == 5 || day == 6)
        {
            return 2;
        }

        return 3;
    }
}
