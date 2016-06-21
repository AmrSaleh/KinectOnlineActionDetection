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

        private string[] msrDataSetActions;
        private string[] actionID3bytes;

        private Actions()
        {
            actionWeights = new double[GlobalConstant.numberOfActions][];
            actionsThresholds = new double[GlobalConstant.numberOfActions];
            initializeActionWeights();
            initializeActionThresholds();
            initializeActionID3bytesArray();
        }

        public void initializeMSRDatasetActionsArray()
        {
            msrDataSetActions = new string[] { "high arm wave", "horizontal arm wave", "hammer", "hand catch", "forward punch", "high throw", "draw x", "draw tick", "draw circle", "hand clap", "two hand wave", "side - boxing", "bend", "forward kick", "side kick", "jogging", "tennis swing", "tennis serve", "golf swing", "pickup & throw" };
        }

        public string getActionName(int actionId)
        {
            return msrDataSetActions[actionId];
        }

        public void initializeActionID3bytesArray()
        {
            actionID3bytes = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08",
                "09", "10", "11", "12","13","14","15","16","17","18","19"};
        }

        public string get3bytesID(int actionId)
        {
            return actionID3bytes[actionId];
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
