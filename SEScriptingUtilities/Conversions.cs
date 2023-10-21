using System;
using System.Collections.Generic;
using System.Text;

namespace SEScriptingUtilities
{
    public class Conversions
    {
        public static float RadToDeg(float radians)
        {
            return radians*180/(float)Math.PI;
        }
        public static float DegToRad(float degrees)
        {
            return degrees*(float)Math.PI/180;
        }
    }
}
