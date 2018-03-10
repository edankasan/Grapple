using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleBlock : MonoBehaviour {

    public Sprite complete;
    public Sprite broken;
    public Sprite crumbling;
    public Sprite[] order;
    public int counter = 0;
	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = complete;
        order = new Sprite[3];
        order[0] = complete;
        order[1] = broken;
        order[2] = crumbling;
        GetComponent<SpriteRenderer>().sprite = order[counter];
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (counter == 2)
        {
            gameObject.SetActive(false);
            if(collision.collider.GetComponent<DistanceJoint2D>() != null)
            {
                GameObject.Destroy(collision.collider.GetComponent<DistanceJoint2D>());
                collision.collider.GetComponent<LineRenderer>().enabled = false;
            }
        }
        if (counter < 2)
        {
            counter++;
            GetComponent<SpriteRenderer>().sprite = order[counter];
        }
    }

}
