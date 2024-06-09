using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForFigures : MonoBehaviour
{
    public static List<GameObject> instances = new();
    private void Awake()
    {
        instances.Add(gameObject);
    }
    private void OnDestroy()
    {
        instances.Remove(gameObject);
    }
    public static void ChangeSize(float x)
    {
        foreach (GameObject instance in instances)
        {
            if (instance != null)
            {
                instance.transform.localScale *= x;
                instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y * x, instance.transform.position.z);
            }
        }
        foreach (GameObject model in Figure.models)
        {
            model.transform.localScale *= x;
        }
    }
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
    /*private void OnMouseDown()
    {
        ChessVector vector = ChessVector.GetCoordinatesFromVector(transform.position);
        Figure.select(vector);
 //       Debug.Log($"Moved {name}");
    }*/

}