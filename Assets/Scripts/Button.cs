using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butto : MonoBehaviour
{
    public string url = "https://www.patreon.com/didkozhaty";
    public void OnMouseDown()
    {
        Debug.Log("Patreon");
        Application.OpenURL(url);
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
    public void DeleteChild()
    {
        List<GameObject> childList = new List<GameObject>();
        for(int i = 0; transform.childCount > i; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }
        foreach(GameObject child in childList)
        {
            Destroy(child);
        }
    }
}
