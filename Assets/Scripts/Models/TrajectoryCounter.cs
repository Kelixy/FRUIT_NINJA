using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class TrajectoryCounter : ClassSingleton<TrajectoryCounter>
    {
        private const float G = 9.81f;
        public Vector3[] GetPoints(Vector3 startPosition, float speed, float timeSectionLength, float horizonAngle, float gravity, Vector2 sceneSize, float fruitRadius)
        {
            var pointsList = new List<Vector3>();
            var timeSection = 1f;
            var yPos = 0f;
            horizonAngle *= Mathf.Deg2Rad;
            var angleTan = (float)Math.Tan(horizonAngle);
            var angleCos = (float)Math.Cos(horizonAngle);
            var angleCosPow = (float) Math.Pow(angleCos, 2);
            var speedPow = (float) Math.Pow(speed, 2);
            
            var tempY = startPosition.y;
            while (yPos + startPosition.y >= - sceneSize.y / 2 - fruitRadius || yPos + startPosition.y >= startPosition.y)
            {
                var xPos = timeSection * angleCos;
                var gravityPower = xPos * G * gravity / (2 * speedPow * angleCosPow);
                yPos = xPos * (angleTan - gravityPower);
                timeSection += timeSectionLength;
                
                if (yPos + startPosition.y + fruitRadius > sceneSize.y / 2.5f && tempY > yPos)
                {
                    var delta = (yPos + startPosition.y) - sceneSize.y / 2.5f;
                    startPosition -= new Vector3(0, delta + fruitRadius);
                    break;
                }

                tempY = yPos;
            }

            timeSection = 1f;
            Vector3 point = default;
        
            while (point.y >= startPosition.y || point.y > - sceneSize.y / 2 - fruitRadius)
            {
                var xPos = timeSection * angleCos;
                var gravityPower = xPos * G * gravity / (2 * speedPow * angleCosPow);
                yPos = xPos * (angleTan - gravityPower);
                point = startPosition + new Vector3(xPos, yPos);
                if (CheckIfPointOnScene(point, sceneSize, fruitRadius))
                    pointsList.Add(point);
                timeSection += timeSectionLength;
            }
        
            return pointsList.ToArray();
        }

        private bool CheckIfPointOnScene(Vector3 point, Vector2 sceneSize, float indent = 0)
        {
            return point.x > - sceneSize.x / 2 - indent
                   && point.x < sceneSize.x / 2 + indent
                   && point.y > - sceneSize.y / 2 - indent
                   && point.y < sceneSize.y / 2 + indent;
        }
    }
}
