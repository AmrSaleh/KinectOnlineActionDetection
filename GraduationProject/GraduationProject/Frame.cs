using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;


namespace GraduationProject
{
    class Frame
    {
        // the id of the frame
        private int frameId;

        // The poss descriptor concatenated with the angular describtor
        private Descriptor descriptor = null;

        // The points in the 20 joints in the form of xyz sequence
        private double[] xyzSeq;

        // The measure of the 35 angles of the angular descriptor
        private double[] anglesSeq;

        private double[] frameScore;

        // matching clusters
        private int[] matches = null;

        public Frame(int frameId)
        {
            this.frameId = frameId;
            frameScore = new double[GlobalConstant.numberOfActions];
        }

        public Frame(Skeleton skeleton, int frameId)
        {
            this.frameId = frameId;
            xyzSeq = getXYZForm(skeleton);
            anglesSeq = getAngles(skeleton);

        }

        public Frame(float[,] skeleton, int frameId)
        {
            this.frameId = frameId;
            xyzSeq = getXYZForm(skeleton);
            anglesSeq = getAngles(skeleton);
        }

        public double[] getJoint(JointType jointType)
        {

            int jointId = (int)jointType;
            int position = jointId * 3;

            return new double[] { xyzSeq[position++] ,
                                   xyzSeq[position++] ,
                                   xyzSeq[position] };
        }


        public double[] getXYZSeq() {
            return xyzSeq;
        }

        public double[] getAnglesSeq()
        {
            return anglesSeq;
        }

        public int getFrameID()
        {
            return frameId;
        }


        public void setDescriptorAndInitializeScore(Descriptor descriptor, int[] matchesOfCorrespondingBeforeShiftFrame) 
        {
            this.descriptor = descriptor;
            intializeFrameScore(matchesOfCorrespondingBeforeShiftFrame);
        }

        private void intializeFrameScore(int [] matchesOfCorrespondingBeforeShiftFrame)
        {
            if (this.descriptor != null)
            {
                matches = KnnManager.getInstance().getMatchingClusters(descriptor);
                if (matchesOfCorrespondingBeforeShiftFrame != null)
                {
                    frameScore = Actions.getInstance().getFrameScore(matchesOfCorrespondingBeforeShiftFrame);
                }
                else
                {
                    frameScore = new double[GlobalConstant.numberOfActions];
                }
            }
        }

        public int[] getMatches()
        {
            return matches;
        }

        private double[] getXYZForm(Skeleton skeleton)
        {

            double[] seq = new double[skeleton.Joints.Count * 3];

            int i = 0;
            foreach (Joint joint in skeleton.Joints)
            {

                seq[i++] = joint.Position.X;
                seq[i++] = joint.Position.Y;
                seq[i++] = joint.Position.Z;

            }

            return seq;
        }


        private double[] getAngles(Skeleton skeleton)
        {

            int anglesNo = Angles.count;
            int[,] anglesIds = Angles.anglesIds;

            double[] angles = new double[anglesNo];

            for (int i = 0; i < anglesNo; i++)
            {

                double[] pointA = constructPoint(skeleton, anglesIds[i, 0]);
                double[] pointB = constructPoint(skeleton, anglesIds[i, 1]);
                double[] pointC = constructPoint(skeleton, anglesIds[i, 2]);

                angles[i] = getAnglesFromPoints(pointA, pointB, pointC);

            }

            return angles;
        }


        private double[] getXYZForm(float[,] skeleton)
        {

            double[] seq = new double[60];

            int i = 0;
            while(i < 60)
            {

                seq[i] = skeleton[i / 3, 0];
                i++;
                seq[i] = skeleton[i / 3, 1];
                i++;
                seq[i] = skeleton[i / 3, 2];
                i++;

            }

            return seq;
        }


        private double[] getAngles(float[,] skeleton)
        {

            int anglesNo = Angles.count;
            int[,] anglesIds = Angles.anglesIds;

            double[] angles = new double[anglesNo];

            for (int i = 0; i < anglesNo; i++)
            {
                double[] pointA = constructPoint(skeleton, anglesIds[i, 0]);
                double[] pointB = constructPoint(skeleton, anglesIds[i, 1]);
                double[] pointC = constructPoint(skeleton, anglesIds[i, 2]);

                angles[i] = getAnglesFromPoints(pointA, pointB, pointC);
            }

            return angles;
        }


        private double[] constructPoint(Skeleton skeleton, int angleId)
        {

            if (angleId == Angles.zeroPointId)
            {
                return new double[] { 0, 0, 0 };
            }
            return new double[] {
                skeleton.Joints[(JointType)angleId].Position.X,
                skeleton.Joints[(JointType)angleId].Position.Y,
                skeleton.Joints[(JointType)angleId].Position.Z
            };
        }

        private double[] constructPoint(float[,] skeleton, int angleId)
        {

            if (angleId == Angles.zeroPointId)
            {
                return new double[] { 0, 0, 0 };
            }
            return new double[] {
                skeleton[angleId, 0],
                skeleton[angleId, 1],
                skeleton[angleId, 2]
            };
        }

        private double getAnglesFromPoints(double[] pointA, double[] pointB, double[] pointC)
        {

            double[] v1 = getVector(pointA, pointB);
            double[] v2 = getVector(pointC, pointB);

            double normVal = dotProduct(v1, v1) * dotProduct(v2, v2) 
                             -
                             dotProduct(v1, v2) * dotProduct(v1, v2);

            double angleRadian = Math.Atan2(Math.Sqrt(normVal), dotProduct(v1, v2));
            
            return angleRadian;

        }


        private double[] getVector(double[] a, double[] b)
        {
            return new double[] { a[0] - b[0], a[1] - b[1], a[2] - b[2] };
        }


        private double dotProduct(double[] v1, double[] v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }

        public double[] getFrameScoreArray()
        {
            return frameScore;

        }

        public Descriptor getDescriptor()
        {
            return descriptor;
        }



        

    }

}
