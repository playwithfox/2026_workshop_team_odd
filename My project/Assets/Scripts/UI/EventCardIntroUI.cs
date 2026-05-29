// 사건 카드 화면 애니메이션
using DG.Tweening;
using UnityEngine;

public class EventCardIntroUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private RectTransform eventCard;
    [SerializeField] private RectTransform[] optionButtons;

    [Header("Positions")]
    [SerializeField] private Vector2 centerPosition = new Vector2(0f, 338f);
    [SerializeField] private Vector2 finalPosition = new Vector2(-460f, 338f);

    [Header("Timing")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float holdDelay = 1f;
    [SerializeField] private float revealDuration = 0.6f;
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private float optionStartDelay = 0.6f;
    [SerializeField] private float optionInterval = 0.4f;
    [SerializeField] private float optionRevealDuration = 0.4f;
    [SerializeField] private Ease revealEase = Ease.OutCubic;
    [SerializeField] private Ease moveEase = Ease.InOutCubic;
    [SerializeField] private Ease optionRevealEase = Ease.OutCubic;

    private Vector2 originalPivot;
    private Vector3 originalScale;
    private Vector2[] optionOriginalPivots;
    private Vector3[] optionOriginalScales;
    private Sequence introSequence;
    private bool hasSavedOriginalValues;

    private void OnDestroy()
    {
        KillIntroSequence();
    }

    public void PrepareIntro()
    {
        KillIntroSequence();

        if (eventCard == null)
        {
            Debug.LogError("EventCardIntroUI: Event Card is not assigned.", this);
            return;
        }

        SaveOriginalValues();

        eventCard.pivot = new Vector2(originalPivot.x, 1f);
        eventCard.anchoredPosition = centerPosition;
        eventCard.localScale = new Vector3(originalScale.x, 0f, originalScale.z);

        PrepareOptionButtons();
    }

    public void PlayIntro()
    {
        if (eventCard == null)
        {
            Debug.LogError("EventCardIntroUI: Event Card is not assigned.", this);
            return;
        }

        KillIntroSequence();

        introSequence = DOTween.Sequence();
        introSequence.AppendInterval(startDelay);
        introSequence.Append(eventCard.DOScaleY(originalScale.y, revealDuration).SetEase(revealEase));
        introSequence.AppendInterval(holdDelay);
        introSequence.Append(eventCard.DOAnchorPos(finalPosition, moveDuration).SetEase(moveEase));
        introSequence.AppendInterval(optionStartDelay);
        AppendOptionButtonReveals();
    }

    private void SaveOriginalValues()
    {
        if (hasSavedOriginalValues)
        {
            return;
        }

        originalPivot = eventCard.pivot;
        originalScale = eventCard.localScale;

        optionOriginalPivots = new Vector2[optionButtons.Length];
        optionOriginalScales = new Vector3[optionButtons.Length];

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] == null)
            {
                continue;
            }

            optionOriginalPivots[i] = optionButtons[i].pivot;
            optionOriginalScales[i] = optionButtons[i].localScale;
        }

        hasSavedOriginalValues = true;
    }

    private void PrepareOptionButtons()
    {
        if (optionOriginalPivots == null || optionOriginalScales == null)
        {
            return;
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            RectTransform optionButton = optionButtons[i];

            if (optionButton == null)
            {
                continue;
            }

            optionButton.pivot = new Vector2(optionOriginalPivots[i].x, 1f);
            optionButton.localScale = new Vector3(optionOriginalScales[i].x, 0f, optionOriginalScales[i].z);
        }
    }

    private void AppendOptionButtonReveals()
    {
        if (optionOriginalScales == null)
        {
            return;
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            RectTransform optionButton = optionButtons[i];

            if (optionButton == null)
            {
                continue;
            }

            if (i > 0)
            {
                introSequence.AppendInterval(optionInterval);
            }

            introSequence.Append(optionButton.DOScaleY(optionOriginalScales[i].y, optionRevealDuration).SetEase(optionRevealEase));
        }
    }

    private void KillIntroSequence()
    {
        if (introSequence != null && introSequence.IsActive())
        {
            introSequence.Kill();
        }
    }
}
