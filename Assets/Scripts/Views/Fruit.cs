using UnityEngine;

namespace Views
{
    public class Fruit : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
        public float Radius => RectTransform.rect.height / 2;
        
        public void Switch(bool shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);
        }
    }
}
