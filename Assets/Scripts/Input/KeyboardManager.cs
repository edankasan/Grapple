using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager {

    // On button funcs are functions that you run when a button is
    // pressed. It returns a boolean to tell you whether that button
    // wants to STOP input or not. For example, if pressing the W key
    // makes you jump and gamelogic dictates that you may NOT take an action
    // after jumping you might want to return TRUE so as to NOT accept any
    // more inputs.
    public delegate bool OnButtonFunc();
    List<KeyCode> RegisteredKeys = new List<KeyCode>();
    Dictionary<KeyCode, OnButtonFunc> pressedButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>();
    Dictionary<KeyCode, OnButtonFunc> heldButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>();
    Dictionary<KeyCode, OnButtonFunc> releasedButtonFuncs = new Dictionary<KeyCode, OnButtonFunc>();

    public void SetOnPressedKey(KeyCode key, OnButtonFunc func)
    {
        pressedButtonFuncs[key] = func;

        if (RegisteredKeys.Contains(key) == false)
            RegisteredKeys.Add(key);
    }

    public void SetOnHeldKey(KeyCode key, OnButtonFunc func)
    {
        heldButtonFuncs[key] = func;

        if (RegisteredKeys.Contains(key) == false)
            RegisteredKeys.Add(key);
    }

    public void SetOnReleasedKey(KeyCode key, OnButtonFunc func)
    {
        releasedButtonFuncs[key] = func;

        if (RegisteredKeys.Contains(key) == false)
            RegisteredKeys.Add(key);
    }

    public void InitiateKeyControls(Dictionary<KeyCode, OnButtonFunc>[] preset)
    {
        if (preset.Length != 3)
        {
            return;
        }

        foreach (KeyCode key in preset[0].Keys)
        {
            SetOnPressedKey(key, preset[0][key]);
        }

        foreach (KeyCode key in preset[1].Keys)
        {
            SetOnHeldKey(key, preset[1][key]);
        }

        foreach (KeyCode key in preset[2].Keys)
        {
            SetOnReleasedKey(key, preset[2][key]);
        }
    }

    public void CheckForInput()
    {
        foreach (KeyCode key in RegisteredKeys)
        {
            if (Input.GetKeyDown(key) == true && pressedButtonFuncs.ContainsKey(key) == true)
                if (pressedButtonFuncs[key].Invoke() == true)
                    break;

            if (Input.GetKey(key) == true && heldButtonFuncs.ContainsKey(key) == true)
                if (heldButtonFuncs[key].Invoke() == true)
                    break;

            if (Input.GetKeyUp(key) == true && releasedButtonFuncs.ContainsKey(key) == true)
                if (releasedButtonFuncs[key].Invoke() == true)
                    break;
        }
    }


}
