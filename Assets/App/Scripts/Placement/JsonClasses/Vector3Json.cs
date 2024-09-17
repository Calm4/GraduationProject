using System;
using UnityEngine;

[Serializable]
public class Vector3Json
{
    public int x;
    public int y;
    public int z;

    public Vector3Json(Vector3 vec)
    {
        x = Mathf.RoundToInt(vec.x);
        y = Mathf.RoundToInt(vec.y);
        z = Mathf.RoundToInt(vec.z);
    }
}