using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; protected set; }

    //things related to basic player movement

    protected Rigidbody2D rb2d;
    public float jumpPower;
    public float moveSpeed;

    //end of things related to basic player movement

    //things related to the current player state

    protected bool grounded;
    protected Vector2 baseSpeed = new Vector2(0, 0); // the base speed is important for keeping the player in sync with any moving platform they may stand on

    //end of things related to the current player state

    //things related to the animations

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //end of things related to the animations

    public GameObject loadingImage;

    //things related to the grappling hook

    public GameObject grappleShooter;

    private DistanceJoint2D grapple;

    public LineRenderer lineRenderer;

    public Vector3 GrapplePullDirection;

    //end of things related to the grappling hook

    void Awake () //runs during the first frame the player is active.
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        initiateKeyControls();
    }
    public bool hasDistanceJoint2D(GameObject obj) //returns true if the object has a DistanceJoint2D, else false.
    {
        return obj.GetComponent<DistanceJoint2D>() != null;
    }

    public bool isGrounded() // if the player is on a platform returns true, else false.
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.down, 0.001f); 
        //checks if there is anything within 0.001 units below the player
        if(hit && hit.collider.name != "Player" && hit.collider.GetComponent<Rigidbody2D>()!= null)//updates the player's base speed to match where the player is.
        {
            baseSpeed = new Vector2(hit.collider.GetComponent<Rigidbody2D>().velocity.x, hit.collider.GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            baseSpeed = new Vector2(0, 0);
        }
        return (hit && hit.collider.name != "Player" && hit.collider.gameObject.layer != 9);
    }

    public void UpdateAnimator() //updates the animator.
    {
        animator.SetBool("grounded", grounded);
        animator.SetBool("GrappleOn", hasDistanceJoint2D(grappleShooter));
        bool flipSprite = (spriteRenderer.flipX ? (rb2d.velocity.x > 0.1f) : (rb2d.velocity.x < -0.1f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //when colliding with a platform makes sure to keep the basespeed of the player updated.
    {
            animator.SetFloat("velocityX", Mathf.Abs(collision.relativeVelocity.x));
        bool flipSprite = (spriteRenderer.flipX ? (collision.relativeVelocity.x > 0.1f) : (collision.relativeVelocity.x < -0.1f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        if (collision.collider.GetComponent<Rigidbody2D>() != null && collision.collider.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Dynamic)
        {
            if (collision.collider.GetComponent<Rigidbody2D>().velocity.x != 0 && (rb2d.velocity.x == 0 || rb2d.velocity.x == -collision.collider.GetComponent<Rigidbody2D>().velocity.x))
            {
                rb2d.velocity = new Vector2(collision.collider.GetComponent<Rigidbody2D>().velocity.x, rb2d.velocity.y);
            }
            baseSpeed = new Vector2(collision.collider.GetComponent<Rigidbody2D>().velocity.x, collision.collider.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void Update() // runs every frame.
    {
        if(Mathf.Abs(rb2d.velocity.y) < 0.0005)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        if (Mathf.Abs(rb2d.velocity.x) < 0.05)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        grounded = isGrounded();
        if(!grounded)
        {
            baseSpeed = new Vector2(0, 0);
        }
        UpdateAnimator();

        InputController.Instance.CheckForInput();

    }


    // On button funcs are functions that you run when a button is
    // pressed. It returns a boolean to tell you whether that button
    // wants to STOP input or not. For example, if pressing the W key
    // makes you jump and gamelogic dictates that you may NOT take an action
    // after jumping you might want to return TRUE so as to NOT accept any
    // more inputs.
    delegate bool OnButtonFunc();

    void initiateKeyControls() //sets up the controls.
    {
        //Here we will pass the inputcontroller our keyset and mouseset.
        Dictionary<KeyState, MouseManager.MouseFunc[]> mousePreset = new Dictionary<KeyState, MouseManager.MouseFunc[]>();
        Dictionary<KeyCode, KeyboardManager.OnButtonFunc>[] keyboardPreset = new Dictionary<KeyCode, KeyboardManager.OnButtonFunc>[3];
        keyboardPreset[0] = new Dictionary<KeyCode, KeyboardManager.OnButtonFunc>();
        keyboardPreset[1] = new Dictionary<KeyCode, KeyboardManager.OnButtonFunc>();
        keyboardPreset[2] = new Dictionary<KeyCode, KeyboardManager.OnButtonFunc>();

        #region setting keyboard presets

        keyboardPreset[1].Add(KeyCode.W, OnWHeld);
        keyboardPreset[1].Add(KeyCode.A, OnAHeld);
        keyboardPreset[1].Add(KeyCode.S, OnSHeld);
        keyboardPreset[1].Add(KeyCode.D, OnDHeld);
        keyboardPreset[1].Add(KeyCode.R, OnRHeld);

        keyboardPreset[2].Add(KeyCode.W, OnWReleased);
        keyboardPreset[2].Add(KeyCode.A, OnAReleased);
        keyboardPreset[2].Add(KeyCode.D, OnDReleased);
        #endregion

        InputController.Instance.KeyboardManager.InitiateKeyControls(keyboardPreset); //sends the keyboard control scheme to the KeyboardManager.

        //here we call the function that will set up the mouse controls
        #region setting mouse presets
        SetMouseControls(mousePreset);
        #endregion

        InputController.Instance.MouseManager.InitiateKeyControls(mousePreset); // sends the mouse control scheme to the MouseManager.

    }

    public void SetMouseControls(Dictionary<KeyState, MouseManager.MouseFunc[]> mousePreset) // sets up the mouse controls.
    {
        MouseManager.MouseFunc[] PressedMouseButtons = new MouseManager.MouseFunc[3];
        PressedMouseButtons[0] = OnLeftClickPressed;
        MouseManager.MouseFunc[] ReleasedMouseButtons = new MouseManager.MouseFunc[3];
        ReleasedMouseButtons[0] = OnLeftClickReleased;
        MouseManager.MouseFunc[] HeldMouseButtons = new MouseManager.MouseFunc[3];
        HeldMouseButtons[0] = OnLeftClickHeld;
        mousePreset[KeyState.Pressed] = PressedMouseButtons;
        mousePreset[KeyState.Held] = HeldMouseButtons;
        mousePreset[KeyState.Released] = ReleasedMouseButtons;
    }

    //here i have all the functions that will go to the KeyboardManager
    #region keyboard control funcs 
    
    public bool OnDHeld() //runs when the "D" button is held.
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(baseSpeed.x + moveSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.AddForce(new Vector2(3f, 0f));
        }

        return false;
    }

    public bool OnAHeld() //runs when the "A" button is held.
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(baseSpeed.x - moveSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.AddForce(new Vector2(-3f, 0f));
        }

        return false;
    }

    public bool OnWHeld() //runs when the "W" button is held.
    {
        if (grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower + baseSpeed.y);
        }
        else
        {
            if (hasDistanceJoint2D(grappleShooter))
            {
                grapple.distance = grapple.distance - 0.03f;
            }
        }

            return false;
    }

    public bool OnSHeld() //runs when the "S" button is held.
    {
        if(hasDistanceJoint2D(grappleShooter) && grapple.distance <= 6.97)
        {
            grapple.distance = grapple.distance + 0.03f;
        }
        return false;
    }

    public bool OnRHeld() //runs when the "R" button is held.
    {
        loadingImage.SetActive(true);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        return false;
    }

    public bool OnDReleased() //runs when the "D" button is released.
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnAReleased() //runs when the "A" button is released.
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnWReleased() //runs when the "W" button is released.
    {
        if (!hasDistanceJoint2D(grappleShooter) && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 1.25f);
        }

        return false;
    }

    #endregion

    #region mouse control funcs
    public void OnLeftClickPressed() //runs when the left mouse button is pressed. 
    //attempts to shoot the grappling hook in the direction of the mouse relative to the player.
    { 
        Vector3 mousePosition = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = grappleShooter.transform.position;
        Vector3 direction = mousePosition - position;
        GrapplePullDirection = direction;
        direction = Vector3.Scale(direction, new Vector3(1000, 1000, 1));
        Vector3 offset = Vector3.ClampMagnitude(direction, 1f);
        RaycastHit2D hit = Physics2D.Raycast(position + offset, direction, 7); //checks what's the closest platform in the direction of the click within range.
        if (hit.collider != null && hit.collider.gameObject.layer != 8) //if the closest platform is an eligible grapple target then creates the grappling hook.
        {
            DistanceJoint2D newGrapple = grappleShooter.AddComponent<DistanceJoint2D>();
            newGrapple.enableCollision = false;
            newGrapple.enabled = true;
            newGrapple.maxDistanceOnly = true;
            newGrapple.enableCollision = true;
            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                newGrapple.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                newGrapple.connectedAnchor = new Vector2((hit.point.x - newGrapple.connectedBody.transform.position.x) / newGrapple.connectedBody.transform.localScale.x, 
                (hit.point.y - newGrapple.connectedBody.transform.position.y) / newGrapple.connectedBody.transform.localScale.y);
                GameObject.DestroyImmediate(grapple);
                grapple = newGrapple;
            }
        }
    }
    public void OnLeftClickHeld() //runs when the left mouse button is held. updates the grappling hook graphics.
    {
        if (hasDistanceJoint2D(grappleShooter))
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            Vector3 position0 = new Vector3(grappleShooter.transform.position.x, grappleShooter.transform.position.y, -5);
            Vector3 position1;
            if (grapple != null)
            {
                float pos1X = grapple.connectedAnchor.x * grapple.connectedBody.transform.localScale.x + grapple.connectedBody.transform.position.x;
                float pos1Y = grapple.connectedAnchor.y * grapple.connectedBody.transform.localScale.y + grapple.connectedBody.transform.position.y;
                position1 = new Vector3(pos1X, pos1Y, -5);
                lineRenderer.SetPosition(0, position0);
                lineRenderer.SetPosition(1, position1);
            }
        }
    }
    public void OnLeftClickReleased() //runs when the left mouse button is released. destroys the grappling hook and the disables it's graphics.
    {
        GameObject.Destroy(grapple);
        lineRenderer.enabled = false;
    }
    #endregion



}
