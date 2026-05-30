using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReactionPanelUI : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeInDuration = 0.8f;
    [SerializeField] private float staggerDelay = 0.5f;

    [Header("Panel Navigation")]
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject reactionPanelRoot;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private ResultPanelUI resultPanelUI;

    [Header("Reaction Texts")]
    [SerializeField] private TMP_Text reactionText1;
    [SerializeField] private TMP_Text reactionText2;
    [SerializeField] private TMP_Text reactionText3;
    [SerializeField] private TMP_Text reactionText4;
    [SerializeField] private TMP_Text reactionText5;

    private readonly List<TMP_Text> reactionTexts = new List<TMP_Text>(5);
    private Sequence fadeSequence;
    private Tween nextButtonRevealTween;
    private ChoiceData currentChoice;
    private GameObject selectedChoiceButtonObject;

    private void Awake()
    {
        if (reactionPanelRoot == null)
        {
            reactionPanelRoot = gameObject;
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
            nextButton.onClick.AddListener(HandleNextButtonClicked);
        }

        CacheTexts();
        SetAllTextAlpha(0f);
        HideNextButton();
    }

    private void OnDestroy()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
        }

        KillFadeSequence();
        KillNextButtonRevealTween();
    }

    public void ShowReaction(ChoiceData choice)
    {
        currentChoice = choice;
        CacheTexts();

        List<string> reactions = choice != null ? choice.reaction_community : null;

        SetReactionText(reactionText1, reactions, 0);
        SetReactionText(reactionText2, reactions, 1);
        SetReactionText(reactionText3, reactions, 2);
        SetReactionText(reactionText4, reactions, 3);
        SetReactionText(reactionText5, reactions, 4);

        PlayFadeInSequence();
    }

    public void SetSelectedChoiceButton(Button selectedButton)
    {
        selectedChoiceButtonObject = selectedButton != null ? selectedButton.gameObject : null;
    }

    public void ClearReactionTexts()
    {
        KillFadeSequence();
        KillNextButtonRevealTween();
        currentChoice = null;
        selectedChoiceButtonObject = null;

        SetText(reactionText1, string.Empty);
        SetText(reactionText2, string.Empty);
        SetText(reactionText3, string.Empty);
        SetText(reactionText4, string.Empty);
        SetText(reactionText5, string.Empty);
        SetAllTextAlpha(0f);
    }

    public void PrepareForReactionDisplay()
    {
        KillFadeSequence();
        KillNextButtonRevealTween();
        CacheTexts();
        SetAllTextAlpha(0f);
        HideNextButton();
    }

    public void HandleNextButtonClicked()
    {
        if (selectedChoiceButtonObject != null)
        {
            selectedChoiceButtonObject.SetActive(false);
            selectedChoiceButtonObject = null;
        }

        EventDaySchedule.ConsumeOne();

        if (reactionPanelRoot != null)
        {
            reactionPanelRoot.SetActive(false);
        }

        ResultPanelUI resolvedResultPanelUI = ResolveResultPanelUI();
        if (resolvedResultPanelUI != null)
        {
            resolvedResultPanelUI.ShowResult(currentChoice);
        }

        ClearReactionTexts();

    }

    private static void SetReactionText(TMP_Text target, List<string> reactions, int index)
    {
        if (target == null)
        {
            return;
        }

        string value = reactions != null && index >= 0 && index < reactions.Count ? reactions[index] : string.Empty;
        SetText(target, value);
    }

    private static void SetText(TMP_Text target, string value)
    {
        if (target == null)
        {
            return;
        }

        target.text = value ?? string.Empty;
    }

    private void CacheTexts()
    {
        reactionTexts.Clear();
        AddTextIfValid(reactionText1);
        AddTextIfValid(reactionText2);
        AddTextIfValid(reactionText3);
        AddTextIfValid(reactionText4);
        AddTextIfValid(reactionText5);
    }

    private void AddTextIfValid(TMP_Text target)
    {
        if (target == null)
        {
            return;
        }

        if (!reactionTexts.Contains(target))
        {
            reactionTexts.Add(target);
        }
    }

    private void PlayFadeInSequence()
    {
        KillFadeSequence();
        SetAllTextAlpha(0f);
        HideNextButton();

        fadeSequence = DOTween.Sequence();

        for (int i = 0; i < reactionTexts.Count; i++)
        {
            TMP_Text text = reactionTexts[i];
            if (text == null)
            {
                continue;
            }

            text.gameObject.SetActive(true);
            text.alpha = 0f;
            fadeSequence.Append(text.DOFade(1f, fadeInDuration));

            if (i < reactionTexts.Count - 1)
            {
                fadeSequence.AppendInterval(staggerDelay);
            }
        }

        fadeSequence.OnComplete(ShowNextButtonAfterDelay);
    }

    private void SetAllTextAlpha(float alpha)
    {
        for (int i = 0; i < reactionTexts.Count; i++)
        {
            TMP_Text text = reactionTexts[i];
            if (text == null)
            {
                continue;
            }

            text.alpha = alpha;
        }
    }

    private void KillFadeSequence()
    {
        if (fadeSequence != null && fadeSequence.IsActive())
        {
            fadeSequence.Kill();
        }
    }

    private void ShowNextButtonAfterDelay()
    {
        KillNextButtonRevealTween();

        if (nextButton == null)
        {
            return;
        }

        nextButtonRevealTween = DOVirtual.DelayedCall(0.3f, () =>
        {
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(true);
            }
        });
    }

    private void HideNextButton()
    {
        if (nextButton == null)
        {
            return;
        }

        nextButton.gameObject.SetActive(false);
    }

    private void KillNextButtonRevealTween()
    {
        if (nextButtonRevealTween != null && nextButtonRevealTween.IsActive())
        {
            nextButtonRevealTween.Kill();
        }
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

    private ResultPanelUI ResolveResultPanelUI()
    {
        if (resultPanelUI != null)
        {
            return resultPanelUI;
        }

        GameObject resolvedResultPanel = ResolveResultPanel();
        if (resolvedResultPanel == null)
        {
            return null;
        }

        resultPanelUI = resolvedResultPanel.GetComponent<ResultPanelUI>();
        if (resultPanelUI == null)
        {
            resultPanelUI = resolvedResultPanel.GetComponentInChildren<ResultPanelUI>(true);
        }

        return resultPanelUI;
    }
}
