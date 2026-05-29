/* 
using System.Collections.Generic;
 using TMPro;
 using UnityEngine;
 using UnityEngine.UI;
  
 public class EventList : MonoBehaviour
 {
     [Header("Texts")]
     [SerializeField] private TMP_Text dayText;
     [SerializeField] private TMP_Text eventTitleText;
     [SerializeField] private TMP_Text eventDescriptionText;
     [SerializeField] private TMP_Text resultText;

     [Header("Choice Buttons")]
     [SerializeField] private Button[] choiceButtons;
     [SerializeField] private TMP_Text[] choiceButtonTexts;

     private EventListData eventListData;
     private GameStats stats = new GameStats();
     private List<string> usedEventIds = new List<string>();
     private List<EventData> todayEvents = new List<EventData>();

     private int currentDay = 1;
     private int currentEventIndex = 0;

     private void Start()
     {
         stats.InitializeRandom();
         LoadEventData();
         ShowDayEvents();
     }

     private void LoadEventData()
     {
         TextAsset json = Resources.Load<TextAsset>("EventList");
         eventListData = JsonUtility.FromJson<EventListData>(json.text);
     }

     private void ShowDayEvents()
     {
         dayText.text = $"{currentDay}일차";

         todayEvents = EventRandomSelector.PickEventsForDay(
             currentDay,
             eventListData.events,
             stats,
             usedEventIds
          );

          currentEventIndex = 0;
          ShowCurrentEvent();
      }

      private void ShowCurrentEvent()
      {
          resultText.text = "";

          if (todayEvents.Count == 0)
          {
              eventTitleText.text = "오늘 발생한 사건 없음";
              eventDescriptionText.text = "조건에 맞는 사건이 없습니다.";
              HideChoiceButtons();
              return;
          }

          EventData currentEvent = todayEvents[currentEventIndex];

          eventTitleText.text = currentEvent.Title;
          eventDescriptionText.text = currentEvent.Description;

          ShowChoices(currentEvent);
      }

      private void ShowChoices(EventData eventData)
      {
          HideChoiceButtons();

          for (int i = 0; i < eventData.ChoiceIDs.Count && i < choiceButtons.Length; i++)
          {
              ChoiceData choice = FindChoice(eventData.ChoiceIDs[i]);

              if (choice == null)
              {
                  continue;
              }

              int buttonIndex = i;
              choiceButtons[i].gameObject.SetActive(true);
              choiceButtonTexts[i].text = choice.ChoiceName;

              choiceButtons[i].onClick.RemoveAllListeners();
              choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(choice));
          }
      }

      private ChoiceData FindChoice(string choiceId)
      {
          return eventListData.choices.Find(choice => choice.ChoiceID == choiceId);
      }

      private void OnChoiceClicked(ChoiceData choice)
      {
          ChoiceResult result = ChoiceEffectApplier.Apply(choice, stats);

          resultText.text =
              $"{result.ResultComment}\n\n" +
              $"유저 {result.User} / 여론 {result.Public} / 서버 {result.Server} / 개발 {result.Dev} / 예산 {result.Budget}";

          HideChoiceButtons();
      }

      private void HideChoiceButtons()
      {
          foreach (Button button in choiceButtons)
          {
              button.gameObject.SetActive(false);
          }
      }
 }
*/