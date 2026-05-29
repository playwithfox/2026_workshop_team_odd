using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class EventChoiceButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float hoverWidth = 760f;

    private RectTransform buttonRectTransform;
    private Image buttonImage;
    private Image hoverImage;
    private readonly List<TMP_Text> textElements = new List<TMP_Text>();
    private readonly List<Vector3> originalTextScales = new List<Vector3>();
    private Vector2 baseSize;
    private bool configured;
    private bool interactionEnabled = true;

    private void Awake()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>();
        EnsureHoverImage();
        CacheTextElements();
    }

    public void Configure(RectTransform targetRectTransform, float targetHoverWidth)
    {
        if (targetRectTransform == null)
        {
            return;
        }

        buttonRectTransform = targetRectTransform;
        buttonImage = GetComponent<Image>();
        hoverWidth = targetHoverWidth;

        EnsureHoverImage();
        CacheTextElements();

        baseSize = buttonRectTransform.sizeDelta;
        configured = true;
        interactionEnabled = true;

        ApplyNormalState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!configured || !interactionEnabled)
        {
            return;
        }

        ApplyHoverState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!configured || !interactionEnabled)
        {
            return;
        }

        ApplyNormalState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!configured || !interactionEnabled)
        {
            return;
        }

        ApplyNormalState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!configured || !interactionEnabled)
        {
            return;
        }

        ApplyNormalState();
    }

    private void OnDisable()
    {
        if (configured)
        {
            ApplyNormalState();
        }
    }

    private void EnsureHoverImage()
    {
        if (buttonRectTransform == null)
        {
            return;
        }

        Transform parent = buttonRectTransform.parent;
        if (parent == null)
        {
            return;
        }

        string hoverObjectName = $"{buttonRectTransform.name}_HoverBackground";
        Transform existing = parent.Find(hoverObjectName);
        if (existing != null)
        {
            hoverImage = existing.GetComponent<Image>();
            UpdateHoverImageVisuals();
            return;
        }

        GameObject hoverObject = new GameObject(hoverObjectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        hoverObject.transform.SetParent(parent, false);
        hoverObject.transform.SetSiblingIndex(buttonRectTransform.GetSiblingIndex());

        RectTransform hoverRect = hoverObject.GetComponent<RectTransform>();
        CopyTransformState(buttonRectTransform, hoverRect);

        hoverImage = hoverObject.GetComponent<Image>();
        hoverImage.raycastTarget = false;
        UpdateHoverImageVisuals();
        hoverImage.enabled = false;
    }

    private void UpdateHoverImageVisuals()
    {
        if (hoverImage == null)
        {
            return;
        }

        hoverImage.sprite = buttonImage != null ? buttonImage.sprite : null;
        hoverImage.type = buttonImage != null ? buttonImage.type : Image.Type.Simple;
        hoverImage.preserveAspect = buttonImage != null && buttonImage.preserveAspect;
        hoverImage.color = buttonImage != null ? buttonImage.color : Color.white;
    }

    private static void CopyTransformState(RectTransform source, RectTransform target)
    {
        if (source == null || target == null)
        {
            return;
        }

        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
        target.localRotation = source.localRotation;
        target.localScale = source.localScale;
    }

    private void CacheTextElements()
    {
        textElements.Clear();
        originalTextScales.Clear();

        TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i < allTexts.Length; i++)
        {
            TMP_Text text = allTexts[i];
            if (text == null)
            {
                continue;
            }

            textElements.Add(text);
            originalTextScales.Add(text.rectTransform.localScale);
        }
    }

    private void ApplyHoverState()
    {
        SetHoverVisual(true);
        SetTextScale(true);
    }

    private void ApplyNormalState()
    {
        SetHoverVisual(false);
        SetTextScale(false);
    }

    public void ResetVisualState()
    {
        if (!configured)
        {
            return;
        }

        ApplyNormalState();
    }

    public void SetInteractionEnabled(bool enabled)
    {
        interactionEnabled = enabled;

        if (!configured)
        {
            return;
        }

        if (!interactionEnabled)
        {
            ApplyNormalState();
        }
    }

    private void SetHoverVisual(bool visible)
    {
        if (hoverImage == null || buttonRectTransform == null)
        {
            return;
        }

        RectTransform hoverRect = hoverImage.rectTransform;
        CopyTransformState(buttonRectTransform, hoverRect);
        UpdateHoverImageVisuals();
        hoverRect.sizeDelta = visible
            ? new Vector2(hoverWidth, baseSize.y)
            : baseSize;
        if (buttonImage != null)
        {
            buttonImage.enabled = !visible;
        }

        hoverImage.enabled = visible;
    }

    private void SetTextScale(bool hovered)
    {
        float scaleMultiplier = hovered ? 1.08f : 1f;

        for (int i = 0; i < textElements.Count; i++)
        {
            TMP_Text text = textElements[i];
            if (text == null)
            {
                continue;
            }

            Vector3 originalScale = i < originalTextScales.Count ? originalTextScales[i] : Vector3.one;
            text.rectTransform.localScale = originalScale * scaleMultiplier;
        }
    }
}
