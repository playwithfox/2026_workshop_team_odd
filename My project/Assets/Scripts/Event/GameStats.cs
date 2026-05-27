using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStats
{
    private const int MinStatValue = 0;
    private const int MaxStatValue = 100;

    public int User = 50;
    public int Public = 50;
    public int Server = 50;
    public int Dev = 50;
    public int Budget = 50;
    public List<string> Flags = new List<string>();

    public void InitializeRandom(int startMinValue = 45, int startMaxValue = 65)
    {
        startMinValue = Mathf.Clamp(startMinValue, MinStatValue, MaxStatValue);
        startMaxValue = Mathf.Clamp(startMaxValue, MinStatValue, MaxStatValue);

        if (startMaxValue < startMinValue)
        {
            startMaxValue = startMinValue;
        }

        User = GetRandomStatValue(startMinValue, startMaxValue);
        Public = GetRandomStatValue(startMinValue, startMaxValue);
        Server = GetRandomStatValue(startMinValue, startMaxValue);
        Dev = GetRandomStatValue(startMinValue, startMaxValue);
        Budget = GetRandomStatValue(startMinValue, startMaxValue);

        if (Flags == null)
        {
            Flags = new List<string>();
        }
        else
        {
            Flags.Clear();
        }
    }

    private int GetRandomStatValue(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue + 1);
    }
}
