using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
	[Header("Day")]
	[SerializeField] private TMP_Text dayText;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image eventImage;

	[Header("Choices")]
    [SerializeField] private Button[] choiceButtons;
	[SerializeField] private TMP_Text[] choiceNameTexts;
	[SerializeField] private TMP_Text[] choiceDescriptionTexts;
    [SerializeField] private TMP_Text resultCommentText;

    private EventListData eventListData;
	[SerializeField] private GameManager gameManager;
	private GameStats Stats => gameManager.Stats;
    private List<string> usedEventIds = new List<string>();

	private EventData currentEvent;

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("EventPanelUI: GameManager is not assigned.", this);
            return;
        }

        TextAsset json = Resources.Load<TextAsset>("EventList");
        if (json == null)
        {
            Debug.LogError("EventPanelUI: EventList.json was not found in Resources.", this);
            return;
        }

        eventListData = JsonUtility.FromJson<EventListData>(json.text);

		if (dayText != null)
		{
			dayText.text = $"D - {gameManager.CurrentDay}";
		}

        List<EventData> dayOneEvents = EventRandomSelector.PickEventsForDay(
            gameManager.CurrentDay,
            eventListData.events,
            Stats,
            usedEventIds
        );

        if (dayOneEvents.Count > 0)
        {
            ShowEvent(dayOneEvents[0]);
        }
        else
        {
            titleText.text = "오늘 발생한 사건 없음";
            descriptionText.text = "조건에 맞는 사건이 없습니다.";
            eventImage.sprite = null;
			HideChoiceButtons();
        }
    }

    public void ShowEvent(EventData eventData)
    {
		currentEvent = eventData;

        titleText.text = eventData.Title;
        descriptionText.text = eventData.Description;

        Sprite sprite = Resources.Load<Sprite>("UI_Images/사건카드_목록/" + eventData.ImageID);
        eventImage.sprite = sprite;

		if (resultCommentText != null)
        {
            resultCommentText.text = "";
        }

        SetupChoices(eventData);
    }

	private void SetupChoices(EventData eventData)
    {
        HideChoiceButtons();

        for (int i = 0; i < eventData.ChoiceIDs.Count && i < choiceButtons.Length; i++)
        {
            ChoiceData choice = FindChoiceById(eventData.ChoiceIDs[i]);

            if (choice == null)
            {
                continue;
            }

            Button button = choiceButtons[i];

            //TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            button.gameObject.SetActive(true);
            button.interactable = true;

            if (i < choiceNameTexts.Length && choiceNameTexts[i] != null)
            {
                choiceNameTexts[i].text = choice.ChoiceName;
            }

			if (i < choiceDescriptionTexts.Length && choiceDescriptionTexts[i] != null)
			{
				choiceDescriptionTexts[i].text = choice.Description;
			}

            button.onClick.RemoveAllListeners();

            ChoiceData selectedChoice = choice;
            button.onClick.AddListener(() =>
            {
                OnChoiceSelected(selectedChoice);
            });
        }
    }

    private ChoiceData FindChoiceById(string choiceId)
    {
        foreach (ChoiceData choice in eventListData.choices)
        {
            if (choice.ChoiceID == choiceId)
            {
                return choice;
            }
        }

        return null;
    }

    private void OnChoiceSelected(ChoiceData choice)
    {
        ChoiceResult result = ChoiceEffectApplier.Apply(choice, Stats);

        if (resultCommentText != null && result != null)
        {
            resultCommentText.text = result.ResultComment;
        }

        foreach (Button button in choiceButtons)
        {
            button.interactable = false;
        }
    }

    private void HideChoiceButtons()
    {
    	for (int i = 0; i < choiceButtons.Length; i++)
      	{
       		choiceButtons[i].onClick.RemoveAllListeners();
      	    choiceButtons[i].gameObject.SetActive(false);
	
       	  	if (i < choiceNameTexts.Length && choiceNameTexts[i] != null)
         	{
              	choiceNameTexts[i].text = "";
          	}

          	if (i < choiceDescriptionTexts.Length && choiceDescriptionTexts[i] != null)
          	{
              	choiceDescriptionTexts[i].text = "";
          	}
      	}
    }
}
