using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Panels")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject reactionPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject badEndPanel;
    [SerializeField] private GameObject goodEndPanel;

    [Header("Transitions")]
    [SerializeField] private DayToEventTransitionUI dayToEventTransitionUI;

    private void Start()
    {
        ValidateReferences();

        if (gameManager != null)
        {
            gameManager.ResetGameState();
        }

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

        HideReactionPanel();
        HideResultPanel();
        HideBadEndPanel();
        HideGoodEndPanel();

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

        HideReactionPanel();
        HideResultPanel();
        HideBadEndPanel();
        HideGoodEndPanel();

        if (dayToEventTransitionUI != null)
        {
            dayToEventTransitionUI.BeginAutoTransition();
        }
    }

    private void HideReactionPanel()
    {
        GameObject target = ResolveReactionPanel();
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    private void HideResultPanel()
    {
        GameObject target = ResolveResultPanel();
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    private void HideBadEndPanel()
    {
        GameObject target = ResolveBadEndPanel();
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    private void HideGoodEndPanel()
    {
        GameObject target = ResolveGoodEndPanel();
        if (target != null)
        {
            target.SetActive(false);
        }
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

    private GameObject ResolveResultPanel()
    {
        if (resultPanel != null)
        {
            return resultPanel;
        }

        GameObject found = GameObject.Find("ResultPanel");
        if (found != null)
        {
            resultPanel = found;
        }

        return resultPanel;
    }

    private GameObject ResolveBadEndPanel()
    {
        if (badEndPanel != null)
        {
            return badEndPanel;
        }

        GameObject found = GameObject.Find("BadEndPanel");
        if (found != null)
        {
            badEndPanel = found;
        }

        return badEndPanel;
    }

    private GameObject ResolveGoodEndPanel()
    {
        if (goodEndPanel != null)
        {
            return goodEndPanel;
        }

        GameObject found = GameObject.Find("GoodEndPanel");
        if (found != null)
        {
            goodEndPanel = found;
        }

        return goodEndPanel;
    }

    public void ShowTitleScreen()
    {
        if (gameManager != null)
        {
            gameManager.ResetGameState();
        }

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

        HideReactionPanel();
        HideResultPanel();
        HideBadEndPanel();
        HideGoodEndPanel();
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
