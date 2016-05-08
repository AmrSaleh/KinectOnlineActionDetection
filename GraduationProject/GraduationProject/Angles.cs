using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace GraduationProject
{
    public static class Angles
    {
        //number of angles
        public static readonly int count = 35;

        //zero point
        public static readonly int[] zeroPoint = { 0, 0, 0 };

        //zero point id
        public static readonly int zeroPointId = -1;

        // the 35 angles needed for the anguler discriptor
        public static readonly int[,] anglesIds = new int[,] {
                // Right Arm (1-3)
                { (int) JointType.ShoulderCenter, (int) JointType.ShoulderRight, (int) JointType.ElbowRight},
                { (int) JointType.ShoulderRight, (int) JointType.ElbowRight, (int )JointType.WristRight},
                { (int) JointType.ElbowRight, (int) JointType.WristRight, (int) JointType.HandRight },
                
                //Left Arm (4-6)
                { (int) JointType.ShoulderCenter, (int) JointType.ShoulderLeft, (int) JointType.ElbowLeft},
                { (int) JointType.ShoulderLeft, (int) JointType.ElbowLeft, (int) JointType.WristLeft},
                { (int) JointType.ElbowLeft,(int) JointType.WristLeft, (int) JointType.HandLeft},

                //Right Leg (7-9)
                { (int) JointType.HipCenter, (int) JointType.HipRight, (int) JointType.KneeRight},
                { (int) JointType.HipRight, (int) JointType.KneeRight, (int) JointType.AnkleRight},
                { (int) JointType.KneeRight, (int) JointType.AnkleRight, (int) JointType.FootRight},

                //Left Leg (9-11)
                { (int) JointType.HipCenter, (int) JointType.HipLeft, (int) JointType.KneeLeft},
                { (int) JointType.HipLeft, (int) JointType.KneeLeft, (int) JointType.AnkleLeft},
                { (int) JointType.KneeLeft, (int) JointType.AnkleLeft, (int) JointType.FootLeft},

                //Symmetric (12-15)
                { (int) JointType.ShoulderLeft, (int) JointType.ShoulderCenter, (int) JointType.ShoulderRight},
                { (int) JointType.Head, (int) JointType.ShoulderCenter, (int) JointType.Spine},
                { (int) JointType.ShoulderCenter, (int) JointType.Spine, (int) JointType.HipCenter},

                //Big Symmetric (16-19)
                { (int) JointType.ElbowLeft, (int) JointType.ShoulderCenter, (int) JointType.ElbowRight},
                { (int) JointType.WristLeft, (int) JointType.ShoulderCenter, (int) JointType.WristRight},
                { (int) JointType.KneeLeft, (int) JointType.HipCenter, (int) JointType.KneeRight},
                { (int) JointType.AnkleLeft, (int) JointType.HipCenter, (int) JointType.AnkleRight},

                //Big Hand-Foot (20-23)
                { (int) JointType.WristLeft, (int) JointType.HipCenter, (int) JointType.AnkleLeft},
                { (int) JointType.WristRight, (int) JointType.HipCenter, (int) JointType.AnkleRight},
                { (int) JointType.WristLeft, (int) JointType.HipCenter, (int) JointType.AnkleRight},
                { (int) JointType.WristRight, (int) JointType.HipCenter, (int) JointType.AnkleLeft},

                //Absolute camera-facing angles (24-35)
                { (int) JointType.WristLeft, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.ElbowLeft, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.ShoulderLeft, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.AnkleLeft, (int) JointType.HipCenter, zeroPointId},
                { (int) JointType.KneeLeft, (int) JointType.HipCenter, zeroPointId},
                { (int) JointType.HipLeft, (int) JointType.HipCenter, zeroPointId},
                { (int) JointType.WristRight, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.ElbowRight, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.ShoulderRight, (int) JointType.ShoulderCenter, zeroPointId},
                { (int) JointType.AnkleRight, (int) JointType.HipCenter, zeroPointId},
                { (int) JointType.KneeRight, (int) JointType.HipCenter, zeroPointId},
                { (int) JointType.HipRight, (int) JointType.HipCenter, zeroPointId}
            };

    }
}
