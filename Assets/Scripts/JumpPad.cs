using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    protected Rigidbody2D rb2d;
    public float jumppower;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ding");
        collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.otherCollider.GetComponent<Rigidbody2D>().velocity.x, collision.otherCollider.GetComponent<Rigidbody2D>().velocity.y + jumppower);
    }
}
