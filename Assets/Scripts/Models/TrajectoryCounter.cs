using System;
using UnityEngine;

namespace Models
{
    public static class TrajectoryCounter
    {
        private const float G = 9.81f;
        
        public static Vector3 GetTrajectoryPointInMoment(float startSpeed, float time, float horizonAngle)
        {
            horizonAngle *= Mathf.Deg2Rad;
            var angleCos = (float)Math.Cos(horizonAngle);
            var angleSin = (float)Math.Sin(horizonAngle);
            
            var xPos = time * startSpeed * angleCos;
            var yPos = time * (startSpeed * angleSin - time * G * 0.5f);
            var nextPoint = new Vector3(xPos, yPos);
            
            return nextPoint;
        }
    }
}
