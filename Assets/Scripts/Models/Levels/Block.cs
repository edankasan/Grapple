using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Block {

    
	public string Name { get; protected set; }

    public float X { get; protected set; }
    public float Y { get; protected set; }

    public Action<Collision2D, Block> OnLand;

    public Dictionary<string, float> Parameters { get; protected set; }

    public Block(string name, Action<Collision2D, Block> onlands, float x, float y, Dictionary<string, float> parameters = null)
    {
        OnLand = onlands;
        Name = name;

        X = x;
        Y = y;

        Parameters = parameters == null ? new Dictionary<string, float>() : new Dictionary<string, float>(parameters);
    }

    Block(Block other)
    {
        Name = other.Name;
        X = other.X;
        Y = other.Y;
        OnLand = other.OnLand;
        Parameters = new Dictionary<string, float>(other.Parameters);
    }

    public Block Clone()
    {
        return new Block(this);
    }

    public void SetPos(float x, float y)
    {
        // Maybe check bounds or idk...
        X = x;
        Y = y;
    }

    public void ChangeParam(string paramName, float val)
    {
        Parameters[paramName] = val;
    }

}
