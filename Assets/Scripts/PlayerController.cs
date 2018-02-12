using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float minGroundNormalY = 0.65f;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        initiateKeyControls();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        if (grapple != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            Vector3 position0 = new Vector3(grappleShooter.transform.position.x, grappleShooter.transform.position.y, -5);
            Vector3 position1 = new Vector3(grapple.connectedAnchor.x, grapple.connectedAnchor.y, -5);
            lineRenderer.SetPosition(0, position0);
            lineRenderer.SetPosition(1, position1);
            if (Input.GetMouseButtonUp(0))
                GameObject.DestroyImmediate(grapple);
        }
        else
            lineRenderer.enabled = false;
    }
    void Fire()
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
            newGrapple.connectedAnchor = hit.point;
            newGrapple.enabled = true;
            newGrapple.maxDistanceOnly = true;

            GameObject.DestroyImmediate(grapple);
            grapple = newGrapple;
        }
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

    #region TestRegion

    void Update()
    {
        if (!hasDistanceJoint2D(grappleShooter))
        {
            grounded = false;
            if (rb2d.velocity.y == 0)
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }
        UpdateAnimator();
        foreach (KeyCode key in heldButtonFuncs.Keys)
        {
            if (Input.GetKey(key) == true)
                if(heldButtonFuncs[key].Invoke() == true)
                    break;

            if (Input.GetKeyUp(key) == true && releasedButtonFuncs.ContainsKey(key) == true)
                if (releasedButtonFuncs[key].Invoke() == true)
                    break;
        }

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

    void initiateKeyControls(Dictionary<KeyCode, OnButtonFunc> preset = null)
    {
        if (preset != null)
        {
            heldButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>(preset);
            return;
        }

        heldButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>();
        releasedButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>();

        heldButtonFuncs[KeyCode.W] = OnWHeld;
        heldButtonFuncs[KeyCode.A] = OnAHeld;
        heldButtonFuncs[KeyCode.D] = OnDHeld;
        heldButtonFuncs[KeyCode.S] = OnSHeld;
        heldButtonFuncs[KeyCode.R] = OnRHeld;
        releasedButtonFuncs[KeyCode.A] = OnAReleased;
        releasedButtonFuncs[KeyCode.D] = OnDReleased;
        releasedButtonFuncs[KeyCode.W] = OnWReleased;
    }

    public bool OnDHeld()
    {
        if (!hasDistanceJoint2D(grappleShooter))
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
        if (!hasDistanceJoint2D(grappleShooter))
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
        if (!hasDistanceJoint2D(grappleShooter) && grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
        }
        else
        {
            grapple.distance = grapple.distance - 0.03f;
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
        if (!hasDistanceJoint2D(grappleShooter))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnDReleased()
    {
        if (!hasDistanceJoint2D(grappleShooter))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        return false;
    }

    public bool OnWReleased()
    {
        if (!hasDistanceJoint2D(grappleShooter) && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 1.5f);
        }

        return false;
    }

    #endregion


}
