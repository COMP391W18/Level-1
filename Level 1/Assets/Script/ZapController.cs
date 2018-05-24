using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapController : MonoBehaviour {

    public PowerController[] Inputs = new PowerController[2];
    public bool IsActiveOnHigh = true;

    public Color DefaultColor;
    public Color AlternateColor;

    // Update is called once per frame
    void Update () {
		if (Inputs.Length == 1)
        {
            if (Inputs[0].PowerStatus == PowerController.Status.OFF && IsActiveOnHigh)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }
            else if (Inputs[0].PowerStatus == PowerController.Status.ON && IsActiveOnHigh)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            }
            else if (Inputs[0].PowerStatus == PowerController.Status.OFF && !IsActiveOnHigh)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            }
            else if (Inputs[0].PowerStatus == PowerController.Status.ON && !IsActiveOnHigh)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }

        }
    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = IsActiveOnHigh ? DefaultColor : AlternateColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we enter and enter teleport
        if (collision.gameObject.name == "Player" && collision is BoxCollider2D)
        {
            GameController.ChangeGameState(GameController.GameState.Over);
        }
    }
}
