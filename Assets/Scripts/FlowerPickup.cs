using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPickup : MonoBehaviour {
    public GameObject flowerText;
    private Rigidbody2D rb2d;
	// Use this for initialization
	void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb2d.gameObject.SetActive(false);
        flowerText.SetActive(true);
    }
    private IEnumerator FlowerMessageDelay()
    {
        yield return new WaitForSecondsRealtime(5.0f);
    }
}
