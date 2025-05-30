using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts.Input
{
   public class ClickDetector : MonoBehaviour
   {
      private void Update(){
         if (UnityEngine.Input.GetMouseButtonDown(0))
         {
            DetectClick();
         }
      }

      private void DetectClick()
      {
         Ray ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
         RaycastHit hit;

         if (Physics.Raycast(ray, out hit))
         {
            GameObject clickedObject = hit.collider.gameObject;

            EnemyClick enemyClick = clickedObject.GetComponent<EnemyClick>();
            if (enemyClick != null)
            {
               enemyClick.OnClick();
            }
         }
      }
   }
}
