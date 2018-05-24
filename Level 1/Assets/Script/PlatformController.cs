using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
   
    // Start and end position
    public Vector3 PosA;
    public Vector3 PosB;

    float OldPlayerPos;
    float NewPlayerPos;

    // Platform speed
    public float Speed;

    bool HasPlayerOnTop = false;

    // Current pos
    private Vector3 CurrentPos;
    private Vector3 NextDestin;
    
    // Use this for initialization
    void Start()
    {
        // The current position
        CurrentPos = PosA;

        // Set the destination
        NextDestin = PosB;

        HasPlayerOnTop = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current position
        CurrentPos = Vector3.MoveTowards(CurrentPos, NextDestin, Speed * Time.deltaTime);

        // Make sure the destination is the right one
        if (Vector3.Distance(CurrentPos, NextDestin) <= 0.1)
        {
            NextDestin = NextDestin != PosA ? PosA : PosB;
        }

        transform.position = CurrentPos;

        // Attach the player to the 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision is BoxCollider2D)
        {
            OldPlayerPos = transform.position.x;
            NewPlayerPos = transform.position.x;

            Debug.Log("Player Entered!");
        }
    }

    /**/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision is BoxCollider2D)
        {
            NewPlayerPos = transform.position.x;

            Vector3 PlayerPos = collision.transform.position;
            PlayerPos.x += (NewPlayerPos - OldPlayerPos);
            collision.transform.position = PlayerPos;

            OldPlayerPos = NewPlayerPos;

            Debug.Log("Player Stay!" + (NewPlayerPos - OldPlayerPos)); ;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision is BoxCollider2D)
        {
            OldPlayerPos = 0;
            NewPlayerPos = 0;

            Debug.Log("Player Left!");
        }
        
    }
}
