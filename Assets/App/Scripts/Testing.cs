using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(10, 10, 10f);
        Debug.Log(grid);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
