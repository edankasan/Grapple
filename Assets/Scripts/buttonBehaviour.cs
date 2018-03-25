using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;

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
    public void newGame()
    {
        VeggieListClass VeggieList = new VeggieListClass();

        XmlSerializer serializer = new XmlSerializer(typeof(VeggieListClass));
        var path = Application.streamingAssetsPath + "/VeggieList.xml";
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, VeggieList);
        writer.Close();
        loadingImage.SetActive(true);
        SceneManager.LoadScene("Level Select");
    }
    public void ToggleCamera(GameObject PlayerFollower)
    {
        if(PlayerFollower.activeInHierarchy)
        {
            PlayerFollower.SetActive(false);
        }
        else
        {
            PlayerFollower.SetActive(true);
        }
    }
}
