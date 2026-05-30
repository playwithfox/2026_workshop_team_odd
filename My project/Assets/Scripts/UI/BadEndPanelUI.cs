using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadEndPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CallEnding callEnding;
    [SerializeField] private TitleScreenUI titleScreenUI;

    [Header("Panel Root")]
    [SerializeField] private GameObject badEndPanelRoot;

    [Header("Other Panels")]
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject reactionPanel;
    [SerializeField] private GameObject resultPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text badEndingTitleText;
    [SerializeField] private TMP_Text badEndingText;

    [Header("Background")]
    [SerializeField] private Image badEndingBackgroundImage;

    [Header("Zero Sprites")]
    [SerializeField] private Sprite userZeroSprite;
    [SerializeField] private Sprite publicZeroSprite;
    [SerializeField] private Sprite serverZeroSprite;
    [SerializeField] private Sprite devZeroSprite;
    [SerializeField] private Sprite budgetZeroSprite;

    [Header("Stat Icons")]
    [SerializeField] private Image userIcon;
    [SerializeField] private Image publicIcon;
    [SerializeField] private Image serverIcon;
    [SerializeField] private Image devIcon;
    [SerializeField] private Image budgetIcon;

    [Header("Navigation")]
    [SerializeField] private Button returnToTitleButton;

    private void Awake()
    {
        if (badEndPanelRoot == null)
        {
            badEndPanelRoot = gameObject;
        }

        if (returnToTitleButton != null)
        {
            returnToTitleButton.onClick.RemoveListener(HandleReturnToTitleClicked);
            returnToTitleButton.onClick.AddListener(HandleReturnToTitleClicked);
        }

        if (badEndPanelRoot != null)
        {
            badEndPanelRoot.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (returnToTitleButton != null)
        {
            returnToTitleButton.onClick.RemoveListener(HandleReturnToTitleClicked);
        }
    }

    public void ShowBadEnding()
    {
        if (badEndPanelRoot != null)
        {
            badEndPanelRoot.SetActive(true);
        }

        HideOtherPanels();
        HideOverlappingGameplayRoot();

        if (callEnding == null)
        {
            callEnding = FindObjectOfType<CallEnding>();
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (callEnding == null || gameManager == null)
        {
            Debug.LogError("BadEndPanelUI: Missing CallEnding or GameManager reference.", this);
            return;
        }

        if (!callEnding.TryGetBadEndingData(gameManager.Stats, out CallEnding.EndingData endingData))
        {
            Debug.LogWarning("BadEndPanelUI: No bad ending data matched the current stats.", this);
            return;
        }

        SetEndingText(endingData.Title, endingData.Body);
        SetBackgroundImage(endingData.ImageId);
        ShowOnlyBrokenStatIcon(endingData.StatType);
    }

    public void HandleReturnToTitleClicked()
    {
        if (badEndPanelRoot != null)
        {
            badEndPanelRoot.SetActive(false);
        }

        if (titleScreenUI == null)
        {
            titleScreenUI = FindObjectOfType<TitleScreenUI>();
        }

        if (titleScreenUI != null)
        {
            titleScreenUI.ShowTitleScreen();
        }
    }

    private void SetEndingText(string title, string body)
    {
        if (badEndingTitleText != null)
        {
            badEndingTitleText.text = title ?? string.Empty;
        }

        if (badEndingText != null)
        {
            badEndingText.text = body ?? string.Empty;
        }
    }

    private void SetBackgroundImage(string imageId)
    {
        if (badEndingBackgroundImage == null)
        {
            return;
        }

        Sprite sprite = Resources.Load<Sprite>($"Icon_Images/{imageId}");
        badEndingBackgroundImage.sprite = sprite;
    }

    private void ShowOnlyBrokenStatIcon(CallEnding.EndingStatType statType)
    {
        HideAllStatIcons();

        switch (statType)
        {
            case CallEnding.EndingStatType.User:
                SetIcon(userIcon, userZeroSprite);
                break;
            case CallEnding.EndingStatType.Public:
                SetIcon(publicIcon, publicZeroSprite);
                break;
            case CallEnding.EndingStatType.Server:
                SetIcon(serverIcon, serverZeroSprite);
                break;
            case CallEnding.EndingStatType.Dev:
                SetIcon(devIcon, devZeroSprite);
                break;
            case CallEnding.EndingStatType.Budget:
                SetIcon(budgetIcon, budgetZeroSprite);
                break;
        }
    }

    private void HideAllStatIcons()
    {
        SetIcon(userIcon, null);
        SetIcon(publicIcon, null);
        SetIcon(serverIcon, null);
        SetIcon(devIcon, null);
        SetIcon(budgetIcon, null);
    }

    private static void SetIcon(Image target, Sprite sprite)
    {
        if (target == null)
        {
            return;
        }

        target.sprite = sprite;
        target.enabled = sprite != null;
    }

    private void HideOtherPanels()
    {
        if (dayPanel != null)
        {
            dayPanel.SetActive(false);
        }

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        if (reactionPanel != null)
        {
            reactionPanel.SetActive(false);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    private void HideOverlappingGameplayRoot()
    {
        GameObject reactionRoot = GameObject.Find("ReactionPanel");
        if (reactionRoot != null)
        {
            reactionRoot.SetActive(false);
        }
    }
}
