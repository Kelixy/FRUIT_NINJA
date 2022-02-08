using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "Flier_Settings", menuName = "Flier_Settings", order = 51)]
    public class FlierSettings : ScriptableObject
    {
        [SerializeField] private Sprite leftHalfSprites; 
        [SerializeField] private Sprite rightHalfSprites;
        [SerializeField] private Color splashColor;

        public Sprite LeftHalfSprite => leftHalfSprites;
        public Sprite RightHalfSprite => rightHalfSprites;
        public Color SplashColor => splashColor;
    }
}
