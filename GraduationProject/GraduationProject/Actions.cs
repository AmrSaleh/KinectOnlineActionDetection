using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class Actions
    {
        private static Actions actions = null;
        private double[][] actionWeights;
        private double[] actionsThresholds;

        //private string[] msrDataSetActions;
        private Dictionary<int, String> actionsMap;

        private Actions()
        {
            actionWeights = new double[GlobalConstant.numberOfActions][];
            actionsThresholds = new double[GlobalConstant.numberOfActions];
            initializeActionWeights();
            initializeActionThresholds();
        }

        public void initializeMSRDatasetActionsArray()
        {

            GesturesDetector gestureDetector = GesturesDetector.getInstance();

            actionsMap = new Dictionary<int, String>();

            actionsMap.Add(0, "High Arm Wave");
            actionsMap.Add(1, "Horizontal Arm Wave");
            actionsMap.Add(2, "Hammer");
            actionsMap.Add(3, "Hand Catch");
            actionsMap.Add(4, "Forward Punch");
            actionsMap.Add(5, "High Throw");
            actionsMap.Add(6, "Draw X");
            actionsMap.Add(7, "Draw Tick");
            actionsMap.Add(8, "Draw Circle");
            actionsMap.Add(9, "Hand Clap");
            actionsMap.Add(10, "Two Hand Wave");
            actionsMap.Add(11, "Side - Boxing");
            actionsMap.Add(12, "Bend");
            actionsMap.Add(13, "Forward Kick");
            actionsMap.Add(14, "Side Kick");
            actionsMap.Add(15, "Jogging");
            actionsMap.Add(16, "Tennis Swing");
            actionsMap.Add(17, "Tennis Serve");
            actionsMap.Add(18, "Golf Swing");
            actionsMap.Add(19, "Pickup & Throw");
            actionsMap.Add(gestureDetector.FRONT_GESTURE_ID, "Front Gesture");
            actionsMap.Add(gestureDetector.BACK_GESTURE_ID, "Back Gesture");
            actionsMap.Add(gestureDetector.RIGHT_GESTURE_ID, "Right Gesture");
            actionsMap.Add(gestureDetector.LEFT_GESTURE_ID, "Left Gesture");
            actionsMap.Add(gestureDetector.FRONT_RIGHT_GESTURE_ID, "Front Right Gesture");
            actionsMap.Add(gestureDetector.FRONT_LEFT_GESTURE_ID, "Front Left Gesture");
            actionsMap.Add(gestureDetector.BACK_RIGHT_GESTURE_ID, "Back Right Gesture");
            actionsMap.Add(gestureDetector.BACK_LEFT_GESTURE_ID, "Back Left Gesture");
            actionsMap.Add(gestureDetector.IDLE_ID, "Idle");

        }



        public string getActionName(int actionId)
        {
            return actionsMap[actionId];
        }

        public string get2bytesID(int actionId)
        {
            if (actionId <= 9)
                return "0"+actionId;         
            else
                return actionId + "";
        }

        private void initializeActionThresholds()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(GlobalConstant.actionThresholdsFilePath);
            int index = 0;
            String line = "";
            while ((line = file.ReadLine()) != null)
            {
                actionsThresholds[index] = double.Parse(line);
                index++;
            }
            file.Close();
        }

        public static Actions getInstance()
        {
            if (actions == null) actions = new Actions();
            return actions;
        }

        private void initializeActionWeights()
        {
            // Read the file and display it line by line.
            string line;
            int index = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(GlobalConstant.actionWeightsFilePath);
            while ((line = file.ReadLine()) != null)
            {
                actionWeights[index] = Array.ConvertAll(line.Split(' '), new Converter<string, double>(Double.Parse));
                index++;
            }
            file.Close();
        }

        public double[] getFrameScore(int[] matchingClustersIndices)
        {
            double[] result = new double[GlobalConstant.numberOfActions];
            for (int i = 0; i < GlobalConstant.numberOfActions; i++)
            {
                result[i] = calculateFrameScore(i, matchingClustersIndices);
            }
            return result;
        }

        private double calculateFrameScore(int actionNumber, int[] matchingClustersIndices)
        {
            double calculatedScore = 0;

            for (int i = 1; i <= GlobalConstant.numberOfBins; i++)
            {
                calculatedScore = calculatedScore + (actionWeights[actionNumber][matchingClustersIndices[i - 1]] / i);
            }


            return calculatedScore;
        }

        public double getActionThreshold(int actionNo)
        {
            return this.actionsThresholds[actionNo];
        }
    }
}
