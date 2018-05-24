using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    // The player to follow
    public GameObject Player;

    public float Speed = 1f;
    
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    public Vector2 Xmargin;
    public Vector2 Ymargin;

    // Use this for initialization
    void Start ()
    {
        // Cache the player to follow
        if (!Player)
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Change the offset
    public void ChangeOffset(Vector3 NewOffset)
    {
        Offset = NewOffset;
    }
    public void ChangeOffset(Vector2 NewMargim)
    {
        Xmargin = NewMargim;
    }

    public void UpdateCameraPos(Vector3 PlayerPos)
    {
        // Check a
        //if (Vector2.Distance(Camera.main.transform.position, PlayerPos) < 0.5f)
            //return;

        float ZValue = Camera.main.transform.position.z;
        Vector3 NewPos = Vector3.MoveTowards(Camera.main.transform.position, PlayerPos, Speed * Time.deltaTime) - Offset;
        Camera.main.transform.position = new Vector3(Mathf.Clamp(NewPos.x, Xmargin.x, Xmargin.y), Mathf.Clamp(NewPos.y, Ymargin.x, Ymargin.y), ZValue);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCameraPos(Player.transform.position);
    }
}
