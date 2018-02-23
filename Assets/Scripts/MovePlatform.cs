using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    public Vector3 BasePosition;
    public Vector3 EndPosition;
    Vector3 movement;

    public float speedModifier;
    public Rigidbody2D block; //don't use this on the player you dipshit
    //the base position is the position of the block in the base scene.

    private bool EndSkip;
    private bool BaseSkip;
    private void Awake()
    {
        BasePosition = GetComponent<Transform>().position;
        movement = new Vector3((EndPosition.x - BasePosition.x) / speedModifier, (EndPosition.y - BasePosition.y) / speedModifier, 0);
        block = GetComponent<Rigidbody2D>();
        block.velocity = new Vector2(movement.x, movement.y);
    }

    void Update () {
        EndSkip = (Mathf.Abs(EndPosition.x - BasePosition.x) < Mathf.Abs(block.position.x - BasePosition.x) || Mathf.Abs(EndPosition.y - BasePosition.y) < Mathf.Abs(block.position.y - BasePosition.y));
        BaseSkip = (Mathf.Abs(EndPosition.x - BasePosition.x) < Mathf.Abs(block.position.x - EndPosition.x) || Mathf.Abs(EndPosition.y - BasePosition.y) < Mathf.Abs(block.position.y - EndPosition.y));
        if (EndSkip || BaseSkip)
        {
            block.velocity = new Vector2(-block.velocity.x, -block.velocity.y);
            Debug.Log("dafuq");
        }
	}
}
