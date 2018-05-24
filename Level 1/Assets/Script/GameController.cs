using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Possible game states
    public enum GameState
    {
        Started,
        Running,
        Paused,
        Over,
        Won
    }

    // current game state
    static public GameState CurrentGameState { get; protected set; }

    public static SaveGameController LastSaveGame { get; set; }

    // Function used to change the game state
    static public void ChangeGameState(GameState NewGameState)
    {
        // Change the current game state
        CurrentGameState = NewGameState;

        switch (CurrentGameState)
        {
            // If we pause the game
            case GameState.Paused:
                OnGamePaused();
                break;

            // If we are running the game
            case GameState.Running:
                OnGameRunning();
                break;

            // When we start the game
            case GameState.Started:
                OnGameStarted();
                break;

            // When we finish the game
            case GameState.Over:
                OnGameOver();
                break;

            // When we win the game
            case GameState.Won:
                OnGameWon();
                break;
        }
    }

    // Function run when we pause the game
    private static void OnGamePaused()
    {
        // Show the paused UI
        UIController.ShowScreen("GamePaused");

        Time.timeScale = 0;
    }

    // Function run when we pause the game
    private static void OnGameRunning()
    {
        UIController.HideScreens();

        Time.timeScale = 1;
    }

    // Function run when we pause the game
    private static void OnGameStarted()
    {
        UIController.ShowScreen("GameStarted");

        Time.timeScale = 0;
    }

    private static void OnGameOver()
    {
        UIController.ShowScreen("GameEnded");

        Time.timeScale = 0;
    }

    private static void OnGameWon()
    {
        UIController.ShowScreen("GameWon");

        Time.timeScale = 0;
    }

    // Use this for initialization
    void Start ()
    {
        ChangeGameState(GameState.Started);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CurrentGameState == GameState.Over)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeGameState(GameState.Running);

                if (LastSaveGame != null)
                    LastSaveGame.ResetSection();
                else
                {
                    GateController.AllGateControllers = new GateController[50];
                    GateController.GateControllerCount = 0;

                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        else if (CurrentGameState == GameState.Won)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
	}
}
