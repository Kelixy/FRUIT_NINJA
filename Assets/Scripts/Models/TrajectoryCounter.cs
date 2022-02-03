using System;
using UnityEngine;

namespace Models
{
    public static class TrajectoryCounter
    {
        private const float G = 9.81f;
        
        public static Vector3 GetTrajectoryPointInMoment(Vector3 lastPosition, float speed, float time, float horizonAngle)
        {
            horizonAngle *= Mathf.Deg2Rad;
            var angleTan = (float)Math.Tan(horizonAngle);
            var angleCos = (float)Math.Cos(horizonAngle);
            var angleCosPow = (float) Math.Pow(angleCos, 2);
            var speedPow = (float) Math.Pow(speed, 2);
            
            var xPos = time * angleCos;
            var gravityPower = xPos * G / (2 * speedPow * angleCosPow);
            var yPos = xPos * (angleTan - gravityPower);
            var nextPoint = lastPosition + new Vector3(xPos, yPos);
        
            return nextPoint;
        }
    }
}
