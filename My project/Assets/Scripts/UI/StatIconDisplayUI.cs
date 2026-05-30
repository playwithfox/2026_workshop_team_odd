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
public class StatTierChangeSprites
{
    [Header("0")]
    [SerializeField] private Sprite zeroSpriteUp;
    [SerializeField] private Sprite zeroSpriteDown;

    [Header("1 - 20")]
    [SerializeField] private Sprite oneToTwentySpriteUp;
    [SerializeField] private Sprite oneToTwentySpriteDown;

    [Header("21 - 40")]
    [SerializeField] private Sprite twentyOneToFortySpriteUp;
    [SerializeField] private Sprite twentyOneToFortySpriteDown;

    [Header("41 - 60")]
    [SerializeField] private Sprite fortyOneToSixtySpriteUp;
    [SerializeField] private Sprite fortyOneToSixtySpriteDown;

    [Header("61 - 80")]
    [SerializeField] private Sprite sixtyOneToEightySpriteUp;
    [SerializeField] private Sprite sixtyOneToEightySpriteDown;

    [Header("81 - 100")]
    [SerializeField] private Sprite eightyOneToHundredSpriteUp;
    [SerializeField] private Sprite eightyOneToHundredSpriteDown;

    public Sprite GetSpriteChanged(int value, int beforeValue)
    {
        if (value <= 0)
        {
            if (value > beforeValue)
            {
                return zeroSpriteUp;
            }
            return zeroSpriteDown;
        }
        if (value <= 20)
        {
            if (value > beforeValue)
            {
                return oneToTwentySpriteUp;
            }
            return oneToTwentySpriteDown;
        }

        if (value <= 40)
        {
            if (value > beforeValue)
            {
                return twentyOneToFortySpriteUp;
            }
            return twentyOneToFortySpriteDown;
        }

        if (value <= 60)
        {
            if (value > beforeValue)
            {
                return fortyOneToSixtySpriteUp;
            }
            return fortyOneToSixtySpriteDown;
        }

        if (value <= 80)
        {
            if (value > beforeValue)
            {
                return sixtyOneToEightySpriteUp;
            }
            return sixtyOneToEightySpriteDown;
        }

        if (value > beforeValue)
        {
            return eightyOneToHundredSpriteUp;
        }
        return eightyOneToHundredSpriteDown;
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
    #region Stat Sprites
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
    #endregion
    
    #region Stat Change Sprites
    [Header("User Change Sprites")]
    [Header("User Sprites Up/Down")]
    [SerializeField] private StatTierSprites userSpritesUp;
    [SerializeField] private StatTierSprites userSpritesDown;

    [Header("Public Sprites Up/Down")]
    [SerializeField] private StatTierSprites publicSpritesUp;
    [SerializeField] private StatTierSprites publicSpritesDown;

    [Header("Server Sprites Up/Down")]
    [SerializeField] private StatTierSprites serverSpritesUp;
    [SerializeField] private StatTierSprites serverSpritesDown;

    [Header("Dev Sprites Up/Down")]
    [SerializeField] private StatTierSprites devSpritesUp;
    [SerializeField] private StatTierSprites devSpritesDown;

    [Header("Budget Sprites Up/Down")]
    [SerializeField] private StatTierSprites budgetSpritesUp;
    [SerializeField] private StatTierSprites budgetSpritesDown;
    #endregion

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
    public void RefreshChanged()
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

        SetIcon(userIcon, userSprites != null ? userSprites.GetSpriteChanged(stats.User,stats.BeforeUser) : null);
        SetIcon(publicIcon, publicSprites != null ? publicSprites.GetSpriteChanged(stats.Public,stats.BeforePublic) : null);
        SetIcon(serverIcon, serverSprites != null ? serverSprites.GetSpriteChanged(stats.Server,stats.BeforeServer) : null);
        SetIcon(devIcon, devSprites != null ? devSprites.GetSpriteChanged(stats.Dev,stats.BeforeDev) : null);
        SetIcon(budgetIcon, budgetSprites != null ? budgetSprites.GetSpriteChanged(stats.Budget,stats.BeforeBudget) : null);
    }
    public static void RefreshAllChanged()
    {
        for (int i = activeDisplays.Count - 1; i >= 0; i--)
        {
            StatIconDisplayUI display = activeDisplays[i];
            if (display == null)
            {
                activeDisplays.RemoveAt(i);
                continue;
            }

            display.RefreshChanged();
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
