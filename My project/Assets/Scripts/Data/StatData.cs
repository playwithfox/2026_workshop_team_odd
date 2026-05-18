using UnityEngine;

[CreateAssetMenu(fileName = "NewStatData", menuName = "Game Data/Stat Data")]
public class StatData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private Color displayColor = Color.white;

    [Header("Value Range")]
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 100;

    [Header("Starting Value")]
    [SerializeField] private int startMinValue = 45;
    [SerializeField] private int startMaxValue = 65;

    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public Color DisplayColor => displayColor;
    public int MinValue => minValue;
    public int MaxValue => maxValue;
    public int StartMinValue => startMinValue;
    public int StartMaxValue => startMaxValue;

    private void OnValidate()
    {
        if (maxValue < minValue)
        {
            maxValue = minValue;
        }

        startMinValue = Mathf.Clamp(startMinValue, minValue, maxValue);
        startMaxValue = Mathf.Clamp(startMaxValue, minValue, maxValue);

        if (startMaxValue < startMinValue)
        {
            startMaxValue = startMinValue;
        }
    }
}
