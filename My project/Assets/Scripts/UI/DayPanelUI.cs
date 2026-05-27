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

    [Header("Stat Values")]
    [SerializeField] private TMP_Text userValueText;
    [SerializeField] private TMP_Text publicValueText;
    [SerializeField] private TMP_Text serverValueText;
    [SerializeField] private TMP_Text devValueText;
    [SerializeField] private TMP_Text budgetValueText;

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
        if (gameManager == null)
        {
            return;
        }

        GameStats stats = gameManager.Stats;

        dayText.text = $"D - {gameManager.CurrentDay}";

        userIcon.sprite = userSprite;
        publicIcon.sprite = publicSprite;
        serverIcon.sprite = serverSprite;
        devIcon.sprite = devSprite;
        budgetIcon.sprite = budgetSprite;

        userValueText.text = stats.User.ToString();
        publicValueText.text = stats.Public.ToString();
        serverValueText.text = stats.Server.ToString();
        devValueText.text = stats.Dev.ToString();
        budgetValueText.text = stats.Budget.ToString();
    }
}