using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
    private enum EventPhase
    {
        Early,
        Mid,
        Late
    }

    [Header("Day")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image eventImage;

    [Header("Choices")]
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceNameTexts;
    [SerializeField] private TMP_Text[] choiceDescriptionTexts;
    [SerializeField] private EventCardIntroUI eventCardIntroUI;
    [SerializeField] private RectTransform reactionTargetButton;
    [SerializeField] private GameObject reactionPanel;
    [SerializeField] private ReactionPanelUI reactionPanelUI;
    [SerializeField] private float selectionFadeDuration = 0.8f;
    [SerializeField] private float reactionDelay = 0.5f;
    [SerializeField] private float choiceMoveDuration = 0.8f;
    [SerializeField] private Ease choiceMoveEase = Ease.InOutCubic;

    private EventListData eventListData;
    [SerializeField] private GameManager gameManager;
    private GameStats Stats => gameManager != null ? gameManager.Stats : null;

    private readonly List<string> usedEventIds = new List<string>();
    private Vector3[] choiceButtonStartPositions;
    private Sequence choiceTransitionSequence;
    private Sequence selectionFadeSequence;
    private SpriteRenderer eventCardSpriteRenderer;
    private Color eventCardSpriteColor = Color.white;
    private GameObject eventCardRootObject;
    private CanvasGroup eventCardCanvasGroup;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup descriptionCanvasGroup;
    private CanvasGroup eventImageCanvasGroup;

    private EventData currentEvent;

    private void Awake()
    {
        HideReactionPanel();
        CacheEventCardVisuals();
        ResolveEventCardIntroUI();
    }

    private void OnDisable()
    {
        if (selectionFadeSequence != null && selectionFadeSequence.IsActive())
        {
            selectionFadeSequence.Kill();
        }

        if (choiceTransitionSequence != null && choiceTransitionSequence.IsActive())
        {
            choiceTransitionSequence.Kill();
        }
    }

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("EventPanelUI: GameManager is not assigned.", this);
            return;
        }

        HideReactionPanel();

        TextAsset json = Resources.Load<TextAsset>("EventList");
        if (json == null)
        {
            Debug.LogError("EventPanelUI: EventList.json was not found in Resources.", this);
            return;
        }

        eventListData = JsonUtility.FromJson<EventListData>(json.text);
        ConfigureChoiceButtons();
        CacheChoiceButtonStartPositions();
        BeginDayForCurrentDay();

        if (dayText != null)
        {
            dayText.text = $"D - {gameManager.CurrentDay}";
        }

        EventData firstEvent = PickEventForCurrentPhase();
        if (firstEvent != null)
        {
            ShowEvent(firstEvent);
        }
        else
        {
            ShowEmptyEventState();
        }
    }

    public void RefreshCurrentDayEvent()
    {
        if (gameManager == null || eventListData == null)
        {
            return;
        }

        if (EventDaySchedule.CurrentDay != gameManager.CurrentDay)
        {
            BeginDayForCurrentDay();
        }

        ResetChoiceButtonPositions();

        if (dayText != null)
        {
            dayText.text = $"D - {gameManager.CurrentDay}";
        }

        EventData nextEvent = PickEventForCurrentPhase();
        if (nextEvent != null)
        {
            ShowEvent(nextEvent);
        }
        else
        {
            ShowEmptyEventState();
            EventDaySchedule.ForceFinishCurrentDay();
        }
    }

    public void ShowEvent(EventData eventData)
    {
        if (eventData == null)
        {
            ShowEmptyEventState();
            return;
        }

        currentEvent = eventData;

        if (eventCardRootObject != null)
        {
            eventCardRootObject.SetActive(true);
        }

        ResetChoiceButtonPositions();
        ResetEventCardVisualState();

        titleText.text = eventData.Title;
        descriptionText.text = eventData.Description;

        Sprite sprite = Resources.Load<Sprite>($"UI_Images/사건카드_목록/{eventData.ImageID}");
        eventImage.sprite = sprite;
        if (sprite == null)
        {
            Debug.LogWarning($"EventPanelUI: Event image not found for ImageID '{eventData.ImageID}'.", this);
        }

        SetupChoices(eventData);
        PlayEventCardIntro();
        StartCoroutine(EnableChoiceButtonsAfterIntro());
    }

    public void BeginDayForCurrentDay()
    {
        EventDaySchedule.BeginDay(gameManager.CurrentDay);
        usedEventIds.Clear();
    }

    private void ConfigureChoiceButtons()
    {
        if (choiceButtons == null)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            ConfigureChoiceButton(choiceButtons[i]);
        }
    }

    private void CacheChoiceButtonStartPositions()
    {
        if (choiceButtons == null)
        {
            return;
        }

        choiceButtonStartPositions = new Vector3[choiceButtons.Length];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            RectTransform rectTransform = choiceButtons[i] != null ? choiceButtons[i].GetComponent<RectTransform>() : null;
            choiceButtonStartPositions[i] = rectTransform != null ? rectTransform.position : Vector3.zero;
        }
    }

    public void ResetChoiceButtonPositions()
    {
        if (choiceButtons == null || choiceButtonStartPositions == null)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length && i < choiceButtonStartPositions.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null)
            {
                continue;
            }

            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                continue;
            }

            rectTransform.position = choiceButtonStartPositions[i];
        }
    }

    public void ResetChoiceButtonsForNextRound()
    {
        if (choiceButtons == null)
        {
            return;
        }

        if (selectionFadeSequence != null && selectionFadeSequence.IsActive())
        {
            selectionFadeSequence.Kill();
        }

        if (choiceTransitionSequence != null && choiceTransitionSequence.IsActive())
        {
            choiceTransitionSequence.Kill();
        }

        ResetChoiceButtonPositions();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null)
            {
                continue;
            }

            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);

            if (i < choiceNameTexts.Length && choiceNameTexts[i] != null)
            {
                choiceNameTexts[i].text = string.Empty;
            }

            if (i < choiceDescriptionTexts.Length && choiceDescriptionTexts[i] != null)
            {
                choiceDescriptionTexts[i].text = string.Empty;
            }
        }
    }

    private void ConfigureChoiceButton(Button button)
    {
        if (button == null)
        {
            return;
        }

        button.transition = Selectable.Transition.None;

        EventChoiceButtonHoverScale hoverScale = button.GetComponent<EventChoiceButtonHoverScale>();
        if (hoverScale == null)
        {
            hoverScale = button.gameObject.AddComponent<EventChoiceButtonHoverScale>();
        }

        hoverScale.Configure(button.GetComponent<RectTransform>(), 760f);

        CanvasGroup canvasGroup = GetOrAddCanvasGroup(button.gameObject);
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void SetupChoices(EventData eventData)
    {
        if (choiceButtons == null)
        {
            return;
        }

        HideChoiceButtons();

        for (int i = 0; i < eventData.ChoiceIDs.Count && i < choiceButtons.Length; i++)
        {
            ChoiceData choice = FindChoiceById(eventData.ChoiceIDs[i]);
            if (choice == null)
            {
                continue;
            }

            Button button = choiceButtons[i];
            ConfigureChoiceButton(button);

            button.gameObject.SetActive(true);
            button.interactable = false;

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(button.gameObject);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

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
            Button selectedButton = button;
            button.onClick.AddListener(() =>
            {
                OnChoiceSelected(selectedChoice, selectedButton);
            });
        }
    }

    private ChoiceData FindChoiceById(string choiceId)
    {
        if (eventListData == null || eventListData.choices == null)
        {
            return null;
        }

        foreach (ChoiceData choice in eventListData.choices)
        {
            if (choice != null && choice.ChoiceID == choiceId)
            {
                return choice;
            }
        }

        return null;
    }

    private EventData PickEventForCurrentPhase()
    {
        if (gameManager == null || eventListData == null || eventListData.events == null)
        {
            return null;
        }

        EventPhase currentPhase = GetCurrentPhase(gameManager.CurrentDay);
        List<EventData> filteredEvents = new List<EventData>();

        foreach (EventData eventData in eventListData.events)
        {
            if (eventData == null)
            {
                continue;
            }

            if (!MatchesCurrentPhase(eventData, currentPhase))
            {
                continue;
            }

            if (usedEventIds.Contains(eventData.EventID))
            {
                continue;
            }

            if (Stats != null && !eventData.IsAvailable(Stats))
            {
                continue;
            }

            filteredEvents.Add(eventData);
        }

        if (filteredEvents.Count == 0)
        {
            return null;
        }

        int selectedIndex = Random.Range(0, filteredEvents.Count);
        EventData selectedEvent = filteredEvents[selectedIndex];
        usedEventIds.Add(selectedEvent.EventID);
        return selectedEvent;
    }

    private static EventPhase GetCurrentPhase(int day)
    {
        if (day <= 3)
        {
            return EventPhase.Early;
        }

        if (day <= 5)
        {
            return EventPhase.Mid;
        }

        return EventPhase.Late;
    }

    private static bool MatchesCurrentPhase(EventData eventData, EventPhase currentPhase)
    {
        string phaseValue = GetPhaseValue(eventData);
        if (string.IsNullOrEmpty(phaseValue))
        {
            return true;
        }

        string normalized = phaseValue.Trim().ToUpperInvariant();
        return currentPhase switch
        {
            EventPhase.Early => normalized.Contains("EARLY") || normalized.Contains("전반"),
            EventPhase.Mid => normalized.Contains("MID") || normalized.Contains("중반"),
            EventPhase.Late => normalized.Contains("LATE") || normalized.Contains("후반"),
            _ => true
        };
    }

    private static string GetPhaseValue(EventData eventData)
    {
        if (eventData == null)
        {
            return string.Empty;
        }

        System.Type eventType = eventData.GetType();

        var phaseProperty = eventType.GetProperty("Phase");
        if (phaseProperty != null && phaseProperty.PropertyType == typeof(string))
        {
            return phaseProperty.GetValue(eventData) as string ?? string.Empty;
        }

        var phaseField = eventType.GetField("Phase");
        if (phaseField != null && phaseField.FieldType == typeof(string))
        {
            return phaseField.GetValue(eventData) as string ?? string.Empty;
        }

        return string.Empty;
    }

    private void OnChoiceSelected(ChoiceData choice, Button selectedButton)
    {
        PlayChoiceSelectionTransition(selectedButton, choice);
    }

    private void PlayChoiceSelectionTransition(Button selectedButton, ChoiceData choice)
    {
        if (selectedButton == null)
        {
            return;
        }

        EventChoiceButtonHoverScale hoverScale = selectedButton.GetComponent<EventChoiceButtonHoverScale>();
        if (hoverScale != null)
        {
            hoverScale.ResetVisualState();
        }

        if (selectionFadeSequence != null && selectionFadeSequence.IsActive())
        {
            selectionFadeSequence.Kill();
        }

        foreach (Button button in choiceButtons)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }

        selectionFadeSequence = DOTween.Sequence();

        FadeEventCard(selectionFadeSequence);
        FadeUnselectedChoiceButtons(selectionFadeSequence, selectedButton);
        selectionFadeSequence.AppendInterval(reactionDelay);

        selectionFadeSequence.OnComplete(() =>
        {
            HideFadedEventCard();
            HideUnselectedChoiceButtons(selectedButton);
            BeginReactionTransition(selectedButton, choice);
        });
    }

    private void ApplyChoiceResult(ChoiceData choice)
    {
        ChoiceEffectApplier.Apply(choice, Stats);
    }

    private void BeginReactionTransition(Button selectedButton, ChoiceData choice)
    {
        GameObject resolvedReactionPanel = ResolveReactionPanel();
        if (resolvedReactionPanel != null && !resolvedReactionPanel.activeSelf)
        {
            resolvedReactionPanel.SetActive(true);
        }

        ReactionPanelUI resolvedReactionPanelUI = ResolveReactionPanelUI();
        if (resolvedReactionPanelUI != null)
        {
            resolvedReactionPanelUI.PrepareForReactionDisplay();
        }

        if (reactionTargetButton == null)
        {
            reactionTargetButton = FindReactionTargetButton(resolvedReactionPanel);
        }

        RectTransform selectedRect = selectedButton.GetComponent<RectTransform>();
        if (selectedRect == null)
        {
            ApplyChoiceResult(choice);
            return;
        }

        if (choiceTransitionSequence != null && choiceTransitionSequence.IsActive())
        {
            choiceTransitionSequence.Kill();
        }

        Vector3 targetPosition = reactionTargetButton != null ? reactionTargetButton.position : selectedRect.position;
        Vector2 targetSize = reactionTargetButton != null ? reactionTargetButton.sizeDelta : selectedRect.sizeDelta;
        Vector2 startSize = selectedRect.sizeDelta;
        Vector3 targetScale = new Vector3(
            startSize.x == 0f ? 1f : targetSize.x / startSize.x,
            startSize.y == 0f ? 1f : targetSize.y / startSize.y,
            selectedRect.localScale.z
        );

        choiceTransitionSequence = DOTween.Sequence();
        choiceTransitionSequence.Join(selectedRect.DOMove(targetPosition, choiceMoveDuration).SetEase(choiceMoveEase));
        choiceTransitionSequence.Join(selectedRect.DOScale(targetScale, choiceMoveDuration).SetEase(choiceMoveEase));
        choiceTransitionSequence.OnComplete(() =>
        {
            if (reactionTargetButton != null)
            {
                selectedRect.position = reactionTargetButton.position;
                selectedRect.localScale = targetScale;
            }

            EventChoiceButtonHoverScale hoverScale = selectedButton.GetComponent<EventChoiceButtonHoverScale>();
            if (hoverScale != null)
            {
                hoverScale.SetInteractionEnabled(false);
            }

            resolvedReactionPanelUI = ResolveReactionPanelUI();
            if (resolvedReactionPanelUI != null)
            {
                resolvedReactionPanelUI.SetSelectedChoiceButton(selectedButton);
                resolvedReactionPanelUI.ShowReaction(choice);
            }

            ApplyChoiceResult(choice);
            StatIconDisplayUI.RefreshAllChanged();
            StartCoroutine(RefreshAfterDelay());
        });
    }

    private RectTransform FindReactionTargetButton(GameObject resolvedReactionPanel)
    {
        if (resolvedReactionPanel == null)
        {
            return null;
        }

        Transform target = resolvedReactionPanel.transform.Find("OptionButton_1");
        return target != null ? target.GetComponent<RectTransform>() : null;
    }

    private void HideReactionPanel()
    {
        GameObject target = ResolveReactionPanel();
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    private void CacheEventCardVisuals()
    {
        titleCanvasGroup = GetOrAddCanvasGroup(titleText != null ? titleText.gameObject : null);
        descriptionCanvasGroup = GetOrAddCanvasGroup(descriptionText != null ? descriptionText.gameObject : null);
        eventImageCanvasGroup = GetOrAddCanvasGroup(eventImage != null ? eventImage.gameObject : null);

        Transform eventCardRoot = null;
        if (titleText != null)
        {
            eventCardRoot = titleText.transform.parent;
        }

        if (eventCardRoot != null)
        {
            eventCardRootObject = eventCardRoot.gameObject;
            eventCardCanvasGroup = GetOrAddCanvasGroup(eventCardRootObject);
            eventCardSpriteRenderer = eventCardRoot.GetComponent<SpriteRenderer>();
            if (eventCardSpriteRenderer != null)
            {
                eventCardSpriteColor = eventCardSpriteRenderer.color;
            }
        }
    }

    private void ResetEventCardVisualState()
    {
        SetCanvasGroupAlpha(eventCardCanvasGroup, 1f);
        SetCanvasGroupAlpha(titleCanvasGroup, 1f);
        SetCanvasGroupAlpha(descriptionCanvasGroup, 1f);
        SetCanvasGroupAlpha(eventImageCanvasGroup, 1f);
        SetSpriteRendererAlpha(eventCardSpriteRenderer, 1f);
    }

    private void FadeEventCard(Sequence sequence)
    {
        AppendCanvasGroupFade(sequence, eventCardCanvasGroup, 0f, selectionFadeDuration);
        AppendCanvasGroupFade(sequence, titleCanvasGroup, 0f, selectionFadeDuration);
        AppendCanvasGroupFade(sequence, descriptionCanvasGroup, 0f, selectionFadeDuration);
        AppendCanvasGroupFade(sequence, eventImageCanvasGroup, 0f, selectionFadeDuration);
        AppendSpriteRendererFade(sequence, eventCardSpriteRenderer, 0f, selectionFadeDuration);
    }

    private void FadeUnselectedChoiceButtons(Sequence sequence, Button selectedButton)
    {
        if (choiceButtons == null)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null || button == selectedButton)
            {
                continue;
            }

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(button.gameObject);
            AppendCanvasGroupFade(sequence, canvasGroup, 0f, selectionFadeDuration);
        }
    }

    private void HideFadedEventCard()
    {
        SetCanvasGroupAlpha(eventCardCanvasGroup, 0f);
        SetCanvasGroupAlpha(titleCanvasGroup, 0f);
        SetCanvasGroupAlpha(descriptionCanvasGroup, 0f);
        SetCanvasGroupAlpha(eventImageCanvasGroup, 0f);
        SetSpriteRendererAlpha(eventCardSpriteRenderer, 0f);

        if (eventCardRootObject != null)
        {
            eventCardRootObject.SetActive(false);
        }
    }

    private void HideUnselectedChoiceButtons(Button selectedButton)
    {
        if (choiceButtons == null)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null || button == selectedButton)
            {
                continue;
            }

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(button.gameObject);
            SetCanvasGroupAlpha(canvasGroup, 0f);
            button.gameObject.SetActive(false);
        }
    }

    private static void AppendCanvasGroupFade(Sequence sequence, CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        if (sequence == null || canvasGroup == null)
        {
            return;
        }

        sequence.Join(canvasGroup.DOFade(targetAlpha, duration));
    }

    private static void AppendSpriteRendererFade(Sequence sequence, SpriteRenderer spriteRenderer, float targetAlpha, float duration)
    {
        if (sequence == null || spriteRenderer == null)
        {
            return;
        }

        sequence.Join(spriteRenderer.DOFade(targetAlpha, duration));
    }

    private static void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha)
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = alpha;
        canvasGroup.interactable = alpha > 0f;
        canvasGroup.blocksRaycasts = alpha > 0f;
    }

    private void SetSpriteRendererAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        Color color = eventCardSpriteColor;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private static CanvasGroup GetOrAddCanvasGroup(GameObject target)
    {
        if (target == null)
        {
            return null;
        }

        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = target.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    private GameObject ResolveReactionPanel()
    {
        if (reactionPanel != null)
        {
            return reactionPanel;
        }

        GameObject found = GameObject.Find("ReactionPanel");
        if (found != null)
        {
            reactionPanel = found;
        }

        return reactionPanel;
    }

    private ReactionPanelUI ResolveReactionPanelUI()
    {
        if (reactionPanelUI != null)
        {
            return reactionPanelUI;
        }

        GameObject resolvedReactionPanel = ResolveReactionPanel();
        if (resolvedReactionPanel == null)
        {
            return null;
        }

        reactionPanelUI = resolvedReactionPanel.GetComponent<ReactionPanelUI>();
        if (reactionPanelUI == null)
        {
            reactionPanelUI = resolvedReactionPanel.GetComponentInChildren<ReactionPanelUI>(true);
        }

        return reactionPanelUI;
    }

    private EventCardIntroUI ResolveEventCardIntroUI()
    {
        if (eventCardIntroUI != null)
        {
            return eventCardIntroUI;
        }

        if (eventCardRootObject != null)
        {
            eventCardIntroUI = eventCardRootObject.GetComponent<EventCardIntroUI>();
            if (eventCardIntroUI == null)
            {
                eventCardIntroUI = eventCardRootObject.GetComponentInChildren<EventCardIntroUI>(true);
            }
        }

        if (eventCardIntroUI == null)
        {
            eventCardIntroUI = GetComponentInChildren<EventCardIntroUI>(true);
        }

        return eventCardIntroUI;
    }

    private void PlayEventCardIntro()
    {
        EventCardIntroUI introUI = ResolveEventCardIntroUI();
        if (introUI == null)
        {
            Debug.LogWarning("EventPanelUI: EventCardIntroUI is not assigned.", this);
            return;
        }

        introUI.PrepareIntro();
        introUI.PlayIntro();
    }

    private IEnumerator EnableChoiceButtonsAfterIntro()
    {
        EventCardIntroUI introUI = ResolveEventCardIntroUI();
        if (introUI != null)
        {
            yield return new WaitForSeconds(introUI.GetIntroDuration());
        }
        else
        {
            yield return null;
        }

        if (choiceButtons == null)
        {
            yield break;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null || !button.gameObject.activeSelf)
            {
                continue;
            }

            button.interactable = true;

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(button.gameObject);
            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }

    private void HideChoiceButtons()
    {
        if (choiceButtons == null)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            if (button == null)
            {
                continue;
            }

            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);

            if (i < choiceNameTexts.Length && choiceNameTexts[i] != null)
            {
                choiceNameTexts[i].text = string.Empty;
            }

            if (i < choiceDescriptionTexts.Length && choiceDescriptionTexts[i] != null)
            {
                choiceDescriptionTexts[i].text = string.Empty;
            }
        }
    }

    private void ShowEmptyEventState()
    {
        currentEvent = null;
        HideChoiceButtons();

        if (eventCardRootObject != null)
        {
            eventCardRootObject.SetActive(true);
        }

        ResetEventCardVisualState();

        if (titleText != null)
        {
            titleText.text = "No event available";
        }

        if (descriptionText != null)
        {
            descriptionText.text = string.Empty;
        }

        if (eventImage != null)
        {
            eventImage.sprite = null;
        }
    }

    private IEnumerator RefreshAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        StatIconDisplayUI.RefreshAll();
    }
}
