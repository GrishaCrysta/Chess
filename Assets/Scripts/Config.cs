using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    private static float SIZE = 0.7f;
    [SerializeField]
    public float Size = SIZE;
    public static float size
    {
        get { return SIZE; }
        set 
        {
            if(value == 0)
                return;
            ForFigures.ChangeSize(value / SIZE);
            SIZE = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnValidate()
    {
        size = Size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
