using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    public GameObject reactive;
    public bool reactiveState;
    public Sprite on;
    public Sprite off;
    public void Awake()
    {
        reactiveState = reactive.activeInHierarchy;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        reactive.SetActive(!reactiveState);
        GetComponent<SpriteRenderer>().sprite = on;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        reactive.SetActive(reactiveState);
        GetComponent<SpriteRenderer>().sprite = off;
    }

}
