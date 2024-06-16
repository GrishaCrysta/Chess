using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OnChessUI : MonoBehaviour
{
   
    public EventHandler mouseDown;
    public EventHandler mouseEnter;
    private void Awake()
    {
        /*Logger.debug.startFunc("OnChessUI.Awake", $"o = {name}")*/;
        if (gameObject.GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }
        /*Logger.debug.endFunc("OnChessUI.Awake")*/;
    }
    public void OnMouseDown()
    {
        if(mouseDown != null)
        {
            /*Logger.debug.startFunc("OnChessUI.OnMouseDown", $"o = {name}")*/;
            mouseDown(1, EventArgs.Empty);
            /*Logger.debug.endFunc("OnChessUI.OnMouseDown")*/;
        }
    }
    public void OnMouseEnter()
    {
        if (mouseEnter != null)
        {
            /*Logger.debug.startFunc("OnChessUI.OnMouseEnter", $"o = {name}")*/;
            mouseEnter(1, EventArgs.Empty);
            /*Logger.debug.endFunc("OnChessUI.OnMouseEnter")*/;
        }
    }
    
}
static class ActiveUI
{
    public static bool builder = false;
}