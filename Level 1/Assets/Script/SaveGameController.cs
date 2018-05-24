using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour {


    public PowerController.Status[] States;
    public GateController.Types[] PlacedGates;
    public PowerUpComponent[] PlacedCranes;

    public GameObject Section;
    public GameObject CraneSection;
    bool IsActive = false;
    Vector3 OldPlayerPosition;

	// Use this for initialization
	void Start ()
    {
		
	}

    void SaveInfo()
    {        
        if (Section != null)
        {
            // Save all the info about the gates
            GateController[] GateToSave = Section.GetComponentsInChildren<GateController>();
            PlacedGates = new GateController.Types[GateToSave.Length];
            for (int Index = 0; Index < GateToSave.Length; ++Index)
            {
                PlacedGates[Index] = GateToSave[Index].PlacedGate;
            }

            // Save all the info about the power cables
            PowerController[] PowerToSave = Section.GetComponentsInChildren<PowerController>();
            States = new PowerController.Status[PowerToSave.Length];
            for (int Index = 0; Index < PowerToSave.Length; ++Index)
            {
                States[Index] = PowerToSave[Index].PowerStatus;
            }
        }

        // Save all the info about the power cables
        if (CraneSection != null)
            PlacedCranes = CraneSection.GetComponentsInChildren<PowerUpComponent>();
    }

    public void ResetSection()
    {
        // Reset the player position
        GameObject.FindGameObjectWithTag("Player").transform.position = OldPlayerPosition;
        
        if (Section != null)
        {
            GateController[] GateToReload = Section.GetComponentsInChildren<GateController>();
            for (int Index = 0; Index < GateToReload.Length; ++Index)
            {
                GateToReload[Index].OnGatePlaced(PlacedGates[Index]);

            }

            PowerController[] PowerToReload = Section.GetComponentsInChildren<PowerController>();
            for (int Index = 0; Index < PowerToReload.Length; ++Index)
            {
                PowerToReload[Index].PowerStatus = States[Index];
            }
        }

        if (CraneSection != null)
        {
            foreach (PowerUpComponent Crane in PlacedCranes)
                Crane.RenablePowerUp();
        }
            

        InventoryController.ResetInventory();
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (GameController.CurrentGameState == GameController.GameState.Running)
        {
            if (IsActive && Input.GetKeyDown(KeyCode.R))
            {
                GameController.LastSaveGame.ResetSection();
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision is BoxCollider2D)
        {
            OldPlayerPosition = collision.gameObject.transform.position;
            IsActive = true;

            SaveInfo();

            GameController.LastSaveGame = this;

            //GetComponent<BoxCollider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
        }

        
    }
}
