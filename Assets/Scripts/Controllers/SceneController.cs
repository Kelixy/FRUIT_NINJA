using Dialogs;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private HeartsPanel heartsPanel;
        [SerializeField] private Score score;
        [SerializeField] private RectTransform gameCanvasRectTransform;
        [SerializeField] private FinalLevelDialog finalLevelDialog;
        [SerializeField] private StartDialog startDialog;

        public StartDialog StartDialog => startDialog;
        public FinalLevelDialog FinalLevelDialog => finalLevelDialog;
        public HeartsPanel HeartsPanel => heartsPanel;
        public Score Score => score;
        public Vector2 SceneSize => gameCanvasRectTransform.rect.size;

        public void Initialize()
        {
            heartsPanel.Initialize();
            score.ReInit();
        }

        
    }
}
