
using UnityEngine;
using System.Collections;

public class VeggieListClass
{
    public VeggieClass[] VeggieList;

    public VeggieListClass()
    {
        VeggieList = new VeggieClass[10];
        VeggieList[0] = new VeggieClass("Broccoli", false);
        VeggieList[1] = new VeggieClass("Carrot", false);
        VeggieList[2] = new VeggieClass("Corn", false);
        VeggieList[3] = new VeggieClass("Cucumber", false);
        VeggieList[4] = new VeggieClass("Eggplant", false);
        VeggieList[5] = new VeggieClass("Leek", false);
        VeggieList[6] = new VeggieClass("Mushroom", false);
        VeggieList[7] = new VeggieClass("Pepper", false);
        VeggieList[8] = new VeggieClass("Tomato", false);
        VeggieList[9] = new VeggieClass("Turnip", false);
    }
}
