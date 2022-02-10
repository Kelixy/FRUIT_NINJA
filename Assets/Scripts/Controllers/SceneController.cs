using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private Score score;
        [SerializeField] private RectTransform gameCanvasRectTransform;
        [SerializeField] private CanvasScaler gameCanvasScaler;
        [SerializeField] private BackgroundEffects backgroundEffects;

        public HealthPoints HealthPoints => healthPoints;
        public BackgroundEffects BackgroundEffects => backgroundEffects;
        public Score Score => score;
        public Vector2 SceneSize => gameCanvasRectTransform.rect.size;
        public Vector2 ReferenceResolution => gameCanvasScaler.referenceResolution;

        public void Initialize()
        {
            backgroundEffects.Initialize();
            healthPoints.ReInit();
            score.ReInit();
        }

        
    }
}
