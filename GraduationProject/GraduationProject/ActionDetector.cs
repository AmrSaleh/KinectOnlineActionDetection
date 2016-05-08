using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{

    class ActionDetector
    {

        private int nActions;
        private int[] actionsCurrentCount;
        private double[] maxSum;
        private double[] crntSum;
        private int[] negPointsCnt; // for kOnlineLatency
        private Actions actions;
        private int kOnlineLatency;
        private int minDetectionLength; // to discard very short detections.

        private static ActionDetector actionDetector = null;

        private ActionDetector()
        {
            nActions = GlobalConstant.numberOfActions;

            actionsCurrentCount = new int[nActions];
            maxSum = new double[nActions];
            crntSum = new double[nActions];
            negPointsCnt = new int[nActions]; // for kOnlineLatency
            kOnlineLatency = GlobalConstant.kOnlineLatency;
            minDetectionLength = GlobalConstant.minDetectionLength; // to discard very short detections.
            actions = Actions.getInstance();

        }

        public static ActionDetector getInstance()
        {
            if (actionDetector == null) actionDetector = new ActionDetector();
            return actionDetector;
        }

        int index = 3;
        double ptScore;
        public int updateState(double[] sliceScoreArray)
        {
          

            for (int actionID = 0; actionID < nActions; actionID++)
            {
                // apply weighted moving average with one frame before and after
                // the current frame


                //I think this is some kind of normalization
                ptScore = sliceScoreArray[actionID];

                /*
                if (actionID == 5)
                {
                    Console.WriteLine("actionsThresholds=" + actions.getActionThreshold(actionID));
                    Console.WriteLine("actionsCurrentCount[actionID] = " + actionsCurrentCount[actionID]);
                    Console.WriteLine("index = " + index++);
                    //Console.WriteLine("Initial score = " + initialFrameScoreArray[actionID]);
                    Console.WriteLine("ptScore = " + ptScore);
                    Console.WriteLine("maxSum = " + maxSum[actionID]);
                    Console.WriteLine("crntSum = " + crntSum[actionID]);
                    Console.WriteLine("negPointsCnt = " + negPointsCnt[actionID]);
                    //Console.WriteLine("SliceScore for frame " + frame + " and action 0 = " + ptScore);
                    Console.WriteLine("press any key to continue");
                    Console.ReadLine();
                }
                */
                if (maxSum[actionID] >= actions.getActionThreshold(actionID))
                {
                    if (ptScore > 0)
                    {
                        negPointsCnt[actionID] = 0;
                    }
                    else if (ptScore < 0)
                    {
                        // update count of -ve points to trigger detection
                        negPointsCnt[actionID] = negPointsCnt[actionID] + 1;

                        if (negPointsCnt[actionID] >= kOnlineLatency &&
                            actionsCurrentCount[actionID] >= minDetectionLength)
                        {


                            /// reset variable and return
                            /// 


                            // communicate detection to other actions
                            crntSum = new double[nActions];
                            maxSum = new double[nActions];
                            actionsCurrentCount = new int[nActions];
                            negPointsCnt = new int[nActions];

                            index--;
                            updateState(sliceScoreArray);
                            
                            return actionID;

                        }
                    }
                }

                if (crntSum[actionID] == 0 && ptScore > 0)
                {
                    actionsCurrentCount[actionID] = 0;
                }
                crntSum[actionID] = (crntSum[actionID] + ptScore);
                if (crntSum[actionID] < 0)
                {
                    crntSum[actionID] = 0;
                    maxSum[actionID] = 0;
                    actionsCurrentCount[actionID] = 0;
                }

                if (crntSum[actionID] > maxSum[actionID])
                {
                    maxSum[actionID] = crntSum[actionID];
                    actionsCurrentCount[actionID]++;
                }
            }




            return -1;
        }


        private static double mean(int[] values, int start, int end)
        {
            double s = 0;

            for (int i = start; i <= end; i++)
            {
                s += values[i];
            }

            return s / (end - start + 1);
        }

        private static int[] findAll(int[] array, int value)
        {
            int[] result = new int[array.Length];
            int index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    result[index] = i;
                    index++;
                }
            }

            if (index == 0) return null;

            int[] result2 = new int[index];
            //   result = new List<int>(result).GetRange(0, index - 1).ToArray();
            for (int i = 0; i < index; i++)
            {
                result2[i] = result[i];
            }

            return result2;
        }

        private static int[] getValuesByIndeces(int[] data, int[] indeces)
        {
            int[] result = new int[indeces.Length];
            for (int i = 0; i < indeces.Length; i++)
            {
                result[i] = data[indeces[i]];
            }

            return result;
        }

        private static void setBoolValues(bool[] boolArray, int start, int end, bool value)
        {
            for (int i = start; i < end + 1; i++)
            {
                boolArray[i] = value;
            }

        }

        private static void setIntValues(int[] array, bool[] indeces, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (indeces[i] == true)
                {
                    array[i] = value;
                }

            }

        }

        private static void logicalAndAtIndeces(bool[] array, int[] indeces, bool value)
        {
            for (int i = 0; i < indeces.Length; i++)
            {
                array[indeces[i]] = array[indeces[i]] & value;
            }
        }
    }
}
