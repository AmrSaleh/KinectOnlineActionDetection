using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;

namespace GraduationProject
{
    class KnnManager
    {

        private static KnnManager knnManager = null;
        private KNearestNeighbors knn;
        private double[][] codebook = new double[GlobalConstant.numberOfClusters][];
        private int[] clusterIndex = new int[GlobalConstant.numberOfClusters];

        private KnnManager()
        {
            // Read the file line by line
            string line;
            int index = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(GlobalConstant.coodbookFilePath);
            while ((line = file.ReadLine()) != null)
            {
                codebook[index] = Array.ConvertAll(line.Split(' '), new Converter<string, double>(Double.Parse));
                clusterIndex[index] = index;
                index++;
            }

            file.Close();

            knn = new KNearestNeighbors(k: GlobalConstant.numberOfKNearestNeighbour
                                        , classes: GlobalConstant.numberOfClusters
                                        , inputs: codebook
                                        , outputs: clusterIndex);
        }

        public static KnnManager getInstance()
        {
            if (knnManager == null) knnManager = new KnnManager();
            return knnManager;
        }

        public int[] getMatchingClusters(Descriptor descriptor)
        {
            // get the nearest 3 classes
            int[] labels;
            double[][] point = knn.GetNearestNeighbors(descriptor.getArray(), out labels);

            double[] distance = new double[GlobalConstant.numberOfKNearestNeighbour];
            for (int i = 0; i < GlobalConstant.numberOfKNearestNeighbour; i++)
            {
                distance[i] = Accord.Math.Distance.Euclidean(point[i], descriptor.getArray());
                //Console.WriteLine(distance[i]);
            }

            
            // sort 2 arrays related to each other
            Array.Sort(distance, labels);

            return labels;
        }

        

        


    }
}
