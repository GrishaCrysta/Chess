using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butto : MonoBehaviour
{
    public string url = "https://www.patreon.com/didkozhaty";
    public void OnMouseDown()
    {
        /*Logger.debug.startFunc("Butto.OnMouseDown", $"o = {gameObject.name}, url = {url}")*/;
        Logger.ui.log($"Butto.OnMouseDown({gameObject.name})");
        Debug.Log("Patreon");
        Application.OpenURL(url);
        /*Logger.debug.endFunc("Butto.OnMouseDown")*/;
    }
    public void Delete()
    {
        /*Logger.debug.startFunc("Butto.Delete", $"o = {gameObject.name}")*/;
        Destroy(gameObject);
        /*Logger.debug.endFunc("Butto.Delete")*/;
    }
    public void DeleteChild()
    {
        /*Logger.debug.startFunc("Butto.DeleteChild", $"o = {gameObject.name}")*/;
        List<GameObject> childList = new List<GameObject>();
        for(int i = 0; transform.childCount > i; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }
        foreach(GameObject child in childList)
        {
            Destroy(child);
        }
        /*Logger.debug.endFunc("Butto.DeleteChild")*/;
    }
}
