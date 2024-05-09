using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OnChessUI : MonoBehaviour
{
    public EventHandler func;
    public void OnMouseDown()
    {
        func(1, EventArgs.Empty);
    }
}
