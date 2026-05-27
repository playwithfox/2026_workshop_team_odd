using System;
using System.Collections.Generic;

public class EventGameSession
{
    private readonly EventListLoader eventListLoader;
    private readonly HashSet<string> usedEventIds = new HashSet<string>();
    private readonly Random random;

    public GameStats Stats { get; private set; }
    public int CurrentDay { get; private set; }

    public EventGameSession(EventListLoader eventListLoader, GameStats initialStats = null, Random random = null)
    {
        this.eventListLoader = eventListLoader ?? new EventListLoader(new EventListData());
        this.random = random ?? new Random();
        StartNewGame(initialStats);
    }

    public static EventGameSession CreateFromResources(string resourcePath = "EventList")
    {
        return new EventGameSession(EventListLoader.LoadFromResources(resourcePath));
    }

    public void StartNewGame(GameStats initialStats = null)
    {
        Stats = initialStats ?? new GameStats();
        CurrentDay = 1;
        usedEventIds.Clear();
    }

    public List<EventData> PickEventsForCurrentDay()
    {
        return PickEventsForDay(CurrentDay);
    }

    public List<EventData> PickEventsForDay(int day)
    {
        CurrentDay = day;

        // MVP 구조: 사건별 등장 조건 없이, 일자별 개수만큼 아직 안 나온 사건에서 랜덤 선택.
        return EventRandomSelector.PickEventsForDay(
            day,
            eventListLoader.Events,
            Stats,
            usedEventIds,
            false,
            random
        );
    }

    public List<ChoiceData> GetChoicesForEvent(EventData eventData)
    {
        return eventListLoader.GetChoicesForEvent(eventData);
    }

    public ChoiceData GetChoiceById(string choiceId)
    {
        return eventListLoader.GetChoiceById(choiceId);
    }

    public ChoiceResult ApplyChoice(ChoiceData choiceData)
    {
        return ChoiceEffectApplier.Apply(choiceData, Stats);
    }

    public ChoiceResult ApplyChoiceById(string choiceId)
    {
        ChoiceData choiceData = GetChoiceById(choiceId);
        return ApplyChoice(choiceData);
    }

    public void GoToNextDay()
    {
        CurrentDay++;
    }

    public bool IsAllScheduledEventsUsed()
    {
        return usedEventIds.Count >= eventListLoader.Events.Count;
    }
}
