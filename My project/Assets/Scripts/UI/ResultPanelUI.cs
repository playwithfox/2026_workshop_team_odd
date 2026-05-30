using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Result Text")]
    [SerializeField] private TMP_Text resultText;

    [Header("Panels")]
    [SerializeField] private GameObject resultPanelRoot;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private DayPanelUI dayPanelUI;
    [SerializeField] private EventPanelUI eventPanelUI;
    [SerializeField] private DayToEventTransitionUI dayToEventTransitionUI;
    [SerializeField] private BadEndPanelUI badEndPanelUI;
    [SerializeField] private GoodEndPanelUI goodEndPanelUI;

    [Header("Navigation")]
    [SerializeField] private Button nextButton;

    private bool isGameOver;

    private void Awake()
    {
        if (resultPanelRoot == null)
        {
            resultPanelRoot = gameObject;
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
            nextButton.onClick.AddListener(HandleNextButtonClicked);
        }

        ClearResultText();
    }

    private void OnDestroy()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
        }
    }

    public void ShowResult(ChoiceData choice)
    {
        if (resultPanelRoot != null && !resultPanelRoot.activeSelf)
        {
            resultPanelRoot.SetActive(true);
        }

        if (resultText == null)
        {
            resultText = GetComponentInChildren<TMP_Text>(true);
        }

        if (resultText == null)
        {
            Debug.LogError("ResultPanelUI: Result Text is not assigned.", this);
            return;
        }

        string summary = choice != null ? choice.result_summary : string.Empty;
        resultText.text = summary ?? string.Empty;

        StatIconDisplayUI.RefreshAll();

        CheckGameOverState();
    }

    public void ClearResultText()
    {
        if (resultText == null)
        {
            resultText = GetComponentInChildren<TMP_Text>(true);
        }

        if (resultText != null)
        {
            resultText.text = string.Empty;
        }
    }

    public void HandleNextButtonClicked()
    {
        if (isGameOver)
        {
            HideGameplayPanelsForBadEnding();

            if (badEndPanelUI == null)
            {
                badEndPanelUI = FindObjectOfType<BadEndPanelUI>();
            }

            if (badEndPanelUI != null)
            {
                badEndPanelUI.ShowBadEnding();
            }

            return;
        }

        if (EventDaySchedule.RemainingEvents > 0)
        {
            if (eventPanel != null)
            {
                eventPanel.SetActive(true);
            }

            if (resultPanelRoot != null)
            {
                resultPanelRoot.SetActive(false);
            }

            ResolveEventPanelUI()?.ResetChoiceButtonsForNextRound();
            ResolveEventPanelUI()?.RefreshCurrentDayEvent();
            return;
        }

        if (gameManager != null && gameManager.CurrentDay >= 7)
        {
            HideGameplayPanelsForGoodEnding();

            if (goodEndPanelUI == null)
            {
                goodEndPanelUI = FindObjectOfType<GoodEndPanelUI>();
            }

            if (goodEndPanelUI != null)
            {
                goodEndPanelUI.ShowGoodEnding();
            }

            return;
        }

        if (gameManager != null)
        {
            AdvanceToNextLoopDay();
            EventDaySchedule.BeginDay(gameManager.CurrentDay);
        }

        EventPanelUI resolvedEventPanelUI = ResolveEventPanelUI();
        resolvedEventPanelUI?.BeginDayForCurrentDay();
        resolvedEventPanelUI?.ResetChoiceButtonsForNextRound();

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        if (resultPanelRoot != null)
        {
            resultPanelRoot.SetActive(false);
        }

        if (dayPanel != null && !dayPanel.activeSelf)
        {
            dayPanel.SetActive(true);
        }

        ResolveDayPanelUI()?.Refresh();

        if (dayToEventTransitionUI != null)
        {
            dayToEventTransitionUI.ResetToStartPositions();
            dayToEventTransitionUI.BeginAutoTransition();
        }
    }

    private void AdvanceToNextLoopDay()
    {
        if (gameManager == null)
        {
            return;
        }

        int nextDay = gameManager.CurrentDay + 1;
        if (nextDay > 7)
        {
            nextDay = 1;
        }

        System.Reflection.FieldInfo currentDayField = typeof(GameManager).GetField(
            "currentDay",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        if (currentDayField == null)
        {
            Debug.LogError("ResultPanelUI: GameManager.currentDay could not be found.", this);
            return;
        }

        currentDayField.SetValue(gameManager, nextDay);
    }

    private void CheckGameOverState()
    {
        isGameOver = false;

        if (gameManager == null)
        {
            return;
        }

        GameStats stats = gameManager.Stats;
        if (stats == null)
        {
            return;
        }

        isGameOver =
            stats.User <= 0 ||
            stats.Public <= 0 ||
            stats.Server <= 0 ||
            stats.Dev <= 0 ||
            stats.Budget <= 0;
    }

    private DayPanelUI ResolveDayPanelUI()
    {
        if (dayPanelUI != null)
        {
            return dayPanelUI;
        }

        if (dayPanel != null)
        {
            dayPanelUI = dayPanel.GetComponent<DayPanelUI>();
            if (dayPanelUI == null)
            {
                dayPanelUI = dayPanel.GetComponentInChildren<DayPanelUI>(true);
            }
        }

        return dayPanelUI;
    }

    private EventPanelUI ResolveEventPanelUI()
    {
        if (eventPanelUI != null)
        {
            return eventPanelUI;
        }

        if (eventPanel != null)
        {
            eventPanelUI = eventPanel.GetComponent<EventPanelUI>();
            if (eventPanelUI == null)
            {
                eventPanelUI = eventPanel.GetComponentInChildren<EventPanelUI>(true);
            }
        }

        return eventPanelUI;
    }

    private void HideGameplayPanelsForBadEnding()
    {
        if (resultPanelRoot != null)
        {
            resultPanelRoot.SetActive(false);
        }

        if (dayPanel != null)
        {
            dayPanel.SetActive(false);
        }

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        GameObject reactionPanel = GameObject.Find("ReactionPanel");
        if (reactionPanel != null)
        {
            reactionPanel.SetActive(false);
        }
    }

    private void HideGameplayPanelsForGoodEnding()
    {
        HideGameplayPanelsForBadEnding();
    }
}
