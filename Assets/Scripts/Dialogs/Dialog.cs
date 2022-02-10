using DG.Tweening;
using Models;
using UnityEngine;

namespace Dialogs
{
    public class Dialog : ComponentSingleton<Dialog>
    {
        [SerializeField] private RectTransform dialogRectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform hiddenPosPoint;
        
        protected void Show()
        {
            if (canvasGroup.gameObject.activeSelf) return;
            
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
            DOTween.Sequence()
                .Append(canvasGroup.DOFade(1, 1))
                .Join(dialogRectTransform.DOAnchorPos(Vector2.zero, 1));
        }

        protected void Hide()
        {
            DOTween.Sequence()
                .Append(dialogRectTransform.DOAnchorPos(hiddenPosPoint.localPosition, 1))
                .Join(canvasGroup.DOFade(0, 1));
        }
    }
}
