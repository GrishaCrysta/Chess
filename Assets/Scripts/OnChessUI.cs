using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OnChessUI : MonoBehaviour
{
    public EventHandler mouseDown;
    public EventHandler mouseEnter;
    public void OnMouseDown()
    {
        if(mouseDown != null)
            mouseDown(1, EventArgs.Empty);
    }
    public void OnMouseEnter()
    {
        if(mouseEnter != null)
            mouseEnter(1, EventArgs.Empty);
    }
    
}
static class ActiveUI
{
    public static bool builder = false;
}