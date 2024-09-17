using System;

namespace App.Scripts.Placement.JsonClasses
{
    [Serializable]
    public class Vector2IntJson
    {
        public int x;
        public int y;
        
        public Vector2IntJson(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2IntJson() {}
    }
}