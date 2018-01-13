using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //grappling hook stuff

    public GameObject grappleShooter;

    private DistanceJoint2D grapple;

    public LineRenderer lineRenderer;

    public Vector3 GrapplePullDirection;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (!hasDistanceJoint2D(grappleShooter))
        {
            grounded = false;
            if (rb2d.velocity.y == 0)
            {
                grounded = true;
            }
            float movex = Input.GetAxisRaw("Horizontal");
            if (movex > 0)
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            else if (movex < 0)
                rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            else
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            }
            if (Input.GetButtonUp("Jump") && !grounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
        }
        else
        {
            float movex = Input.GetAxisRaw("Horizontal");
            if (movex > 0)
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            else if (movex < 0)
                rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            else
                rb2d.velocity = new Vector2(0, 0);
            float adjustRope = Input.GetAxisRaw("Vertical");
            if (adjustRope > 0)
                grapple.distance = grapple.distance - 0.03f;
            else if(adjustRope < 0 && rb2d.position.y < grapple.connectedAnchor.y)
                grapple.distance = grapple.distance + 0.03f;
        }
        animator.SetBool("grounded", grounded);
        animator.SetBool("GrappleOn", hasDistanceJoint2D(grappleShooter));
        animator.SetFloat("velocityX", Mathf.Abs(rb2d.velocity.x / moveSpeed));
    }
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
        RaycastHit2D hit = Physics2D.Raycast(position + offset, direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.layer != 8)
        {
            DistanceJoint2D newGrapple = grappleShooter.AddComponent<DistanceJoint2D>();
            newGrapple.enableCollision = false;
            newGrapple.connectedAnchor = hit.point;
            newGrapple.enabled = true;

            GameObject.DestroyImmediate(grapple);
            grapple = newGrapple;
        }
    }
    public static bool hasDistanceJoint2D(GameObject obj)
    {
        return obj.GetComponent<DistanceJoint2D>() != null;
    }
}
