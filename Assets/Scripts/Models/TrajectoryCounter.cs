using System;
using System.Collections.Generic;
using UnityEngine;

public static class TrajectoryCounter
{
    private const float G = 9.81f;
    public static Vector3[] GetPoints(Vector3 startPosition, float speed, float timeSectionLength, float horizonAngle, float gravity)
    {
        var pointsList = new List<Vector3>();
        var timeSection = 0f;
        var yPos = 0f;
        horizonAngle *= Mathf.Deg2Rad;
        var angleTan = (float)Math.Tan(horizonAngle);
        var angleCos = (float)Math.Cos(horizonAngle);
        var angleCosPow = (float) Math.Pow(angleCos, 2);
        var speedPow = (float) Math.Pow(speed, 2);
        
        while (yPos + startPosition.y >= startPosition.y)
        {
            var xPos = timeSection * angleCos;
            var gravityPower = xPos * G * gravity / (2 * speedPow * angleCosPow);
            yPos = xPos * (angleTan - gravityPower);

            pointsList.Add(startPosition + new Vector3(xPos, yPos));
            timeSection += timeSectionLength;
        }
        
        return pointsList.ToArray();
    }
}
