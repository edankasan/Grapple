using System;
using System.Collections.Generic;

public class Level {

	public string Name { get; protected set; }
    public int Value { get; protected set; }

    public List<Block> Blocks { get; protected set; }

    public Action<Block> OnChanged;

    public Level(int val, string name, List<Block> blocks = null)
    {
        Value = val;
        Name = name;

        Blocks = blocks == null ? new List<Block>() : new List<Block>(blocks);
    }
}