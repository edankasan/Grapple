using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer : MonoBehaviour {

    public string Veggie;
	// Use this for initialization
	private void Start () {
        VeggieListClass VeggieList = new VeggieListClass();

        XmlSerializer serializer = new XmlSerializer(typeof(VeggieListClass));
        var path = Application.streamingAssetsPath + "/VeggieList.xml";
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, VeggieList);
        writer.Close();
	}
}
