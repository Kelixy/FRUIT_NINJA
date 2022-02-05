using UnityEngine;

namespace Models
{
    public class ComponentSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject
                        {
                            hideFlags = HideFlags.HideAndDontSave
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

    }
}