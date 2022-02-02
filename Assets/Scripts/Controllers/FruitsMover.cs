using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FruitsMover : MonoBehaviour
{
    [Range(0, 200)] [SerializeField] private float fruitStartSpeed = 100;
    [Range(0, 2)] [SerializeField] private float gravity = 1;
    [Range(0, 10)] [SerializeField] private float jumpDuration = 1;

    [SerializeField] private RectTransform screenRectTransform;

    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject pointPrefab;

    private Vector2 _screenSize;
    private Vector3 _bottomLeftPoint;
    private Vector3 _bottomRightPoint;
    private Vector3 _topLeftPoint;
    private Vector3 _topRightPoint;
    private float _yCeiling;

    private void Start()
    {
        _screenSize = screenRectTransform.rect.size;
        _yCeiling = _screenSize.y / 2;
        _bottomLeftPoint = new Vector3(-_screenSize.x / 2, -_yCeiling);
        _bottomRightPoint = new Vector3(_screenSize.x / 2, -_yCeiling);
        _topLeftPoint = new Vector3(-_screenSize.x / 2, _yCeiling);
        _topRightPoint = new Vector3(-_screenSize.x / 2, _yCeiling);
        
        var fruit = Instantiate(fruitPrefab, transform);
        var fruitRt = fruit.GetComponent<RectTransform>();
        fruit.transform.localPosition = _bottomLeftPoint;
        var positions = TrajectoryCounter.GetPoints(fruit.transform.localPosition, fruitStartSpeed, 1, 80, gravity);

        var maxY = GetTheTopOfTrajectory(positions);
        if (maxY >= _yCeiling - fruitRt.rect.height)
        {
            var delta = (maxY + _yCeiling / 2) / (_yCeiling * 2);
            positions = TrajectoryCounter.GetPoints(fruit.transform.localPosition, fruitStartSpeed, 1, 60, gravity * delta);
        }
        
        DOTween.Sequence()
            .Append(fruitRt.DOLocalPath(positions, jumpDuration).SetEase(Ease.InOutQuad));

        ShowTestTrajectory(positions);
    }

    private Vector3 GetRandomStartPoint(float fruitHeight)
    {
        

        var randomX = Random.Range(_bottomLeftPoint.x, _bottomRightPoint.x);
        var randomY = -_screenSize.y / 2;
        
        return new Vector3(0, -_screenSize.y / 2 - fruitHeight);
    }

    private float GetTheTopOfTrajectory(Vector3[] trajectory) => trajectory[trajectory.Length / 2].y;

    private void ShowTestTrajectory(Vector3[] positions)
    {
        int ind = 0;
        foreach (var pos in positions)
        {
            if (ind % 10 == 0)
            {
                var point = Instantiate(pointPrefab, transform);
                point.transform.localPosition = pos;
            }

            ind++;
        }
    }
}
