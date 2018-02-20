using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Block {

    
	public string Name { get; protected set; }

    public Rect Rect { get; protected set; }

    public Action<Collision2D, Block> OnLand;

    public Dictionary<string, float> Parameters { get; protected set; }

    public Block(string name, Rect rect, Action<Collision2D, Block> onlands = null, Dictionary<string, float> parameters = null)
    {
        OnLand = onlands;
        Name = name;

        Rect = rect;

        Parameters = parameters == null ? new Dictionary<string, float>() : new Dictionary<string, float>(parameters);
    }

    Block(Block other)
    {
        Name = other.Name;
        Rect = other.Rect;
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
        Rect.setPosition(x, y);
    }

    public void ChangeParam(string paramName, float val)
    {
        Parameters[paramName] = val;
    }

}
