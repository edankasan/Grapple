using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Rect
{
    public float Height { get; protected set; }
    public float Width { get; protected set; }

    public float X { get; protected set; }
    public float Y { get; protected set; }

    public Rect(float x, float y, float height, float width)
    {
        X = x;
        Y = y;

        Width = width;
        Height = height;
    }

    public bool Contains(float x, float y)
    {
        if ((Width / 2) + X > x && x < X - (Width / 2) && y > Y - (Height / 2) && y < Y + (Height / 2))
            return true;

        return false;
    }

    public bool Contains(Rect rect)
    {
    }
}
