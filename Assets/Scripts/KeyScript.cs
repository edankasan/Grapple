using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    public GameObject Key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Key.SetActive(false);
    }
}
