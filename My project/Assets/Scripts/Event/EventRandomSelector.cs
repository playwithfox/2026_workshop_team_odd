using System;
using System.Collections.Generic;

public static class EventRandomSelector
{
    public static List<EventData> PickEventsForDay(
        int day,
        List<EventData> allEvents,
        GameStats stats,
        ICollection<string> usedEventIds)
    {
        return PickEventsForDay(day, allEvents, stats, usedEventIds, true, new Random());
    }

    public static List<EventData> PickEventsForDay(
        int day,
        List<EventData> allEvents,
        GameStats stats,
        ICollection<string> usedEventIds,
        Random random)
    {
        return PickEventsForDay(day, allEvents, stats, usedEventIds, true, random);
    }

    public static List<EventData> PickEventsForDay(
        int day,
        List<EventData> allEvents,
        GameStats stats,
        ICollection<string> usedEventIds,
        bool requireConditions,
        Random random)
    {
        List<EventData> selectedEvents = new List<EventData>();
        if (allEvents == null || stats == null)
        {
            return selectedEvents;
        }

        if (usedEventIds == null)
        {
            usedEventIds = new List<string>();
        }

        random = random ?? new Random();

        int eventCount = EventDaySchedule.GetEventCount(day);
        List<EventData> candidates = GetCandidates(allEvents, stats, usedEventIds, requireConditions);

        while (selectedEvents.Count < eventCount && candidates.Count > 0)
        {
            int index = random.Next(candidates.Count);
            EventData selectedEvent = candidates[index];

            selectedEvents.Add(selectedEvent);
            usedEventIds.Add(selectedEvent.EventID);
            candidates.RemoveAt(index);
        }

        return selectedEvents;
    }

    private static List<EventData> GetCandidates(
        List<EventData> allEvents,
        GameStats stats,
        ICollection<string> usedEventIds,
        bool requireConditions)
    {
        List<EventData> candidates = new List<EventData>();
        foreach (EventData eventData in allEvents)
        {
            if (eventData == null || usedEventIds.Contains(eventData.EventID))
            {
                continue;
            }

            if (!requireConditions || eventData.IsAvailable(stats))
            {
                candidates.Add(eventData);
            }
        }

        return candidates;
    }
}
