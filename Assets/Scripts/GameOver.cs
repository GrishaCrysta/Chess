using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject text;
    public void OnMouseDown()
    {
        /*Logger.debug.startFunc("GameOver.OnMouseDown", $"o = {name}")*/;
        Logger.ui.log($"OnChessUI.OnMouseDown(o = {name})");
        Destroy(text);
        Destroy(gameObject);
        /*Logger.debug.endFunc("GameOver.OnMouseDown")*/;
    }
}
