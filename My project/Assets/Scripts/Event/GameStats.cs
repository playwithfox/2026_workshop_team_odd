using System.Collections.Generic;

[System.Serializable]
public class GameStats
{
    public int User = 50;
    public int Public = 50;
    public int Server = 50;
    public int Dev = 50;
    public int Budget = 50;
    public List<string> Flags = new List<string>();
}
