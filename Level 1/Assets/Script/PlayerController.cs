using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    // RigidBodyReference
    Rigidbody2D RBody;

    // Sprite normal sprite
    public float WalkSpeed = 2f;
    public float RunningSpeed = 4f;

    private float CurrentSpeed;
    public float smoothTime = 0.3F;
    private float xVelocity = 0.0F;

    public Vector2 Velocity;
    public float JumpVelocity;
    public float Gravity;
    public BoxCollider2D HeadCollider;
    public LayerMask Maskes;

    private bool walk, walk_left, walk_right, jump;

    public LayerMask PowerBoxMask;

    public bool CanPlaceGate { get; set; }

    // Use this for initialization
    void Start()
    {
        // Cache the rigid body component
        RBody = GetComponent<Rigidbody2D>();
        CanPlaceGate = false;


//         JumpVelocity *= 0.32f;
//         Velocity *= 0.32f;
//         Gravity *= 0.32f;
    }

    public float Speed()
    {
        return Input.GetKey(KeyCode.LeftShift) ? RunningSpeed : WalkSpeed;
    }

    // Update the player position
    private void UpdatePlayerPosition()
    { 
        Vector3 pos = RBody.position;
        Vector3 scale = RBody.transform.localScale;

        if (walk)
        {
            if (walk_left)
            {
                pos.x -= Velocity.x * Time.deltaTime;
                scale.x = -1f;
            }
            if (walk_right)
            {
                pos.x += Velocity.x * Time.deltaTime;
                scale.x = 1f;
            }
        }
        /**/
        if (jump && Velocity.y < 0.4f)
        {
            Velocity = new Vector2(Velocity.x, JumpVelocity);

            pos.y += Velocity.y * Time.deltaTime;
        }
        else if (Velocity.y > 0f)
        {
            if (HeadCollider.IsTouchingLayers(Maskes))
            {
                Velocity.y = 0f;
            }
            else
            {
                Velocity.y -= Gravity * Time.deltaTime;
                Velocity.y = Mathf.Clamp(Velocity.y, 0, 50f);
            }

            pos.y += Velocity.y * Time.deltaTime;
        }

        // If I'm sitting on top of the platform move with the platform
        //if (Velocity.y == 0f && GetComponent<BoxCollider2D>().IsTouchingLayers(Maskes) && GetComponent<BoxCollider2D>().)
        
        RBody.position = pos;
        RBody.transform.localScale = scale;
        

        // Get how much we are moving left or right
        ///float Movement = Input.GetAxis("Horizontal");
        ///bool Jump = Input.GetKeyDown(KeyCode.Space);

        // Get the current position and update the pos
        ///Vector3 CurrPos = RBody.position;
        ///CurrPos += new Vector3(Movement, Jump ? 10f : 0f) * (Input.GetKey(KeyCode.LeftShift) ? RunningSpeed : WalkSpeed) * Time.deltaTime;
        ///RBody.position = CurrPos;


    }

    private void HandleGatePlacement()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<CircleCollider2D>().IsTouchingLayers(PowerBoxMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.5f, PowerBoxMask);

            if (hit.collider != null)
            {
                hit.collider.gameObject.GetComponent<GateController>().OnGatePlaced(InventoryController.GetSelectedItem());
                InventoryController.OnRemoveItemFromInventory(InventoryController.GetSelectedItem());

                foreach (GateController Gate in GateController.AllGateControllers)
                    if (Gate != null)
                        Gate.OnGatePlaced(Gate.PlacedGate);
            }
        }
    }
    void CheckPlayerInput()
    {
        bool input_left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool input_right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        bool input_space = Input.GetKeyDown(KeyCode.Space);
        //CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SpeedMultiplier : 1;

        walk = input_left || input_right;
        walk_left = input_left && !input_right;
        walk_right = !input_left && input_right;
        jump = input_space;

    }

    void UpdateAnimationStates()
    {
        if (Velocity.y == 0f && !walk)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        if (Velocity.y == 0f && walk)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }

        //if (playerState == PlayerState.jumping)
        if (Velocity.y < -0.6f || Velocity.y > 0.2f)
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        UpdatePlayerPosition();

        UpdateAnimationStates();

        HandleGatePlacement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we enter and enter teleport
        if (collision.gameObject.name == "Enter" && collision.IsTouching(GetComponent<BoxCollider2D>()))
        {
            RBody.position = collision.gameObject.transform.parent.GetChild(1).transform.position;
        }
    }
    
}
