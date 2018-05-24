using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour {

    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    public Vector2 Xmargin;
    public Vector2 Ymargin;
    public CameraController Cam;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When The player gets the power up!
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            Cam.Offset = Offset;
            Cam.Xmargin = Xmargin;
            Cam.Ymargin = Ymargin;
        }
    }
}
