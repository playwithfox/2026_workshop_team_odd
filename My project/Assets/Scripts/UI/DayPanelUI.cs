using TMPro;
using UnityEngine;

public class DayPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private StatIconDisplayUI statIconDisplayUI;

    [Header("Day")]
    [SerializeField] private TMP_Text dayText;

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
        if (statIconDisplayUI == null)
        {
            statIconDisplayUI = GetComponent<StatIconDisplayUI>();
        }

        if (statIconDisplayUI != null)
        {
            statIconDisplayUI.Refresh();
        }
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

        if (statIconDisplayUI == null)
        {
            statIconDisplayUI = GetComponent<StatIconDisplayUI>();
        }

        if (statIconDisplayUI == null)
        {
            Debug.LogError("DayPanelUI: StatIconDisplayUI is not assigned.", this);
            isValid = false;
        }

        return isValid;
    }
}
