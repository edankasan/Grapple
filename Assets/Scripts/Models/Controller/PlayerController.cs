using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController {

    public Player Player { get; protected set; }




	// Use this for initialization
	public void Start (Player player) {
        Player = player;
	}
	
	// Update is called once per frame
	public void Update () {
		
	}
    
}
