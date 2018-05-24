using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    // The type of gate we have
    public enum Types { NOT, OR, AND, XOR, POWER, NULL };
    public Sprite[] GatesSprites = new Sprite[5];

    // Pin Inputs
    public PowerController[] Inputs = new PowerController[2];

    // PowerControllerz
    public PowerController Output;

    // If we place a gate or not
    public Types PlacedGate = Types.NULL;
    bool IsGatePlaced = false;

    // If By default the output is ON or OFF
    public bool DefaultOutputOn = true;

    // Cache the children  SpriteRenderer;
    public SpriteRenderer ValidBox;
    public SpriteRenderer Gate;

    static public GateController[] AllGateControllers = new GateController[50];
    static public int GateControllerCount = 0;

    // When we change the type of gate inserted
    public void OnGatePlaced(Types NewdGate)
    {
        PlacedGate = NewdGate;

        switch (PlacedGate)
        {
            case Types.NOT:

                // Output is the opposite of the input
                Output.PowerStatus = Inputs[0].PowerStatus == PowerController.Status.ON ? PowerController.Status.OFF : PowerController.Status.ON;

                // Change the gate sprite
                Gate.sprite = GatesSprites[0];                

                break;

            case Types.OR:

                // Output is ON if at least one input is on, otherwise is false
                if (Inputs[0].PowerStatus == PowerController.Status.OFF && Inputs[1].PowerStatus == PowerController.Status.OFF)
                    Output.PowerStatus = PowerController.Status.OFF;
                else
                    Output.PowerStatus =  PowerController.Status.ON;

                // Change the gate sprite
                Gate.sprite = GatesSprites[1];

                break;

            case Types.AND:

                // Output is ON if both inputs are on, otherwise is false
                if (Inputs[0].PowerStatus == PowerController.Status.ON && Inputs[1].PowerStatus == PowerController.Status.ON)
                    Output.PowerStatus = PowerController.Status.ON;
                else
                    Output.PowerStatus = PowerController.Status.OFF;

                // Change the gate sprite
                Gate.sprite = GatesSprites[2];

                break;

            case Types.XOR:

                // Output is ON if only inputs is on, otherwise is false
                if ((Inputs[0].PowerStatus == PowerController.Status.ON && Inputs[1].PowerStatus == PowerController.Status.ON) ||
                     Inputs[0].PowerStatus == PowerController.Status.OFF && Inputs[1].PowerStatus == PowerController.Status.OFF)
                    Output.PowerStatus = PowerController.Status.OFF;
                else
                    Output.PowerStatus = PowerController.Status.ON;

                // Change the gate sprite
                Gate.sprite = GatesSprites[3];

                break;

            case Types.POWER:

                // Output is the opposite of the input
                Output.PowerStatus = PowerController.Status.ON;
                Output.DefaulStatus = PowerController.Status.ON;

                // Change the gate sprite
                Gate.sprite = GatesSprites[4];

                break;
        }

        if (PlacedGate != Types.NULL)
        {
            Gate.enabled = true;
        }
        else
        {
            Gate.enabled = false;
        }

    }
	// Use this for initialization
	void Start () {

        AllGateControllers[GateControllerCount++] = this;
        if (gameObject.transform.parent.gameObject.name == "ScritpableCable_19")
            Debug.Log("");

        OnGatePlaced(PlacedGate);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is CircleCollider2D)
        {
            ValidBox.enabled = true;
            collision.gameObject.GetComponent<PlayerController>().CanPlaceGate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is CircleCollider2D)
        {
            ValidBox.enabled = false;
            collision.gameObject.GetComponent<PlayerController>().CanPlaceGate = false;
        }
    }
}
