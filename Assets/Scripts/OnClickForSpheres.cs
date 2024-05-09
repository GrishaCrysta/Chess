using System;
using System.Collections.Generic;
using UnityEngine;

public class ForMovePads : MonoBehaviour
{
    public bool isMove;
    public void delete(object sender, EventArgs args)
    {
        Destroy(gameObject);
    }
    /*
    public void Start()
    {
        Transform button = Figure.chessButtons.transform.GetChild(ChessVector.GetCoordinatesFromVector(transform.position).y);
        button = button.GetChild(ChessVector.GetCoordinatesFromVector(transform.position).x);
        button.gameObject.GetComponent<ForButtons>().selected = true;
    }
    public void OnMouseDown()
    {
        Figure.GetFigure(Figure.selected).move(ChessVector.GetCoordinatesFromVector(gameObject.transform.position));
    }
    */
    private void OnMouseDown()
    {
        if (isMove)
            Figure.GetFigure(Figure.selected).move(ChessVector.GetCoordinatesFromVector(gameObject.transform.position));
    }
}
