// 시작 화면 UI 작동
using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Panels")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject eventPanel;

    [Header("Transitions")]
    [SerializeField] private DayToEventTransitionUI dayToEventTransitionUI;

    private void Start()
    {
        ValidateReferences();

        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }

        if (dayPanel != null)
        {
            dayPanel.SetActive(false);
        }

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        if (dayToEventTransitionUI != null)
        {
            dayToEventTransitionUI.CancelAutoTransition();
        }
    }

    private void ValidateReferences()
    {
        if (gameManager == null)
        {
            Debug.LogError("TitleScreenUI: GameManager is not assigned.", this);
        }

        if (titlePanel == null)
        {
            Debug.LogError("TitleScreenUI: Title Panel is not assigned.", this);
        }

        if (dayPanel == null)
        {
            Debug.LogError("TitleScreenUI: Day Panel is not assigned.", this);
        }

        if (eventPanel == null)
        {
            Debug.LogError("TitleScreenUI: Event Panel is not assigned.", this);
        }

        if (dayToEventTransitionUI == null)
        {
            Debug.LogError("TitleScreenUI: Day To Event Transition UI is not assigned.", this);
        }
    }

    public void StartGame()
    {
        Debug.Log("TitleScreenUI: Start button clicked.");

        if (gameManager == null)
        {
            Debug.LogError("TitleScreenUI: GameManager is not assigned.", this);
            return;
        }

        gameManager.StartGame();

        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        if (dayPanel != null)
        {
            dayPanel.SetActive(true);
        }

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        if (dayToEventTransitionUI != null)
        {
            dayToEventTransitionUI.BeginAutoTransition();
        }
    }

    public void ExitGame()
    {
        Debug.Log("TitleScreenUI: Exit button clicked.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
