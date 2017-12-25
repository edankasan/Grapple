using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public Vector3 GrapplePullDirection;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //grappling hook stuff
    public GameObject grappleShooter;

    private SpringJoint2D grapple;

    public LineRenderer lineRenderer;

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }   

    protected override void ComputeVelocity()
    {
        if (!hasSpringJoint2D(grappleShooter))
        {
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded == true)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                    velocity.y = velocity.y * 0.5f;
            }
            if (velocity.y <= 0)
            {
                gravityModifier = 2.5f;
            }
            else
            {
                gravityModifier = 1;
            }

            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            targetVelocity = move * maxSpeed;
        }
        else
        {
            velocity.x = grapple.connectedAnchor.x - rb2d.position.x;
            velocity.y = grapple.connectedAnchor.y - rb2d.position.y;
            targetVelocity.x = grapple.connectedAnchor.x - rb2d.position.x;
            targetVelocity.y = grapple.connectedAnchor.y - rb2d.position.y;
            if (velocity.x < 1.5f && velocity.x > -1.5 && velocity.x != 0)
            {
                if(velocity.x < 0.03f && velocity.x > -0.03f)
                {
                    velocity.x = 0;
                    targetVelocity.x = 0;
                }
                if (velocity.x < 0)
                {
                    velocity.x = -1.5f;
                    targetVelocity.x = -1.5f;
                }
                if (velocity.x > 0)
                {
                    velocity.x = 1.5f;
                    targetVelocity.x = 1.5f;
                }
            }
            if (velocity.y < 1.5f && velocity.y > -1.5 && velocity.y != 0)
            {
                if (velocity.y < .15f && velocity.y > -.15 && velocity.y != 0)
                {
                    velocity.y = 0;
                    targetVelocity.y = 0;
                }
                if (velocity.y < 0)
                {
                    velocity.y = -1.5f;
                    targetVelocity.y = -1.5f;
                }
                if (velocity.y > 0)
                {
                    velocity.y = 1.5f;
                    targetVelocity.y = 1.5f;
                }
            }
        }

        animator.SetBool("grounded", grounded);
        animator.SetBool("GrappleOn", hasSpringJoint2D(grappleShooter));
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x / maxSpeed));

    }
    void LateUpdate() //used for the grappling hook
    {
        if(Input.GetMouseButtonDown(0) && grounded)
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

        if (hit.collider != null)
        {
            SpringJoint2D newGrapple = grappleShooter.AddComponent<SpringJoint2D>();
            newGrapple.enableCollision = false;
            newGrapple.frequency = 0.2f; //this doesn't really matter, but why not
            newGrapple.connectedAnchor = hit.point;
            newGrapple.enabled = true;

            GameObject.DestroyImmediate(grapple);
            grapple = newGrapple;
        }
    }
    public static bool hasSpringJoint2D(GameObject obj)
    {
        return obj.GetComponent<SpringJoint2D>() != null;
    }
}

