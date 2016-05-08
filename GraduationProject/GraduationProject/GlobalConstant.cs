using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class GlobalConstant
    {
        // variables loaded from file
        public static int numberOfActions;
        public static int numberOfClusters;
        public static int numberOfBins;
        public static int numberOfKNearestNeighbour;
        public static int scale;
        public static int scoreScale;
        public static int kOnlineLatency;
        public static int minDetectionLength;
        public static string actionWeightsFilePath;
        public static string actionThresholdsFilePath;
        public static string coodbookFilePath;

        // final variables
        public static readonly int jointsCount = 20;
        public static readonly int anglesCount = 35;
        public static readonly int descriptorSize = jointsCount * 3 * 3 + anglesCount * 2;
        public static readonly int alpha = 1;
        public static readonly int beta = 1;
        public static readonly double psi = 60f / 35f; // ~1.7
        public static readonly double eps = 0.0000000001f;


        public static void initializeConstants(string initFilePath)
        {
            // Read the init file line by line
            string line;
            int index = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(initFilePath);

            // read number of actions
            line = file.ReadLine();
            string[] splittedLine = line.Split(',');
            numberOfActions = int.Parse(splittedLine[1]);

            // read number of clusters
            line = file.ReadLine();
            splittedLine = line.Split(',');
            numberOfClusters = int.Parse(splittedLine[1]);

            // read number of soft binning bins
            line = file.ReadLine();
            splittedLine = line.Split(',');
            numberOfBins = int.Parse(splittedLine[1]);

            // read number of k nearest neighbours
            line = file.ReadLine();
            splittedLine = line.Split(',');
            numberOfKNearestNeighbour = int.Parse(splittedLine[1]);

            // read scale
            line = file.ReadLine();
            splittedLine = line.Split(',');
            scale = int.Parse(splittedLine[1]);

            // read score scale
            line = file.ReadLine();
            splittedLine = line.Split(',');
            scoreScale = int.Parse(splittedLine[1]);

            // read kOnlineLatency scale
            line = file.ReadLine();
            splittedLine = line.Split(',');
            kOnlineLatency = int.Parse(splittedLine[1]);

            // read minDetectionLength scale
            line = file.ReadLine();
            splittedLine = line.Split(',');
            minDetectionLength = int.Parse(splittedLine[1]);

            // read path of actions weights file
            line = file.ReadLine();
            splittedLine = line.Split(',');
            actionWeightsFilePath = splittedLine[1];


            // read path of actions Thresholds file
            line = file.ReadLine();
            splittedLine = line.Split(',');
            actionThresholdsFilePath = splittedLine[1];

            // read path of codebook
            line = file.ReadLine();
            splittedLine = line.Split(',');
            coodbookFilePath = splittedLine[1];


            file.Close();
        }
    }
}
