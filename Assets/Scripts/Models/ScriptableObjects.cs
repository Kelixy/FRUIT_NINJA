using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    [CreateAssetMenu(fileName = "Flier Settings", menuName = "Flier Settings", order = 51)]
    public class FlierSettings : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Color splashColor;

        public Sprite Sprite => sprite;
        public Color SplashColor => splashColor;
    }
}
