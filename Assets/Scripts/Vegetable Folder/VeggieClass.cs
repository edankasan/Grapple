using UnityEngine;
using System.Collections;

public class VeggieClass
{
    public string name;
    public bool found;
    
    public VeggieClass()
    {
        name = "N/A. YIKES";
        found = false;
    }

    public VeggieClass(string VeggieName, bool VeggieFound)
    {
        name = VeggieName;
        found = VeggieFound;
    }
}
