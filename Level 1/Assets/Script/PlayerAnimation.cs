using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public float jumpVelocity;
    public Vector2 velocity;
    public float gravity;
    public LayerMask wallMask;
    public LayerMask CeilingMask;
    public LayerMask floorMask;
    public LayerMask PlatformMask;
    public BoxCollider2D BCollider;

    //public float SpeedMultiplier = 1.5f;
    //public float CurrentSpeed = 1;

    private bool walk, walk_left, walk_right, jump;

    public enum PlayerState
    {
        jumping,
        idle,
        walking
    }

    private PlayerState playerState = PlayerState.idle;

    private bool grounded = false;

    // Use this for initialization
    void Start()
    {
        //Fall();

        jumpVelocity *= 0.32f;
        velocity *= 0.32f;
        gravity *= 0.32f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.CurrentGameState == GameController.GameState.Running)
        {
            CheckPlayerInput();
            UpdatePlayerPosition();
            UpdateAnimationStates();
        }
    }

    void UpdatePlayerPosition()
    {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk)
        {
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime /** CurrentSpeed*/;
                scale.x = -1f;
            }
            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime /** CurrentSpeed*/;
                scale.x = 1f;
            }

            pos = CheckWallRays(pos, scale.x);
        }

        if (jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if (playerState == PlayerState.jumping)
        {
            pos.y += velocity.y * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }

        if (velocity.y <= 0)
            pos = CheckFloorRays(pos);

        if (velocity.y > 0)
            pos = CheckCeilingRays(pos);

        transform.localPosition = pos;
        transform.localScale = scale;
    }

    void UpdateAnimationStates()
    {
        if (grounded && !walk)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        if (grounded && walk)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }

        //if (playerState == PlayerState.jumping)
        if (velocity.y < -0.6f || velocity.y > 0.2f)
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
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

    Vector3 CheckWallRays(Vector3 pos, float direction)
    {
        Vector2 originTop = new Vector2(pos.x + direction * 0.08f, pos.y + 0.32f - 0.2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * 0.08f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * 0.08f, pos.y - 0.32f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            pos.x -= velocity.x * Time.deltaTime * direction /** CurrentSpeed*/;

            //Debug.Log(hitRay.point);
        }
        return pos;
    }

    Vector3 CheckFloorRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.16f, pos.y - 0.32f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - 0.32f);
        Vector2 originRight = new Vector2(pos.x + 0.16f, pos.y - 0.32f);

        // 
        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        RaycastHit2D PlatformLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, PlatformMask);
        RaycastHit2D PlatformMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, PlatformMask);
        RaycastHit2D PlatformRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, PlatformMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null ||
            PlatformLeft.collider != null || PlatformMiddle.collider != null || PlatformRight.collider != null)
        {
            RaycastHit2D hitRay = floorRight;
            if (floorLeft)
            {
                hitRay = floorLeft;
            }
            else if (floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (floorRight)
            {
                hitRay = floorRight;
            }
            else if (PlatformLeft)
            {
                hitRay = floorLeft;
            }
            else if (PlatformMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (PlatformRight)
            {
                hitRay = floorRight;
            }

            playerState = PlayerState.idle;
            grounded = true;
            velocity.y = 0;

            pos.y = hitRay.point.y + 0.32f;
        }
        else
        {
            if (playerState != PlayerState.jumping)
            {
                Fall();
            }
        }
        return pos;
    }

    Vector3 CheckCeilingRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.16f, pos.y + 0.32f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y + 0.32f);
        Vector2 originRight = new Vector2(pos.x + 0.16f, pos.y + 0.32f);

        RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, CeilingMask);
        RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, CeilingMask);
        RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, CeilingMask);

        if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null)
        {
            RaycastHit2D hitRay = ceilLeft;

            if (ceilLeft)
            {
                hitRay = ceilLeft;
            }
            else if (ceilMiddle)
            {
                hitRay = ceilMiddle;
            }
            else if (ceilRight)
            {
                hitRay = ceilRight;
            }

            pos.y -= hitRay.distance;
            //pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - 1;

            Fall();
        }

        return pos;
    }

    void Fall()
    {
        velocity.y = 0;
        playerState = PlayerState.jumping;
        grounded = false;
    }
}
