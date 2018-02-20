using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameController Instance { get; protected set; }

    PlayerController playerController;

    InputController inputController;
    public Level currLevel;

	// Use this for initialization
	void Awake () {
        Instance = this;

        currLevel = LevelPrototypes.LevelProtos["FirstLevel"];

        playerController = new PlayerController();
        playerController.Start(currLevel.Player);

        inputController = InputController.Instance;
	}
	
	// Update is called once per frame
	void Update () {

        inputController.CheckForInput();
        playerController.Update();

	}


    
}
