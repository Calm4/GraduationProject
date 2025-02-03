using UnityEngine;

namespace App.Scripts.Modifiers
{
    public class AttackSpeedModifier : Modifier
    {
        public AttackSpeedModifier()
        {
            
        }
        
        public override void ModifierUpdate()
        {
            Debug.Log("AttackSpeedModifier Update");
        }
    }
}