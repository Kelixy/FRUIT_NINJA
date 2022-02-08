using UnityEngine;
using Views;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private Score score;
        [SerializeField] private RectTransform gameCanvasRectTransform;

        public HealthPoints HealthPoints => healthPoints;
        public Score Score => score;
        public Vector2 SceneSize => gameCanvasRectTransform.rect.size;

        public void Initialize()
        {
            healthPoints.ReInit();
            score.ReInit();
        }
    }
}
