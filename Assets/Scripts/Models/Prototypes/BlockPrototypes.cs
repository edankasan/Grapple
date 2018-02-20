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

        Dictionary<string, float> jumpParameters = new Dictionary<string, float>();
        jumpParameters["JumpStrength"] = 5f;

        blocks.Add("JumpPad", new Block(
            "JumpPad",
            new Rect(0, 0, 1, 1),
            jumpAction,
            jumpParameters
            ));

        #endregion

        #region CrumblingBlock

        Action<Collision2D, Block> crumbleAction;
        crumbleAction = (collision, block) =>
        {
            float counter;
            if (block.Parameters.ContainsKey("CrumbleCounter") && block.Parameters["CrumbleCounter"] > 0)
                counter = block.Parameters["CrumbleCounter"];
            else
            {
                counter = 3f;
            }
            if (counter >= 0)
            {
                //destroy the block here;
                if (collision.collider.GetComponent<DistanceJoint2D>() != null)
                {
                    GameObject.Destroy(collision.collider.GetComponent<DistanceJoint2D>());
                    collision.collider.GetComponent<LineRenderer>().enabled = false;
                }
            }
            if (counter < 0)
            {
                counter++;
            }
        };
        Dictionary<string, float> crumbleParameters = new Dictionary<string, float>();
        crumbleParameters["CrumbleCounter"] = 3f;

        blocks.Add("CrumbleBlock", new Block(
            "CrumbleBlock",
            new Rect(0, 0, 1, 1),
            crumbleAction,
            crumbleParameters
            ));
        #endregion

        #region basic block
        blocks.Add("BasicBlock", new Block(
            "BasicBlock",
            new Rect(0, 0, 1, 1),
            null,
            null
            ));
        #endregion

        #region steel block
        blocks.Add("SteelBlock", new Block(
            "SteelBlock",
            new Rect(0, 0, 1, 1),
            null,
            null
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
