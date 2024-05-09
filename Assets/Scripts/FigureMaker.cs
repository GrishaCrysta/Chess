using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FigureMaker : MonoBehaviour
{
    private static List<GameObject> intances = new List<GameObject>();
    private static color COLOR;
    public static void SetColor(string value)
    {
        if (value == "white")
            COLOR = color.white;
        else if (value == "black")
            COLOR = color.black;
        intances.ForEach(i => i.GetComponent<Renderer>().material = Figure.materials[(int)COLOR]);
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
        intances.ForEach(i => i.SetActive(!i.activeSelf));
    }
    public void Awake()
    {
        intances.Add(gameObject);
    }
    public void OnDestroy()
    {
        intances.Remove(gameObject);
        
    }
    public void OnMouseDown()
    {
        GameObject figureMaker = Instantiate(gameObject);
        figureMaker.transform.SetParent(gameObject.transform.parent);
        figureMaker.transform.localPosition = gameObject.transform.localPosition;
    }
    public void OnMouseDrag()
    {
        float distanceToBoard = (Camera.main.transform.position.y - Camera.main.nearClipPlane) - (GameObject.Find("Chessboard").transform.position.y + (Figure.size / 2.0f));
        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToBoard));
        gameObject.transform.position = worldPoint;
    }
    public void OnMouseUp()
    {
        ChessVector coords = ChessVector.GetCoordinatesFromVector(transform.position);
        if (coords.isValid())
        {
            switch (TYPE)
            {
                case 0:
                    Figure.SetFigure(new Pawn(coords, COLOR));
                    break;
                case 1:
                    Figure.SetFigure(new Elephant(coords, COLOR));
                    break;
                case 2:
                    Figure.SetFigure(new Horse(coords, COLOR));
                    break;
                case 3:
                    Figure.SetFigure(new Officer(coords, COLOR));
                    break;
                case 4:
                    Figure.SetFigure(new Queen(coords, COLOR));
                    break;
                case 5:
                    Figure.SetFigure(new King(coords, COLOR));
                    break;
                case 6:
                    Figure.Destroy(coords);
                    break;
            }
        }
        Destroy(gameObject);
    }

    public static void ChangeColor()
    {
        if (COLOR == color.white)
            SetColor("black");
        else if (COLOR == color.black)
            SetColor("white");
    }
}