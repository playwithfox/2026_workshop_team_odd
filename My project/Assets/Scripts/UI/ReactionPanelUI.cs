using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReactionPanelUI : MonoBehaviour
{
    [Header("Reaction Texts")]
    [SerializeField] private TMP_Text reactionText1;
    [SerializeField] private TMP_Text reactionText2;
    [SerializeField] private TMP_Text reactionText3;
    [SerializeField] private TMP_Text reactionText4;
    [SerializeField] private TMP_Text reactionText5;

    public void ShowReaction(ChoiceData choice)
    {
        List<string> reactions = choice != null ? choice.reaction_community : null;

        SetReactionText(reactionText1, reactions, 0);
        SetReactionText(reactionText2, reactions, 1);
        SetReactionText(reactionText3, reactions, 2);
        SetReactionText(reactionText4, reactions, 3);
        SetReactionText(reactionText5, reactions, 4);
    }

    public void ClearReactionTexts()
    {
        SetText(reactionText1, string.Empty);
        SetText(reactionText2, string.Empty);
        SetText(reactionText3, string.Empty);
        SetText(reactionText4, string.Empty);
        SetText(reactionText5, string.Empty);
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
}
