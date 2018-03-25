﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class DoorWarp : MonoBehaviour {

    public GameObject loadingImage;
    private RectTransform door;
    private Vector3[] boundary;
    private Transform PlayerTransform;

    public string VegetableName;
    public GameObject VegetablePatch;

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
                if(!VegetablePatch.activeInHierarchy)
                {
                    UpdateVeggieList(VegetableName);
                }
                loadingImage.SetActive(true);
                SceneManager.LoadScene("Level Select");
                Debug.Log("ding");
            }
        }
    }
    private void UpdateVeggieList(string Vegetable)
    {
        var path = Application.streamingAssetsPath + "/VeggieList.xml";
        XmlDocument VeggieList = new XmlDocument();
        StreamReader reader = new StreamReader(path);
        VeggieList.Load(reader);
        reader.Close();
        XmlNodeList Veggies = VeggieList.GetElementsByTagName("VeggieClass");
        foreach (XmlNode VeggieClass in Veggies)
        {
            XmlNodeList VeggieDetails = VeggieClass.ChildNodes;
            Debug.Log("POGGERS");
            foreach (XmlNode Detail in VeggieDetails)
            {
                if (Detail.InnerText == Vegetable)
                {
                    Debug.Log("POGGERS1");
                    foreach (XmlNode thing in VeggieDetails)
                    {
                        Debug.Log("POGGERS2");
                        if (thing.Name == "found")
                        {
                            Debug.Log("POGGERS3");
                            thing.InnerText = "true";
                        }
                    }
                }
            }
        }
        XmlSerializer serializer = new XmlSerializer(typeof(VeggieListClass));
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, VeggieList);
        writer.Close();
    }
}
