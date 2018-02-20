using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Player
{
    public string Name { get; protected set; }

    public Rect Rect { get; protected set; }

    public Player(Rect rect, string name)
    {
        Name = name;
        Rect = rect;
    }
}
