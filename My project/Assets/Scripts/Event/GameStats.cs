using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class  GameStats
{
    private const int MinStatValue = 0;
    private const int MaxStatValue = 100;
    public int User = 90;
    public int Public = 55;
    public int Server = 80;
    public int Dev = 75;
    public int Budget = 85;
    public List<string> Flags = new List<string>();

    public void InitializeRandom()
    {
        User = GetRandomStatValue(1,100);
        Public = GetRandomStatValue(1,100);
        Server = GetRandomStatValue(1,100);
        Dev = GetRandomStatValue(1,100);
        Budget = GetRandomStatValue(1,100);
        Flags.Clear();
    }

    private int GetRandomStatValue(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue + 1);
    }
}
