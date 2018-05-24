using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour {

    // The location of the enter and exit port
    public BoxCollider2D EntryPort;
    public BoxCollider2D ExitPort;

    // Use this for initialization
    void Start ()
    {
        // Cache the entry and exit port
        if (!EntryPort)
            EntryPort = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();

        if (!ExitPort)
            ExitPort = gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>();
    }
	
}
