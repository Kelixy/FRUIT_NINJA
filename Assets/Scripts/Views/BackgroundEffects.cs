using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class BackgroundEffects : MonoBehaviour
    {
        private const float MaxCloudSpeed = 0.2f;
        private const float MaxCloudSpeedIncreaseStep = 0.01f;
        
        [SerializeField] private RawImage background;
        [SerializeField] private Camera camera;
        [Range(0, MaxCloudSpeed)][SerializeField] private float cloudsSpeed;

        private SceneController SceneController => ControllersManager.Instance.SceneController;
        private float _uvRectW;
        private float _uvPosX;

        public void Initialize()
        {
            _uvRectW = (SceneController.SceneSize.x / SceneController.SceneSize.y) / (SceneController.ReferenceResolution.x / SceneController.ReferenceResolution.y);
        }

        public void IncreaseCloudSpeed()
        {
            if (cloudsSpeed + MaxCloudSpeedIncreaseStep < MaxCloudSpeed)
                cloudsSpeed += MaxCloudSpeedIncreaseStep;
        }

        public void ShakeCamera()
        {
            camera.DOShakePosition(1);
        }

        private void Update()
        {
            background.uvRect = new Rect(_uvPosX += cloudsSpeed * Time.deltaTime,0,_uvRectW, 1);
        }
    }
}
