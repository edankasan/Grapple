using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public class XMLParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var path = Application.streamingAssetsPath + "/VeggieList.xml";
        XmlDocument VeggieList = new XmlDocument();
        StreamReader reader = new StreamReader(path, new UTF8Encoding(false));
        VeggieList.Load(reader);
        reader.Close();
        XmlNodeList Veggies = VeggieList.GetElementsByTagName("VeggieClass");
        foreach(XmlNode VeggieClass in Veggies)
        {
            XmlNodeList VeggieDetails = VeggieClass.ChildNodes;
            foreach(XmlNode Detail in VeggieDetails)
            {
                if(Detail.Name == "name")
                {
                    Debug.Log(Detail.InnerText);
                }
            }
        }
    }
}
