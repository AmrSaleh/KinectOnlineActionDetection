using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using MathNet.Numerics.LinearAlgebra;

namespace GraduationProject
{
    class DescriptorBuffer:FrameBuffer
    {

        public DescriptorBuffer(int scale) : base(scale)
        {

        }

        public Frame checkAndSetFramesDescriptor()
        {
            if (buffer.Count == bufferSize)
            {
                Frame [] currentBuffer = buffer.ToArray();
                // set frame descriptor of the middle frame index:(scale+1)
                Descriptor descriptor = getDescriptor(currentBuffer);

                int[] matchesOfCorrespondingBeforeShiftFrame = currentBuffer[0].getMatches();

                int middle = bufferSize / 2;
               
                currentBuffer[middle].setDescriptorAndInitializeScore(descriptor, matchesOfCorrespondingBeforeShiftFrame);
               
                return currentBuffer[middle];
            }
            return null;
        }



        private Descriptor getDescriptor(Frame[] currentBuffer)
        {
            int middle = bufferSize / 2;

            double[] frame = currentBuffer[middle].getXYZSeq();
            //get p, dp, d2p
            Matrix<double> pMatrix = getP(currentBuffer[middle]);

            Matrix<double> dpMatrix = getDP(currentBuffer[middle - 1],
                                            currentBuffer[middle + 1]);

            Matrix<double> d2pMatrix = getD2P(currentBuffer[middle - 2],
                                              currentBuffer[middle],
                                              currentBuffer[middle + 2]);

            //get a, da
            Matrix<double> aMatrix = getA(currentBuffer[middle]);

            Matrix<double> daMatrix = getDA(currentBuffer[middle - 1],
                                            currentBuffer[middle + 1]);

            //get Descriptor
            dpMatrix = dpMatrix.Multiply(GlobalConstant.alpha);
            d2pMatrix = d2pMatrix.Multiply(GlobalConstant.beta);
            aMatrix = aMatrix.Multiply(GlobalConstant.psi);
            daMatrix = daMatrix.Multiply(GlobalConstant.psi);

            double[] descriptorArray = getFinalDescriptor(
                pMatrix,
                dpMatrix,
                d2pMatrix,
                aMatrix,
                daMatrix
                );

            Descriptor descriptor = new Descriptor(descriptorArray);
    
            printDescriptor(currentBuffer[middle].getFrameID(), descriptorArray);

            return descriptor;
        }


        private Matrix<double> getP(Frame middleFrame) {
            double[] p = getRelativePos(middleFrame.getXYZSeq(),
                                        middleFrame.getJoint(JointType.HipCenter));
            return normalizeSeq(p);

        }


        private Matrix<double> getDP(Frame firstFrame, Frame lastFrame) {

            double[] dp = getFirstDerivative(firstFrame.getXYZSeq(),
                                             lastFrame.getXYZSeq());

            return normalizeSeq(dp);

        }

        private Matrix<double> getD2P(Frame firstFrame, Frame middleFrame, Frame lastFrame) {

            double[] d2p = getSecondDerivative(firstFrame.getXYZSeq(),
                                               middleFrame.getXYZSeq(),
                                               lastFrame.getXYZSeq());

            return normalizeSeq(d2p);

        }

        private Matrix<double> getA(Frame middleFrame) {

            double[] a = middleFrame.getAnglesSeq();

            return normalizeSeq(a);

        }

        private Matrix<double> getDA(Frame firstFrame, Frame lastFrame) {

            double[] da = getFirstDerivative(firstFrame.getAnglesSeq(),
                                             lastFrame.getAnglesSeq());

            return normalizeSeq(da);

        }


        private Matrix<double> normalizeSeq(double[] seq) {

            Matrix<double> matrix = Matrix<double>.Build.DenseOfColumnMajor(1, seq.Length, seq);

            double norm = matrix.L2Norm();

            matrix = matrix.Divide(norm + GlobalConstant.eps);

            return matrix;
        }


        private double[] getRelativePos(double[] xyzSeq, double[] joint)
        {

            double[] res = new double[xyzSeq.Length];

            int i = 0;
            while (i < xyzSeq.Length)
            {

                res[i] = xyzSeq[i] - joint[0];
                i++;
                res[i] = xyzSeq[i] - joint[1];
                i++;
                res[i] = xyzSeq[i] - joint[2];
                i++;
            }

            return res;
        }



        private double[] getFirstDerivative(double[] firstSeq, double[] lastSeq)
        {

            double[] res = new double[firstSeq.Length];


            for (int i = 0; i < firstSeq.Length; i++)
            {

                res[i] = lastSeq[i] - firstSeq[i];

            }

            return res;
        }



        private double[] getSecondDerivative(double[] firstSeq, double[] midSeq, double[] lastSeq)
        {

            double[] res = new double[firstSeq.Length];

            for (int i = 0; i < firstSeq.Length; i++)
            {

                res[i] = firstSeq[i] + lastSeq[i] - 2 * midSeq[i];

            }

            return res;
        }


        private double[] getFinalDescriptor(params Matrix<double>[] values)
        {

            double[] result = new double[GlobalConstant.descriptorSize];
            int lastLength = 0;

            for (int i = 0; i < values.Length; i++)
            {

                double[] array = values[i].ToColumnWiseArray();

                array.CopyTo(result, lastLength);
                lastLength += array.Length;
            }

            return result;
        }

        private void printDescriptor(int descriptorFrameId, double[] descriptor) {

            //printing
            if (descriptorFrameId == 0)
            {

                //printing in file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"result.txt"))
                {
                    file.WriteLine(string.Join(", ", descriptor.Select(v => v.ToString())) + "\n");
                }

                    //Console.WriteLine("My array: {0}",
                    //string.Join(", ", descriptor.Select(v => v.ToString())))
                ;

            }
        }
             

    }
}
