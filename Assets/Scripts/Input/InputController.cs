using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController {

    static InputController _Instance;
    
    public static InputController Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new InputController();
            }

            return _Instance;
        }
    }
    
    public MouseManager MouseManager { get; protected set; }
    public KeyboardManager KeyboardManager { get; protected set; }


    protected InputController()
    {
        MouseManager = new MouseManager();
        KeyboardManager = new KeyboardManager();
	}
	

	public void CheckForInput()
    {
        MouseManager.CheckMouseContext();
        MouseManager.CheckForInput();
        KeyboardManager.CheckForInput();
	}
}
