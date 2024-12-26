using System.Collections;
using System.Collections.Generic;
using App.Scripts;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
   private void Update(){
      if (Input.GetMouseButtonDown(0))
      {
         DetectClick();
      }
   }

   private void DetectClick()
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
         GameObject clickedObject = hit.collider.gameObject;

         // Check if the clicked object has the EnemyClick component
         EnemyClick enemyClick = clickedObject.GetComponent<EnemyClick>();
         if (enemyClick != null)
         {
            enemyClick.OnClick();
         }
      }
   }
}
