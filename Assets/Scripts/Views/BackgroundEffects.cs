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
        [Range(0, MaxCloudSpeed)][SerializeField] private float startCloudsSpeed;
        
        private float _cloudsSpeed;

        private SceneController SceneController => ControllersManager.Instance.SceneController;
        private float _uvRectW;
        private float _uvPosX;

        public void Initialize()
        {
            _uvRectW = (SceneController.SceneSize.x / SceneController.SceneSize.y) / (SceneController.ReferenceResolution.x / SceneController.ReferenceResolution.y);
            Reinit();
        }

        public void Reinit()
        {
            _cloudsSpeed = startCloudsSpeed;
        }

        public void IncreaseCloudSpeed()
        {
            if (_cloudsSpeed + MaxCloudSpeedIncreaseStep < MaxCloudSpeed)
                _cloudsSpeed += MaxCloudSpeedIncreaseStep;
        }

        public void ShakeCamera()
        {
            camera.DOShakePosition(1);
        }

        private void Update()
        {
            background.uvRect = new Rect(_uvPosX += _cloudsSpeed * Time.deltaTime,0,_uvRectW, 1);
        }
    }
}
