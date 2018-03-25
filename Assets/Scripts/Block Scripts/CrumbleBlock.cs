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
            if(collision.collider.GetComponent<DistanceJoint2D>() != null)
            {
                float checkX = collision.collider.GetComponent<DistanceJoint2D>().connectedAnchor.x * collision.collider.GetComponent<DistanceJoint2D>().connectedBody.transform.localScale.x + collision.collider.GetComponent<DistanceJoint2D>().connectedBody.transform.position.x;
                float checkY = collision.collider.GetComponent<DistanceJoint2D>().connectedAnchor.y * collision.collider.GetComponent<DistanceJoint2D>().connectedBody.transform.localScale.y + collision.collider.GetComponent<DistanceJoint2D>().connectedBody.transform.position.y;
                Debug.Log("checks    " + checkX + "    " + checkY);
                Debug.Log("extents    " + collision.otherCollider.GetComponent<BoxCollider2D>().bounds.extents.x + "    " + collision.otherCollider.GetComponent<BoxCollider2D>().bounds.extents.y);
                if (collision.otherCollider.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(checkX,checkY,0)))
                {
                    Debug.Log("hookdestroyed");
                    GameObject.Destroy(collision.collider.GetComponent<DistanceJoint2D>());
                    collision.collider.GetComponent<LineRenderer>().enabled = false;
                }
            }
            gameObject.SetActive(false);
        }
        if (counter < 2)
        {
            counter++;
            GetComponent<SpriteRenderer>().sprite = order[counter];
        }
    }

}
