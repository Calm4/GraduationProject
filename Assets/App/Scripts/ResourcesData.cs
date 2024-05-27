using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;


[Serializable]
public struct ResourcesData
{
    public ResourcesTypes resourceType;
    public int currentAmount;
    public int maxAmount;
}
