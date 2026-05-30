using UnityEngine;
using UnityEngine.UI;

public class GoodEndPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TitleScreenUI titleScreenUI;

    [Header("Panel Root")]
    [SerializeField] private GameObject goodEndPanelRoot;

    [Header("Navigation")]
    [SerializeField] private Button returnToTitleButton;

    private void Awake()
    {
        if (goodEndPanelRoot == null)
        {
            goodEndPanelRoot = gameObject;
        }

        if (returnToTitleButton != null)
        {
            returnToTitleButton.onClick.RemoveListener(HandleReturnToTitleClicked);
            returnToTitleButton.onClick.AddListener(HandleReturnToTitleClicked);
        }

        if (goodEndPanelRoot != null)
        {
            goodEndPanelRoot.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (returnToTitleButton != null)
        {
            returnToTitleButton.onClick.RemoveListener(HandleReturnToTitleClicked);
        }
    }

    public void ShowGoodEnding()
    {
        if (goodEndPanelRoot != null)
        {
            goodEndPanelRoot.SetActive(true);
        }
    }

    public void HandleReturnToTitleClicked()
    {
        if (goodEndPanelRoot != null)
        {
            goodEndPanelRoot.SetActive(false);
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
}
