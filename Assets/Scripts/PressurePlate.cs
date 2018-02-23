using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    public GameObject reactive;
    public bool reactiveState;

    public void Awake()
    {
        reactiveState = reactive.active;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        reactive.SetActive(!reactiveState);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        reactive.SetActive(reactiveState);
    }

}
