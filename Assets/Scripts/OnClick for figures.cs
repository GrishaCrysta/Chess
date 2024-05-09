using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForFigures : MonoBehaviour
{
    /*
    public bool selected = false;
    private void OnMouseDown()
    {
        ChessVector vector = ChessVector.GetCoordinatesFromVector(transform.position);
        if(Figure.GetFigure(vector) != null && !Figure.selectedCanBeat.Contains(vector))
            Figure.select(vector);
        else
        {
            if (selected)
            {
                Figure.GetFigure(Figure.selected).move(vector);
                selected = false;
            }
            Figure.OnSelect(vector, EventArgs.Empty);
            Figure.OnSelect = (sender, e) => { };
            Figure.selectedCanBeat.Clear();
        }
    }
    */
    private void OnMouseDown()
    {
        ChessVector vector = ChessVector.GetCoordinatesFromVector(transform.position);
        Figure.select(vector);
 //       Debug.Log($"Moved {name}");
    }
}