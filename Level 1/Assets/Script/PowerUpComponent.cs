using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpComponent : MonoBehaviour {

    [System.Serializable]
    public struct Swingable
    {
        public float Offset;
        public float Velocity;
        public float OriginalPos { get; set; }
        private float CurrentOffset;       

        public float Update()
        {
            CurrentOffset = Mathf.MoveTowards(CurrentOffset, Offset, Velocity * Time.deltaTime);

            if (Mathf.Approximately(CurrentOffset, Offset))
                 Offset = -Offset;

            return CurrentOffset + OriginalPos;
        }
    }

    //public enum Type { Gate, Power };
    public GateController.Types GateType;
    
    // Cached the child gameobject that holds the sprite of the type of pwerup
    GameObject PowerUpObject;
    Vector3[] OriginalTransform = new Vector3[2];

    // Swing propertis
    public Swingable SwingDistance;

    // Identify whatever we are picked up or not
    bool IsPickeUp = false;

    string GetPowerUpType()
    {
        return GateType.ToString();
    }

	// Use this for initialization
	void Start ()
    {
		// Hide one asset based on what type of component this is
        if (GateType != GateController.Types.POWER)
        {
            gameObject.transform.Find("Power").gameObject.SetActive(false);
            PowerUpObject = gameObject.transform.Find("Chip").gameObject;
        }
        else
        {
            gameObject.transform.Find("Chip").gameObject.SetActive(false);
            PowerUpObject = gameObject.transform.Find("Power").gameObject;
        }

        // Get the original offset ogf the swingable gameobject
        SwingDistance.OriginalPos = PowerUpObject.transform.localPosition.y;

        OriginalTransform[0] = PowerUpObject.transform.localPosition;
        OriginalTransform[1] = PowerUpObject.transform.localScale;
    }

    IEnumerator OnObjectPickedUp()
    {
        Vector3 StartPos = PowerUpObject.transform.localPosition;
        Vector3 CurrentPos = StartPos;
        Vector3 EndPos = StartPos + new Vector3(0, 1, 0);

        // Reset rotation
        PowerUpObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Move it up 1f
        while (CurrentPos.y < EndPos.y)
        {
            CurrentPos = Vector3.MoveTowards(CurrentPos, EndPos, Time.deltaTime * 2);
            PowerUpObject.transform.localPosition = CurrentPos;
            PowerUpObject.transform.localScale *= 1.05f;

            yield return new WaitForSeconds(0.01f);
        }

        // Flash it 5 times
        SpriteRenderer SpriteR = PowerUpObject.GetComponent<SpriteRenderer>();
        for (int Counter = 0; Counter < 5; ++Counter)
        {
            SpriteR.enabled = false;
            yield return new WaitForSeconds(0.05f);
            SpriteR.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        gameObject.SetActive(false);
    }

    public void RenablePowerUp()
    {
        PowerUpObject.transform.localPosition = OriginalTransform[0];
        PowerUpObject.transform.localScale = OriginalTransform[1];

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        IsPickeUp = false;

        gameObject.SetActive(true);
    }

    void SwingGameObject()
    {
        PowerUpObject.transform.localPosition = new Vector3(0, SwingDistance.Update());
        PowerUpObject.transform.Rotate(0, 180 * Time.deltaTime, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!IsPickeUp)
            SwingGameObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When The player gets the power up!
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // Played the picked Up animation
            StartCoroutine(OnObjectPickedUp());

            // Notify the UI we have picked Up a component
            InventoryController.OnAddItemToInventory(GetPowerUpType());

            IsPickeUp = true;
        }
        else if (collision.tag == "Player" && collision is CircleCollider2D)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    
}
