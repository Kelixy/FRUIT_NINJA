using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Flier_Settings", menuName = "Flier_Settings", order = 51)]
    public class FlierSettings : ScriptableObject
    {
        [SerializeField] private Sprite leftHalfSprites; 
        [SerializeField] private Sprite rightHalfSprites;
        [SerializeField] private Color splashColor;
        [SerializeField] private Material explosionMaterial;
        [SerializeField] private int kindOfMechanic;
        [Range(0.1f,10)][SerializeField] private float speedMultiplier = 1;

        public Sprite LeftHalfSprite => leftHalfSprites;
        public Sprite RightHalfSprite => rightHalfSprites;
        public Color SplashColor => splashColor;
        public int KindOfMechanic => kindOfMechanic;
        public float SpeedMultiplier => speedMultiplier;
        public Material ExplosionMaterial => explosionMaterial;
    }
}
