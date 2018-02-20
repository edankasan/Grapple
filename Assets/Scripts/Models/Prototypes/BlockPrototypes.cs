using System;
using System.Collections.Generic;
using UnityEngine;

public static class BlockPrototypes {

    public static Dictionary<string, Block> BlockProtos { get; private set; }

    static BlockPrototypes()
    {
        BlockProtos = CreateBlockPrototypes();
    }

    static Dictionary<string, Block> CreateBlockPrototypes()
    {
        Dictionary<string, Block> blocks = new Dictionary<string, Block>();

        #region JumpPad

        Action<Collision2D, Block> jumpAction;

        jumpAction = (collision, block) =>
            {

                Debug.Log("ding");
                float jumpVal;

                if (block.Parameters.ContainsKey("JumpStrength") == false)
                    jumpVal = 5f;
                else
                    jumpVal = block.Parameters["JumpStrength"];

                ContactPoint2D test = collision.contacts[0];
                collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(-(test.normal.x * jumpVal), -(test.normal.y * jumpVal));
            };


        Dictionary<string, float> parameters = new Dictionary<string, float>();
        parameters["JumpStrength"] = 5f;

        blocks.Add("JumpPad", new Block(
            "JumpPad",
            jumpAction,
            0,
            0,
            parameters
            ));

        #endregion

        return blocks;
    }
    
    public static Block GetBlock(string name)
    {
        if (BlockProtos.ContainsKey(name) == false)
            return null;

        return BlockProtos[name].Clone();
    }
}
