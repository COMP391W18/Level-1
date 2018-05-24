using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAnimation : MonoBehaviour {

    [System.Serializable]
    public struct Swingable
    {
        public float Offset;
        public float Velocity;
        public Vector2 OriginalPos { get; set; }
        private float CurrentOffset;

        public float Update()
        {
            CurrentOffset = Mathf.MoveTowards(CurrentOffset, Offset, Velocity * Time.deltaTime);

            if (Mathf.Approximately(CurrentOffset, Offset))
                Offset = -Offset;

            return CurrentOffset + OriginalPos.y;
        }
    }

    // Swing propertis
    public Swingable SwingDistance;

    // Identify whatever we are picked up or not
    bool IsPickeUp = false;


    IEnumerator OnObjectPickedUp()
    {
        Vector3 StartPos = transform.localPosition;
        Vector3 CurrentPos = StartPos;
        Vector3 EndPos = StartPos + new Vector3(0, 1, 0);

        // Reset rotation
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Move it up 1f
        while (CurrentPos.y < EndPos.y)
        {
            CurrentPos = Vector3.MoveTowards(CurrentPos, EndPos, Time.deltaTime * 2);
            transform.localPosition = CurrentPos;
            transform.localScale *= 1.05f;

            yield return new WaitForSeconds(0.01f);
        }

        // Flash it 5 times
        SpriteRenderer SpriteR = GetComponent<SpriteRenderer>();
        for (int Counter = 0; Counter < 5; ++Counter)
        {
            SpriteR.enabled = false;
            yield return new WaitForSeconds(0.05f);
            SpriteR.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void SwingGameObject()
    {
        transform.localPosition = new Vector3(SwingDistance.OriginalPos.x, SwingDistance.Update());
        transform.Rotate(0, 180 * Time.deltaTime, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsPickeUp)
            SwingGameObject();
    }

    // Use this for initialization
    void Start () {
        // Get the original offset ogf the swingable gameobject
        SwingDistance.OriginalPos = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When The player gets the power up!
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            //GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // Played the picked Up animation
            StartCoroutine(OnObjectPickedUp());

            IsPickeUp = true;
        }
        else if (collision.tag == "Player" && collision is CircleCollider2D)
        {
            //GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
