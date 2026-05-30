using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int maxDay = 7;
    [SerializeField] private CallEnding callEnding;

    private int currentDay = 1;
    private bool isGameOver;
    private readonly GameStats gamestats = new GameStats();

    public int CurrentDay => currentDay;
    public bool IsGameOver => isGameOver;
    public GameStats Stats => gamestats;

    public void StartGame()
    {
        ResetGameState();
        Debug.Log("Game start");
        Debug.Log($"{currentDay} day start");
    }

    public void ResetGameState()
    {
        currentDay = 1;
        isGameOver = false;
        gamestats.InitializeRandom();
    }

    private void CheckGameOver()
    {
        if (gamestats.User <= 0 ||
            gamestats.Public <= 0 ||
            gamestats.Server <= 0 ||
            gamestats.Dev <= 0 ||
            gamestats.Budget <= 0)
        {
            isGameOver = true;
            callEnding?.PrintEnding(gamestats);
            Debug.Log("Game over condition reached");
        }
    }

    public void GoToNextDay()
    {
        if (isGameOver)
        {
            Debug.Log("Game over state, cannot advance to next day");
            CheckEnding();
            return;
        }

        currentDay++;

        if (currentDay > maxDay)
        {
            Debug.Log("Reached final day. Move to ending");
            CheckEnding();
            return;
        }

        Debug.Log($"{currentDay} day start");
    }

    private void CheckEnding()
    {
        if (isGameOver)
        {
            Debug.Log("Bad ending");
            callEnding?.PrintEnding(gamestats);
        }
        else
        {
            Debug.Log("Good ending");
            callEnding?.PrintEnding(gamestats);
        }
    }

    public void isthisreal()
    {
        gamestats.User = 0;
        CheckGameOver();
    }
}
