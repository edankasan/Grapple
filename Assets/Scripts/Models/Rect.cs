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

    public void setPosition(float x, float y)
    {
        X = x;
        Y = y;
    }

    public bool Contains(float x, float y)
    {
        if ((Width / 2) + X > x && x < X - (Width / 2) && y > Y - (Height / 2) && y < Y + (Height / 2))
            return true;

        return false;
    }

    public bool CollidingWith(Rect rect)
    {
        float distanceX = this.X - rect.X;
        if (distanceX < 0)
        {
            distanceX = distanceX * -1;
        }
        float distanceY = this.Y - rect.Y;
        if (distanceY < 0)
        {
            distanceY = distanceY * -1;
        }
        bool CollidingOrOverlapping = ((distanceX <= (this.Width/2 + rect.Width/2)) && (distanceY <= (this.Height / 2 + rect.Height / 2)));
        bool Colliding = ((distanceX == (this.Width / 2 + rect.Width / 2)) && (distanceY == (this.Height / 2 + rect.Height / 2)));
        return false;
    }
}
