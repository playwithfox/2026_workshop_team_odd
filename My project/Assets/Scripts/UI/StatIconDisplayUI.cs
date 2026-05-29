using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatTierSprites
{
    [Header("0")]
    [SerializeField] private Sprite zeroSprite;

    [Header("1 - 20")]
    [SerializeField] private Sprite oneToTwentySprite;

    [Header("21 - 40")]
    [SerializeField] private Sprite twentyOneToFortySprite;

    [Header("41 - 60")]
    [SerializeField] private Sprite fortyOneToSixtySprite;

    [Header("61 - 80")]
    [SerializeField] private Sprite sixtyOneToEightySprite;

    [Header("81 - 100")]
    [SerializeField] private Sprite eightyOneToHundredSprite;

    public Sprite GetSprite(int value)
    {
        if (value <= 0)
        {
            return zeroSprite;
        }

        if (value <= 20)
        {
            return oneToTwentySprite;
        }

        if (value <= 40)
        {
            return twentyOneToFortySprite;
        }

        if (value <= 60)
        {
            return fortyOneToSixtySprite;
        }

        if (value <= 80)
        {
            return sixtyOneToEightySprite;
        }

        return eightyOneToHundredSprite;
    }
}

public class StatIconDisplayUI : MonoBehaviour
{
    private static readonly List<StatIconDisplayUI> activeDisplays = new List<StatIconDisplayUI>();

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Stat Icons")]
    [SerializeField] private Image userIcon;
    [SerializeField] private Image publicIcon;
    [SerializeField] private Image serverIcon;
    [SerializeField] private Image devIcon;
    [SerializeField] private Image budgetIcon;

    [Header("User Sprites")]
    [SerializeField] private StatTierSprites userSprites;

    [Header("Public Sprites")]
    [SerializeField] private StatTierSprites publicSprites;

    [Header("Server Sprites")]
    [SerializeField] private StatTierSprites serverSprites;

    [Header("Dev Sprites")]
    [SerializeField] private StatTierSprites devSprites;

    [Header("Budget Sprites")]
    [SerializeField] private StatTierSprites budgetSprites;

    private void OnEnable()
    {
        Register();
        Refresh();
    }

    private void OnDisable()
    {
        Unregister();
    }

    public void Refresh()
    {
        if (!ValidateReferences())
        {
            return;
        }

        GameStats stats = gameManager.Stats;
        if (stats == null)
        {
            return;
        }

        SetIcon(userIcon, userSprites != null ? userSprites.GetSprite(stats.User) : null);
        SetIcon(publicIcon, publicSprites != null ? publicSprites.GetSprite(stats.Public) : null);
        SetIcon(serverIcon, serverSprites != null ? serverSprites.GetSprite(stats.Server) : null);
        SetIcon(devIcon, devSprites != null ? devSprites.GetSprite(stats.Dev) : null);
        SetIcon(budgetIcon, budgetSprites != null ? budgetSprites.GetSprite(stats.Budget) : null);
    }

    public static void RefreshAll()
    {
        for (int i = activeDisplays.Count - 1; i >= 0; i--)
        {
            StatIconDisplayUI display = activeDisplays[i];
            if (display == null)
            {
                activeDisplays.RemoveAt(i);
                continue;
            }

            display.Refresh();
        }
    }

    private void Register()
    {
        if (!activeDisplays.Contains(this))
        {
            activeDisplays.Add(this);
        }
    }

    private void Unregister()
    {
        activeDisplays.Remove(this);
    }

    private bool ValidateReferences()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameManager == null)
        {
            Debug.LogError("StatIconDisplayUI: GameManager is not assigned.", this);
            return false;
        }

        if (userIcon == null)
        {
            Debug.LogError("StatIconDisplayUI: User Icon is not assigned.", this);
            return false;
        }

        if (publicIcon == null)
        {
            Debug.LogError("StatIconDisplayUI: Public Icon is not assigned.", this);
            return false;
        }

        if (serverIcon == null)
        {
            Debug.LogError("StatIconDisplayUI: Server Icon is not assigned.", this);
            return false;
        }

        if (devIcon == null)
        {
            Debug.LogError("StatIconDisplayUI: Dev Icon is not assigned.", this);
            return false;
        }

        if (budgetIcon == null)
        {
            Debug.LogError("StatIconDisplayUI: Budget Icon is not assigned.", this);
            return false;
        }

        return true;
    }

    private static void SetIcon(Image target, Sprite sprite)
    {
        if (target == null)
        {
            return;
        }

        target.sprite = sprite;
        target.enabled = sprite != null;
    }
}
