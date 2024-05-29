using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;


[Serializable]
public struct ResourceData
{
    public ResourceConfig resourceConfig;
    public int currentAmount;
    public int maxAmount;
}
