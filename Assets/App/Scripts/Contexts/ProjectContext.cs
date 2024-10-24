using UnityEngine;

namespace App.Scripts
{
    public sealed class ProjectContext : MonoBehaviour
    {
        private static ProjectContext _instance;

        public static ProjectContext Instance
        {
            get
            {
                Debug.Log("ETO PIPEC!");
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<ProjectContext>();
                if (_instance != null)
                {
                    return _instance;
                }


                _instance = Resources.Load<ProjectContext>("ProjectContext");
                if (_instance != null)
                {
                    _instance = Instantiate(_instance);
                    _instance.name = "ProjectContext";
                    _instance.Initialize();
                    
                    DontDestroyOnLoad(_instance);
                }
                else
                {
                    Debug.LogError("Prefab not found!");
                }

                return _instance;
            }
        }

        public void Test()
        {
            Debug.Log("LOOK AT ME");
        }

        private void Initialize()
        {
            Debug.Log("123!!!");
            gamePhaseManager = FindObjectOfType<GamePhaseManager>();
        }
        
         public GamePhaseManager gamePhaseManager;
    }
}