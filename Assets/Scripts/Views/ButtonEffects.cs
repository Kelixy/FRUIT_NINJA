using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ButtonEffects : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image buttonImg;
        [SerializeField] private Vector3 squeezedScale;
        [SerializeField] private Color squeezedColor;
        [Range(0.1f,1f)][SerializeField] private float squeezedDuration;
    
        private Sequence _sequence;
    
        public void SqueezeButton()
        {
            StopSequence();

            _sequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(squeezedScale, squeezedDuration))
                .Join(buttonImg.DOColor(squeezedColor,squeezedDuration));
        }
    
        public void RelaxButton()
        {
            StopSequence();
        
            _sequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(1, squeezedDuration))
                .Join(buttonImg.DOColor(Color.white,squeezedDuration));
        }

        private void StopSequence()
        {
            if (_sequence != null && _sequence.IsPlaying())
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
    }
}
