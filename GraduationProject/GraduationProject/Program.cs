using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using MathNet.Numerics.LinearAlgebra;
using System.IO;


namespace GraduationProject
{
    class Program
    {

        private static int framesCount = 0; 

        // Active Kinect sensor
        private static KinectSensor sensor;

        // Get instance of the online detector
        private static OnlineDetector onlineDetector;

        // Joints IDs map
        private static Dictionary<int, int> jointsIdsMap;

        //get from file boolean
        private static bool FROM_FILE = true;

        // Perform MSCR-12 Mapping or not
        private static bool DO_MAPPING = false;

      
        private static ArrayList seqFrameAnnotation = new ArrayList();
        private static ArrayList detectedLabels = new ArrayList();

        static void Main(string[] args)
        {
            GlobalConstant.initializeConstants("init.txt");
            onlineDetector = OnlineDetector.getInstance();

            if (FROM_FILE)
            {
                Console.WriteLine("\nWorking from file. Pls w8, This will take a bit to run.");
                 setJointsIdsMap();
                 loadFromFile();
            } else
            {
                connectKinect();
                Console.WriteLine("Kinect Connected!, Press any key to continue");
                Console.ReadLine();
                closeKinectConnections();
            }

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("seqFrameAnnotation.txt"))
            {
                String line;
                for (int i = 0; i < seqFrameAnnotation.Count; i++)
                {
                    line = seqFrameAnnotation[i] + "";
                    file.WriteLine(line);
                }


            }

            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter("detectedLabels.txt"))
            {
                String line;
                for (int i = 0; i < detectedLabels.Count; i++)
                {
                    line = detectedLabels[i] + "";
                    file.WriteLine(line);
                }


            }

            Console.WriteLine("\nProgram Fininshed, Press any key to exit.");
            Console.ReadLine();
        }


        private static void connectKinect()
        {

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    sensor = potentialSensor;
                    break;
                }
            }

            if (null != sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                sensor.SkeletonFrameReady += SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    sensor.Start();
                }
                catch (IOException)
                {
                    sensor = null;
                }
            }

            if (null == sensor)
            {
                //this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }



        private static void closeKinectConnections()
        {
            if (null != sensor)
            {
                sensor.Stop();
            }
        }

 

        private static void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {

            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            if (skeletons.Length != 0)
            {

                int skeletonNo = 0;

                foreach (Skeleton skel in skeletons)
                {

                    skeletonNo++;

                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        
                        addFrameAndCheckAction(skeletonNo, skel);
                        
                    }

                }

            }
            
        }


        /*
            Assumption
            We assume that there is only one player
        */

        private static void addFrameAndCheckAction(int skeletonNo, Skeleton skeleton) {

            Frame newFrame = new Frame(skeleton, framesCount);

            int actionLabel = onlineDetector.addFrameAndCheckAction(newFrame);

            if (actionLabel != -1)
                Console.WriteLine("Action Detected: " + actionLabel);

            framesCount++;
        }


        private static void addFrameAndCheckAction(int skeletonNo, float[,] skeleton)
        {

            Frame newFrame = new Frame(skeleton, framesCount);

            int actionLabel = onlineDetector.addFrameAndCheckAction(newFrame);

            if (actionLabel != -1)
            {
                detectedLabels.Add(actionLabel);
            }
            seqFrameAnnotation.Add(actionLabel);
                

            framesCount++;
        }


        private static void loadFromFile() {

            int jointNo = 0;

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(@"testUnsegmentedSeq.txt");

            // Display the file contents by using a foreach loop.
            //System.Console.WriteLine("Reading file");

            float[,] skeleton = new float[20, 3];
            int i = 0;

            foreach (string line in lines)
            {

                // Use a tab to indent each line of the file.
                //Console.WriteLine(lineNo + "\t" + line);

                String[] xyzpString = line.Split();
                float[] xyz = { float.Parse(xyzpString[0]),
                                float.Parse(xyzpString[1]),
                                float.Parse(xyzpString[2])};


                //creating Skeleton joint
                skeleton[jointNo, 0] = xyz[0];
                skeleton[jointNo, 1] = xyz[1];
                skeleton[jointNo, 2] = xyz[2];

                jointNo++;

                if (jointNo >= 20)
                {
                    addFrameAndCheckAction(0, reOrder(skeleton));
                    skeleton = new float[20, 3];
                    jointNo = 0;
                    i++;
                }

            }

            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();

        }

        private static float[,] reOrder(float[,] skeleton) {
            float[,] result = new float[20, 3];

            for (int i = 0; i < 20; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    if (DO_MAPPING)
                        result[i, j] = skeleton[jointsIdsMap[i], j];
                    else
                        result[i, j] = skeleton[i, j];
                }
            }

            return result;

        }

        private static void setJointsIdsMap()
        {

            jointsIdsMap = new Dictionary<int, int>();

            int id = 0;

            jointsIdsMap.Add(id++, 6);
            jointsIdsMap.Add(id++, 3);
            jointsIdsMap.Add(id++, 2);
            jointsIdsMap.Add(id++, 19);
            jointsIdsMap.Add(id++, 0);
            jointsIdsMap.Add(id++, 7);
            jointsIdsMap.Add(id++, 9);
            jointsIdsMap.Add(id++, 11);
            jointsIdsMap.Add(id++, 1);
            jointsIdsMap.Add(id++, 8);
            jointsIdsMap.Add(id++, 10);
            jointsIdsMap.Add(id++, 12);
            jointsIdsMap.Add(id++, 4);
            jointsIdsMap.Add(id++, 13);
            jointsIdsMap.Add(id++, 15);
            jointsIdsMap.Add(id++, 17);
            jointsIdsMap.Add(id++, 5);
            jointsIdsMap.Add(id++, 14);
            jointsIdsMap.Add(id++, 16);
            jointsIdsMap.Add(id++, 18);



        }


    }
}
