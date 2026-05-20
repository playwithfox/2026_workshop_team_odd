using UnityEngine;

public static class ChoiceEffectApplier
{
    public static ChoiceResult Apply(ChoiceData choice, GameStats stats)
    {
        if (choice == null || stats == null)
        {
            return null;
        }

        stats.User = ClampStat(stats.User + choice.StatChange_User);
        stats.Public = ClampStat(stats.Public + choice.StatChange_Public);
        stats.Server = ClampStat(stats.Server + choice.StatChange_Server);
        stats.Dev = ClampStat(stats.Dev + choice.StatChange_Dev);
        stats.Budget = ClampStat(stats.Budget + choice.StatChange_Budget);

        if (stats.Flags == null)
        {
            stats.Flags = new System.Collections.Generic.List<string>();
        }

        if (!string.IsNullOrEmpty(choice.ResultFlag) && choice.ResultFlag != "None" && !stats.Flags.Contains(choice.ResultFlag))
        {
            stats.Flags.Add(choice.ResultFlag);
        }

        return new ChoiceResult
        {
            User = stats.User,
            Public = stats.Public,
            Server = stats.Server,
            Dev = stats.Dev,
            Budget = stats.Budget,
            ResultFlag = choice.ResultFlag,
            ResultComment = choice.ResultComment
        };
    }

    private static int ClampStat(int value)
    {
        return Mathf.Clamp(value, 0, 100);
    }
}
