using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; protected set; }

    public float jumpPower;
    public float moveSpeed;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected Rigidbody2D rb2d;

    public GameObject loadingImage;

    //grappling hook stuff

    public GameObject grappleShooter;

    private DistanceJoint2D grapple;

    public LineRenderer lineRenderer;

    public Vector3 GrapplePullDirection;

    void Awake () {
        Instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        initiateKeyControls();
    }

    public bool hasDistanceJoint2D(GameObject obj)
    {
        return obj.GetComponent<DistanceJoint2D>() != null;
    }

    public void UpdateAnimator()
    {
        animator.SetBool("grounded", grounded);
        animator.SetBool("GrappleOn", hasDistanceJoint2D(grappleShooter));
        animator.SetFloat("velocityX", Mathf.Abs(rb2d.velocity.x / moveSpeed));
        bool flipSprite = (spriteRenderer.flipX ? (rb2d.velocity.x > 0.01f) : (rb2d.velocity.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }


    void Update()
    {
        if(Mathf.Abs(rb2d.velocity.y) < 0.0005)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        if (Mathf.Abs(rb2d.velocity.x) < 0.05)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        grounded = false;
            if (rb2d.velocity.y == 0)
            {
                grounded = true;
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
    Dictionary<KeyCode, OnButtonFunc> heldButtonFuncs;
    Dictionary<KeyCode, OnButtonFunc> releasedButtonFuncs;

    void initiateKeyControls()
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

        InputController.Instance.KeyboardManager.InitiateKeyControls(keyboardPreset);

        #region setting mouse presets
        SetMouseControls(mousePreset);
        #endregion

        InputController.Instance.MouseManager.InitiateKeyControls(mousePreset);

    }

    public void SetMouseControls(Dictionary<KeyState, MouseManager.MouseFunc[]> mousePreset)
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

    #region keyboard control funcs
    public bool OnDHeld()
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.AddForce(new Vector2(3f, 0f));
        }

        return false;
    }

    public bool OnAHeld()
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.AddForce(new Vector2(-3f, 0f));
        }

        return false;
    }

    public bool OnWHeld()
    {
        if (grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
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

    public bool OnSHeld()
    {
        if(hasDistanceJoint2D(grappleShooter) && rb2d.position.y < grapple.connectedAnchor.y && grapple.distance <= 6.97)
        {
            grapple.distance = grapple.distance + 0.03f;
        }

        return false;
    }

    public bool OnRHeld()
    {
        loadingImage.SetActive(true);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        return false;
    }

    public bool OnAReleased()
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnDReleased()
    {
        if (!hasDistanceJoint2D(grappleShooter) || grounded)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnWReleased()
    {
        if (!hasDistanceJoint2D(grappleShooter) && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 1.25f);
        }

        return false;
    }

    #endregion

    #region mouse control funcs
    public void OnLeftClickPressed()
    { 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = grappleShooter.transform.position;
        Vector3 direction = mousePosition - position;
        GrapplePullDirection = direction;
        direction = Vector3.Scale(direction, new Vector3(1000, 1000, 1));
        Vector3 offset = Vector3.ClampMagnitude(direction, 1.5f);
        RaycastHit2D hit = Physics2D.Raycast(position + offset, direction, 7);
        if (hit.collider != null && hit.collider.gameObject.layer != 8)
        {
            DistanceJoint2D newGrapple = grappleShooter.AddComponent<DistanceJoint2D>();
            newGrapple.enableCollision = false;
            newGrapple.enabled = true;
            newGrapple.maxDistanceOnly = true;
            newGrapple.enableCollision = true;
            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                if (hit.collider.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
                {
                    newGrapple.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                    Debug.Log("ding1");
                }
                else
                {
                    newGrapple.connectedAnchor = hit.point;
                    Debug.Log("ding2");
                }
                GameObject.DestroyImmediate(grapple);
                grapple = newGrapple;
                Debug.Log("ding");
            }
        }
    }
    public void OnLeftClickHeld()
    {
        if (hasDistanceJoint2D(grappleShooter))
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            Vector3 position0 = new Vector3(grappleShooter.transform.position.x, grappleShooter.transform.position.y, -5);
            Vector3 position1;
            if (grapple != null)
            {
                if (grapple.connectedBody != null)
                {
                    position1 = new Vector3(grapple.connectedBody.position.x, grapple.connectedBody.position.y, -5);
                }
                else
                {
                    position1 = new Vector3(grapple.connectedAnchor.x, grapple.connectedAnchor.y, -5);
                }
                lineRenderer.SetPosition(0, position0);
                lineRenderer.SetPosition(1, position1);
            }
        }
    }
    public void OnLeftClickReleased()
    {
        GameObject.Destroy(grapple);
        lineRenderer.enabled = false;
    }
    #endregion



}
