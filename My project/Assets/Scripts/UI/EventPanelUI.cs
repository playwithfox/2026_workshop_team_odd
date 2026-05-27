using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image eventImage;

    public void ShowEvent(EventData eventData)
    {
        titleText.text = eventData.Title;
        descriptionText.text = eventData.Description;

        Sprite sprite = Resources.Load<Sprite>("UI_Images/" + eventData.ImageID);
        eventImage.sprite = sprite;
    }
}