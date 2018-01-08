using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected Rigidbody2D rb2d;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        float movex = Input.GetAxis("Horizontal");
        if (movex > 0)
            rb2d.velocity = new Vector2(5, rb2d.velocity.y);
        else if (movex < 0)
            rb2d.velocity = new Vector2(-5, rb2d.velocity.y);
        else
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }
}
