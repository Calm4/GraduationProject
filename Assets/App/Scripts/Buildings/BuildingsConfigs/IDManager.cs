using UnityEngine;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    public static class IDManager
    {
        private const string IDKey = "CurrentBuildingID";

        public static int GetNextID()
        {
            // Получаем текущий ID из PlayerPrefs
            int currentID = PlayerPrefs.GetInt(IDKey, 0);
        
            // Увеличиваем ID на 1
            int nextID = currentID + 1;
        
            // Сохраняем новый ID обратно в PlayerPrefs
            PlayerPrefs.SetInt(IDKey, nextID);
            PlayerPrefs.Save(); // Сохраняем изменения немедленно
            return nextID;
        }
    }
}