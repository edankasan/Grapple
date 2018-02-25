using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonBehaviour : MonoBehaviour {

    public GameObject loadingImage;

	public void LoadScene(string level)
    {
        if (level == "current")
        {
            loadingImage.SetActive(true);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            loadingImage.SetActive(true);
            SceneManager.LoadScene(level);
        }
    }
}
