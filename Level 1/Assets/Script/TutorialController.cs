using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public string ConnectedMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision is BoxCollider2D)
        {
            if (gameObject.name == "GameWon")
            {
                GameController.ChangeGameState(GameController.GameState.Won);
            }
            else
            {
                GameController.ChangeGameState(GameController.GameState.Paused);
                UIController.ShowScreen(ConnectedMenu);
            }
            

            enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            
        }
        
    }
}
