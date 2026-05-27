using System.Collections.Generic;

[System.Serializable]
public class ChoiceResult
{
    public int User;
    public int Public;
    public int Server;
    public int Dev;
    public int Budget;
    public string ResultFlag;
    public string ResultComment;
    public string result_summary;
    public List<string> reaction_community = new List<string>();
    public List<string> reaction_internal = new List<string>();
    public List<string> reaction_server = new List<string>();
    public List<string> reaction_management = new List<string>();
}
