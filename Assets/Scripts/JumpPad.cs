using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    protected Rigidbody2D rb2d;
    public float jumppower;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ding");
        ContactPoint2D test = collision.contacts[0];
        collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(-(test.normal.x * jumppower), -(test.normal.y * jumppower));
    }
}
