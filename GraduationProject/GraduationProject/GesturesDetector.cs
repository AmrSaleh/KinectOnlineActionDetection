using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;


namespace GraduationProject
{
    public class GesturesDetector
    {

        private static GesturesDetector gesturesDetector = null;

        private readonly float FRONT_GESTURE_THRESHOLD = GlobalConstant.FRONT_GESTURE_THRESHOLD;
        private readonly float BACK_GESTURE_THRESHOLD = GlobalConstant.BACK_GESTURE_THRESHOLD;
        private readonly float RIGHT_GESTURE_THRESHOLD = GlobalConstant.RIGHT_GESTURE_THRESHOLD;
        private readonly float LEFT_GESTURE_THRESHOLD = GlobalConstant.LEFT_GESTURE_THRESHOLD;

        //Arm Defense Thresholds
        private readonly float HANDLEFTY_SPINEY_MAXDIFFERENCE = GlobalConstant.HANDLEFTY_SPINEY_MADDIFFERENCE;
        private readonly float SHOULDERLEFTY_ELBOWLEFTY_MAXDIFFERENCE = GlobalConstant.SHOULDERLEFTY_ELBOWLEFTY_MAXDIFFERENCE;

        public readonly int FRONT_GESTURE_ID = 30;
        public readonly int BACK_GESTURE_ID = 40;
        public readonly int RIGHT_GESTURE_ID = 31;
        public readonly int LEFT_GESTURE_ID = 32;
        public readonly int FRONT_RIGHT_GESTURE_ID = 61;
        public readonly int FRONT_LEFT_GESTURE_ID = 62;
        public readonly int BACK_RIGHT_GESTURE_ID = 71;
        public readonly int BACK_LEFT_GESTURE_ID = 72;
        public readonly int IDLE_ID = 20;
        public readonly int IDLE_Defending_ID = 22;


        private static System.IO.StreamWriter file;

        public int currentMovingState;

        private int firstGestureId;
        private int secondGestureId;

        private GesturesDetector()
	    {
            firstGestureId = 0;
            secondGestureId = 0;
            currentMovingState = IDLE_ID;
            file = new System.IO.StreamWriter("Test_Gestures_arm_defense.txt");

        }

        public static GesturesDetector getInstance() {

            if (gesturesDetector == null) gesturesDetector = new GesturesDetector();
            return gesturesDetector;
        }

        public int checkGesture(Skeleton skeleton) {

            firstGestureId = 0;
            secondGestureId = 0;

            float hipCenterX = skeleton.Joints[JointType.HipCenter].Position.X;
            float hipCenterZ = skeleton.Joints[JointType.HipCenter].Position.Z;
        
            float shoulderCenterX = skeleton.Joints[JointType.ShoulderCenter].Position.X;
            float shoulderCenterZ = skeleton.Joints[JointType.ShoulderCenter].Position.Z;

            float shoulderRightX = skeleton.Joints[JointType.ShoulderRight].Position.X;

            float shoulderLeftY = skeleton.Joints[JointType.ShoulderLeft].Position.Y;

            float elbowLeftY = skeleton.Joints[JointType.ElbowLeft].Position.Y;
   
            float HandLeftX = skeleton.Joints[JointType.HandLeft].Position.X;
            float HandLeftY = skeleton.Joints[JointType.HandLeft].Position.Y;

            float spineY = skeleton.Joints[JointType.Spine].Position.Y;
     
            //Check Front Gesture
            if (hipCenterZ - shoulderCenterZ > FRONT_GESTURE_THRESHOLD) {
                firstGestureId = FRONT_GESTURE_ID;
            }

            //Check Back Gesture
            else if (hipCenterZ - shoulderCenterZ < BACK_GESTURE_THRESHOLD) {
                firstGestureId = BACK_GESTURE_ID; 
            }
        
            //Check Right Gesture
            if (hipCenterX - shoulderCenterX < RIGHT_GESTURE_THRESHOLD) {
                secondGestureId = RIGHT_GESTURE_ID;
            }

            //Check Left Gesture
            else if (hipCenterX - shoulderCenterX > LEFT_GESTURE_THRESHOLD) {
                secondGestureId = LEFT_GESTURE_ID;
            }

            int detectedGestureID = firstGestureId + secondGestureId;

            //Check if arm defending
            //file.WriteLine(shoulderRightX + ", " + HandLeftX + ", " + shoulderCenterX);
            if (HandLeftX > shoulderCenterX && HandLeftY > spineY + HANDLEFTY_SPINEY_MAXDIFFERENCE
                && shoulderLeftY - elbowLeftY < SHOULDERLEFTY_ELBOWLEFTY_MAXDIFFERENCE){
                    detectedGestureID = IDLE_Defending_ID;
            }

            currentMovingState = (detectedGestureID == 0) ? IDLE_ID : detectedGestureID;
        
            return currentMovingState;
        }
    }
}