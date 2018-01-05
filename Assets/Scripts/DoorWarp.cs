using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWarp : MonoBehaviour {

    public GameObject loadingImage;
    private RectTransform door;
    private Vector3[] boundary;
    private Transform PlayerTransform;
    private void Start()
    {
        door = gameObject.GetComponent<RectTransform>();
        boundary = new Vector3[4];
        door.GetWorldCorners(boundary);
        GameObject isInBound = GameObject.Find("Player");
        PlayerTransform = isInBound.GetComponent<Transform>();
        Debug.Log("ding");
    }
    private void Update()
    {
        if(PlayerTransform.position.x >= boundary[0].x && PlayerTransform.position.x <= boundary[2].x)
        {
            if (PlayerTransform.position.y >= boundary[0].y && PlayerTransform.position.y <= boundary[2].y)
            {
                loadingImage.SetActive(true);
                SceneManager.LoadScene("Level Select");
                Debug.Log("ding");
            }
        }
    }
}
