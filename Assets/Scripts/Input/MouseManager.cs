using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseManager {

    public delegate void MouseFunc();

    Dictionary<KeyState, MouseFunc[]> Funcs = new Dictionary<KeyState, MouseFunc[]>();

    public void SetMouseFunc(int mouseKey, MouseFunc func, KeyState state)
    {
        if (mouseKey < 0 || mouseKey > 2)
        {
            Debug.LogError("WRITE ME!");
            return;
        }

        Funcs[state][mouseKey] = func;
    }

    public void CheckMouseContext()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


    }

    public void CheckForInput()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButtonDown(i) == true && Funcs[KeyState.Pressed][i] != null)
                Funcs[KeyState.Pressed][i].Invoke();

            if (Input.GetMouseButton(i) == true && Funcs[KeyState.Held][i] != null)
                Funcs[KeyState.Held][i].Invoke();
            
            if (Input.GetMouseButtonUp(i) == true && Funcs[KeyState.Released][i] != null)
                Funcs[KeyState.Released][i].Invoke();
        }
    }

    public void InitiateKeyControls(Dictionary<KeyState, MouseFunc[]> preset)
    {
        Funcs = new Dictionary<KeyState, MouseFunc[]>();
        Funcs.Add(KeyState.Pressed, new MouseFunc[3]);
        Funcs.Add(KeyState.Held, new MouseFunc[3]);
        Funcs.Add(KeyState.Released, new MouseFunc[3]);

        if (checkPresetValidity(preset) == false)
            return;

        foreach (KeyState state in preset.Keys)
        {
            Funcs[state] = preset[state];
        }
    }

    bool checkPresetValidity(Dictionary<KeyState, MouseFunc[]> preset)
    {
        if (preset.Count != 3)
        {
            Debug.LogError("WRITE ME!");
            return false;
        }

        foreach (KeyState state in preset.Keys)
        {
            if (preset[state].Length != 3)
            {
                Debug.LogError("WRITE ME!");
                return false;
            }
        }

        return true;
    }
    public Dictionary<KeyState, MouseFunc[]> GetFuncs()
    {
        return Funcs;
    }
    public void SetFuncs(Dictionary<KeyState, MouseFunc[]> newFuncs)
    {
        Funcs = newFuncs;
    }
}

public enum KeyState
{
    Pressed,
    Held,
    Released
}