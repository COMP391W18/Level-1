using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    
    // Spots in the UI
    static public GameObject[] InvetorySpots = new GameObject[5];

    // Images used by the UI
    public Sprite[] EditorSprites = new Sprite[5];
    static Sprite[] StaticSprites = new Sprite[5];
    

    static int[] ItemQuantity = new int[5];
    static int SelectedItem = 0;

    static public void ResetInventory()
    {
        SelectedItem = 0;

        for (int Index = 0; Index < ItemQuantity.Length; ++Index)
            ItemQuantity[Index] = 0;

        foreach (GameObject Spot in InvetorySpots)
        {
            Spot.transform.Find("NumberOfItems").GetComponent<Text>().text = "0";
            Spot.transform.Find("Image").GetComponent<Image>().enabled = false;
        }
    }

    static public void OnAddItemToInventory(string Item)
    {
        int ItemIndex = 0;

        switch (Item)
        {
            case "NOT":
                ItemIndex = 0;
                break;
            case "OR":
                ItemIndex = 1;
                break;
            case "AND":
                ItemIndex = 2;
                break;
            case "XOR":
                ItemIndex = 3;
                break;
            case "POWER":
                ItemIndex = 4;
                break;
        }

        ++ItemQuantity[ItemIndex];

        InvetorySpots[ItemIndex].transform.Find("NumberOfItems").GetComponent<Text>().text = System.Convert.ToString(ItemQuantity[ItemIndex]);

        Image ImageComponent = InvetorySpots[ItemIndex].transform.Find("Image").GetComponent<Image>();
        ImageComponent.sprite = StaticSprites[ItemIndex];
        ImageComponent.enabled = true;
    }

    static public void OnRemoveItemFromInventory(GateController.Types Item)
    {
        int ItemIndex = 0;

        switch (Item)
        {
            case GateController.Types.NOT:
                ItemIndex = 0;
                break;
            case GateController.Types.OR:
                ItemIndex = 1;
                break;
            case GateController.Types.AND:
                ItemIndex = 2;
                break;
            case GateController.Types.XOR:
                ItemIndex = 3;
                break;
            case GateController.Types.POWER:
                ItemIndex = 4;
                break;
        }

        --ItemQuantity[ItemIndex];

        InvetorySpots[ItemIndex].transform.Find("NumberOfItems").GetComponent<Text>().text = System.Convert.ToString(ItemQuantity[ItemIndex]);

        if (ItemQuantity[ItemIndex] == 0)
        {
            InvetorySpots[ItemIndex].transform.Find("Image").GetComponent<Image>().enabled = false;
        }

        SelectedItem = 0;
    }
    
    static public GateController.Types GetSelectedItem()
    {
        if (SelectedItem > 0 && ItemQuantity[SelectedItem - 1] > 0)
        {
            return (GateController.Types)(SelectedItem - 1);
        }

        return GateController.Types.NULL;
    }

    // Use this for initialization
    void Start () {
        // Cache the spots gameobject
        for (int Index = 0; Index < InvetorySpots.Length; ++Index)
            InvetorySpots[Index] = transform.GetChild(Index).gameObject;

        // Cache the sprites
        for (int Index = 0; Index < EditorSprites.Length; ++Index)
            StaticSprites[Index] = EditorSprites[Index];
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Reset background color
        foreach (GameObject Spot in InvetorySpots)
            Spot.transform.Find("Background").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.75f);

        // Highlight the selected panel
        if (SelectedItem > 0)
            InvetorySpots[SelectedItem - 1].transform.Find("Background").GetComponent<Image>().color = new Color(32f / 255f, 148f / 255f, 28f / 255f, 0.75f);


        // Check for user input
        if (Input.GetKeyDown(KeyCode.Z) && ItemQuantity[0] > 0)
            //PanelSelection[KeyCode.Z] = !PanelSelection[KeyCode.Z];
            SelectedItem = 1;
        else if (Input.GetKeyDown(KeyCode.X) && ItemQuantity[1] > 0)
            //PanelSelection[KeyCode.X] = !PanelSelection[KeyCode.Z];
            SelectedItem = 2;
        else if (Input.GetKeyDown(KeyCode.C) && ItemQuantity[2] > 0)
            //PanelSelection[KeyCode.C] = !PanelSelection[KeyCode.Z];
            SelectedItem = 3;
        else if (Input.GetKeyDown(KeyCode.V) && ItemQuantity[3] > 0)
            //PanelSelection[KeyCode.C] = !PanelSelection[KeyCode.Z];
            SelectedItem = 4;
        else if (Input.GetKeyDown(KeyCode.B) && ItemQuantity[4] > 0)
            //PanelSelection[KeyCode.C] = !PanelSelection[KeyCode.Z];
            SelectedItem = 5;


    }
}
