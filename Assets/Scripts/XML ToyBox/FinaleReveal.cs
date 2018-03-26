using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public class FinaleReveal : MonoBehaviour {
    public GameObject FinaleButton;
    private bool RevealTime;
    
    void Start()
    {
        RevealTime = true;
        var path = Application.streamingAssetsPath + "/VeggieList.xml";
        XmlDocument VeggieList = new XmlDocument();
        StreamReader reader = new StreamReader(path, new UTF8Encoding(false));
        VeggieList.Load(reader);
        reader.Close();
        XmlNodeList Veggies = VeggieList.GetElementsByTagName("VeggieClass");
        foreach (XmlNode VeggieClass in Veggies)
        {
            XmlNodeList VeggieDetails = VeggieClass.ChildNodes;
            foreach (XmlNode Detail in VeggieDetails)
            {
                if (Detail.Name == "found")
                {
                    if(Detail.InnerText != "true")
                    {
                        RevealTime = false;
                    }
                }
            }
        }
            FinaleButton.SetActive(RevealTime);
    }
}
