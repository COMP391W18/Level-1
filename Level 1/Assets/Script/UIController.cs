using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [System.Serializable]
    public struct ScreenStruct
    {
        public System.String Name;
        public GameObject Screen;

    }
    public ScreenStruct[] ScreenList;

    // Game UI
    static public GameObject UI;

    // List of screens
    static private Dictionary<System.String, GameObject> Screens;
    
    static public void ShowScreen(string ScreenName)
    {
        HideScreens();

        // Display the selected screen
        Screens[ScreenName].SetActive(true);
    }

    static public void HideScreens()
    {
        // Hide all the menus but the selected screen
        foreach (var Screen in Screens)
            Screen.Value.SetActive(false);
    }

    // Use this for initialization
    void Awake()
    {
        // Fill the Dictionary struct
        Screens = new Dictionary<System.String, GameObject>();
        foreach (ScreenStruct ScreenObject in ScreenList)
            Screens.Add(ScreenObject.Name, ScreenObject.Screen);

        // Cache the UI object
        UI = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update ()
    {
	    switch (GameController.CurrentGameState)
        {
            case GameController.GameState.Paused:
                if (Input.GetKeyUp(KeyCode.P))
                    GameController.ChangeGameState(GameController.GameState.Running);
                break;
                
            case GameController.GameState.Running:
                if (Input.GetKeyUp(KeyCode.P))
                    GameController.ChangeGameState(GameController.GameState.Paused);
                break;
                
            case GameController.GameState.Started:
                if (Input.GetKeyUp(KeyCode.P))
                    GameController.ChangeGameState(GameController.GameState.Running);
                break;
                
            case GameController.GameState.Over:
                break;
        }
	}
}
