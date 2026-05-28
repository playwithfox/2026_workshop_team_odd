using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Day")]
    [SerializeField] private TMP_Text dayText;

    [Header("Stat Icons")]
    [SerializeField] private Image userIcon;
    [SerializeField] private Image publicIcon;
    [SerializeField] private Image serverIcon;
    [SerializeField] private Image devIcon;
    [SerializeField] private Image budgetIcon;

    [Header("Icon Sprites")]
    [SerializeField] private Sprite userSprite;
    [SerializeField] private Sprite publicSprite;
    [SerializeField] private Sprite serverSprite;
    [SerializeField] private Sprite devSprite;
    [SerializeField] private Sprite budgetSprite;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!ValidateReferences())
        {
            return;
        }

        dayText.text = $"D - {gameManager.CurrentDay}";

        userIcon.sprite = userSprite;
        publicIcon.sprite = publicSprite;
        serverIcon.sprite = serverSprite;
        devIcon.sprite = devSprite;
        budgetIcon.sprite = budgetSprite;

    }

    private bool ValidateReferences()
    {
        bool isValid = true;

        if (gameManager == null)
        {
            Debug.LogError("DayPanelUI: GameManager is not assigned.", this);
            isValid = false;
        }

        if (dayText == null)
        {
            Debug.LogError("DayPanelUI: Day Text is not assigned.", this);
            isValid = false;
        }

        if (userIcon == null)
        {
            Debug.LogError("DayPanelUI: User Icon is not assigned.", this);
            isValid = false;
        }

        if (publicIcon == null)
        {
            Debug.LogError("DayPanelUI: Public Icon is not assigned.", this);
            isValid = false;
        }

        if (serverIcon == null)
        {
            Debug.LogError("DayPanelUI: Server Icon is not assigned.", this);
            isValid = false;
        }

        if (devIcon == null)
        {
            Debug.LogError("DayPanelUI: Dev Icon is not assigned.", this);
            isValid = false;
        }

        if (budgetIcon == null)
        {
            Debug.LogError("DayPanelUI: Budget Icon is not assigned.", this);
            isValid = false;
        }

        return isValid;
    }
}
