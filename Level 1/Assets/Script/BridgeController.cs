using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeController : MonoBehaviour {

    public PowerController[] Inputs = new PowerController[2];
    	
	// Update is called once per frame
	void Update () {
        if (Inputs.Length == 1)
        {
            if (Inputs[0].PowerStatus == PowerController.Status.OFF)
            {
                gameObject.GetComponent<TilemapRenderer>().enabled = false;
                gameObject.GetComponent<TilemapCollider2D>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<TilemapRenderer>().enabled = true;
                gameObject.GetComponent<TilemapCollider2D>().enabled = true;
            }
        }
    }
}
