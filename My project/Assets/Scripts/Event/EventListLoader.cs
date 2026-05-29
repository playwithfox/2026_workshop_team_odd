using System.Collections.Generic;
using UnityEngine;

public class EventListLoader
{
    private readonly EventListData eventListData;
    private readonly Dictionary<string, ChoiceData> choiceById = new Dictionary<string, ChoiceData>();

    public EventListLoader(EventListData eventListData)
    {
        this.eventListData = eventListData ?? new EventListData();
        BuildChoiceLookup();
    }

    public List<EventData> Events => eventListData.events;
    public List<ChoiceData> Choices => eventListData.choices;

    public static EventListLoader LoadFromResources(string resourcePath = "EventList")
    {
        TextAsset eventListAsset = Resources.Load<TextAsset>(resourcePath);
        if (eventListAsset == null)
        {
            Debug.LogError($"EventListLoader: Resources/{resourcePath}.json 파일을 찾을 수 없습니다.");
            return new EventListLoader(new EventListData());
        }

        EventListData loadedData = JsonUtility.FromJson<EventListData>(eventListAsset.text);
        if (loadedData == null)
        {
            Debug.LogError($"EventListLoader: Resources/{resourcePath}.json 파싱에 실패했습니다.");
            return new EventListLoader(new EventListData());
        }

        EnsureLists(loadedData);
        return new EventListLoader(loadedData);
    }

    public ChoiceData GetChoiceById(string choiceId)
    {
        if (string.IsNullOrEmpty(choiceId))
        {
            return null;
        }

        return choiceById.TryGetValue(choiceId, out ChoiceData choiceData) ? choiceData : null;
    }

    public List<ChoiceData> GetChoicesForEvent(EventData eventData)
    {
        List<ChoiceData> choices = new List<ChoiceData>();
        if (eventData == null || eventData.ChoiceIDs == null)
        {
            return choices;
        }

        foreach (string choiceId in eventData.ChoiceIDs)
        {
            ChoiceData choiceData = GetChoiceById(choiceId);
            if (choiceData != null)
            {
                choices.Add(choiceData);
            }
            else
            {
                Debug.LogWarning($"EventListLoader: 선택지 ID를 찾을 수 없습니다. ChoiceID={choiceId}");
            }
        }

        return choices;
    }

    private void BuildChoiceLookup()
    {
        choiceById.Clear();
        if (eventListData.choices == null)
        {
            eventListData.choices = new List<ChoiceData>();
            return;
        }

        foreach (ChoiceData choiceData in eventListData.choices)
        {
            if (choiceData == null || string.IsNullOrEmpty(choiceData.ChoiceID))
            {
                continue;
            }

            if (choiceById.ContainsKey(choiceData.ChoiceID))
            {
                Debug.LogWarning($"EventListLoader: 중복 선택지 ID가 있습니다. ChoiceID={choiceData.ChoiceID}");
                continue;
            }

            choiceById.Add(choiceData.ChoiceID, choiceData);
        }
    }

    private static void EnsureLists(EventListData data)
    {
        if (data.events == null)
        {
            data.events = new List<EventData>();
        }

        if (data.choices == null)
        {
            data.choices = new List<ChoiceData>();
        }
    }
}
