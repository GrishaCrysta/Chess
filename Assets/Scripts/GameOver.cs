using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject text;
    public void OnMouseDown()
    {
        Destroy(text);
        Destroy(gameObject);
    }
}
