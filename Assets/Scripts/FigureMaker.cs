using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FigureMaker : MonoBehaviour
{
    public static List<GameObject> intances = new List<GameObject>();
    public static bool isDragging;
    private static color COLOR;
    public static int intColor
    {
        get { return (int)COLOR; }
        set { COLOR = (color) value; }
    }
    public static int selected;
    public static void SetColor(string value)
    {
        /*Logger.debug.startFunc("FigureMaker.SetColor", $"v = {value}")*/;
        if (value == "white")
            COLOR = color.white;
        else if (value == "black")
            COLOR = color.black;
        intances.ForEach(i => i.GetComponent<Renderer>().material = Figure.materials[(int)COLOR]);
        /*Logger.debug.endFunc("FigureMaker.SetColor")*/;
    }
    public int TYPE;
    public string type
    {
        get 
        { 
            if(TYPE == 0)
                return "Pawn";
            if (TYPE == 1)
                return "Elephant";
            if (TYPE == 2)
                return "Horse";
            if (TYPE == 3)
                return "Officer";
            if (TYPE == 4)
                return "Queen";
            if (TYPE == 5)
                return "King";
            if (TYPE == 6)
                return "Del";
            else
                return TYPE.ToString();
        }
        set
        {
            if (value == "Pawn")
                TYPE = 0;
            if (value == "Elephant")
                TYPE = 1;
            if (value == "Horse")
                TYPE = 2;
            if (value == "Officer")
                TYPE = 3;
            if (value == "Queen")
                TYPE = 4;
            if (value == "King")
                TYPE = 5;
            if (value == "Del")
                TYPE = 6;
        }
    }
    public static void Active()
    {
        /*Logger.debug.startFunc("FigureMaker.Active")*/;
        intances.ForEach(i => i.SetActive(!i.activeSelf));
        /*Logger.debug.endFunc("FigureMaker.Active")*/;
    }
    public void Awake()
    {
        /*Logger.debug.startFunc("FigureMaker.Awake")*/;
        intances.Add(gameObject);
        /*Logger.debug.endFunc("FigureMaker.Awake")*/;
    }
    public void OnDestroy()
    {
        /*Logger.debug.startFunc("FigureMaker.OnDestroy")*/;
        intances.Remove(gameObject);
        /*Logger.debug.endFunc("FigureMaker.OnDestroy")*/;
    }
    public void OnMouseDown()
    {
        /*Logger.debug.startFunc("FigureMaker.OnMouseDown", $"o = {name}")*/;
        Logger.ui.log($"FigureMaker.OnMouseDown(o = {name})");
        isDragging = true;
        GameObject figureMaker = Instantiate(gameObject);
        figureMaker.transform.SetParent(gameObject.transform.parent);
        figureMaker.transform.localPosition = gameObject.transform.localPosition;
        selected = TYPE;
        /*Logger.debug.endFunc("FigureMaker.OnMouseDown")*/;
    }
    public void OnMouseDrag()
    {
        /*Logger.debug.startFunc("FigureMaker.OnMouseDrag", $"o = {name}, p = {transform.position}")*/;
        float distanceToBoard = (Camera.main.transform.position.y - Camera.main.nearClipPlane) - (GameObject.Find("Chessboard").transform.position.y + (transform.lossyScale.y / 2.0f));
        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToBoard));
        gameObject.transform.position = worldPoint;
        /*Logger.debug.endFunc("FigureMaker.OnMouseDrag")*/;
    }
    public void OnMouseUp()
    {
        /*Logger.debug.startFunc("FigureMaker.OnMouseUp", $"o = {name}, p = {transform.position}")*/;
        Logger.ui.log($"FigureMaker.OnMouseUp(o = {name}, p = {transform.position})");
        isDragging = false;
        ChessVector coords = ChessVector.GetCoordinatesFromVector(transform.position);
        if (coords.isValid())
        {
            Figure.SetFigure(TYPE, coords, COLOR);
        }
        Destroy(gameObject);
        /*Logger.debug.endFunc("FigureMaker.OnMouseUp")*/;
    }

    public static void ChangeColor()
    {
        /*Logger.debug.startFunc("FigureMaker.ChangeColor", $"c = {COLOR}")*/;
        if (COLOR == color.white)
            SetColor("black");
        else if (COLOR == color.black)
            SetColor("white");
        /*Logger.debug.endFunc("FigureMaker.ChangeColor")*/;
    }
}