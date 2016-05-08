using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class ScoreBuffer : FrameBuffer
    {
        public ScoreBuffer(int scale) : base(scale)
        {  
        }

        public double[] getInitialFramesScore()
        {
           
                Frame[] currentBuffer = buffer.ToArray();
                int middle = buffer.Count / 2;
                return currentBuffer[middle].getFrameScoreArray();
            
          
        }

        new public Frame getMidFrame()
        {
            Frame[] currentBuffer = buffer.ToArray();
            int middle = buffer.Count / 2;
            return currentBuffer[middle];
        }

        public double[] getFramesScore()
        {
            double[] scores = null;
            if (buffer.Count == bufferSize)
            {
                Frame[] currentBuffer = buffer.ToArray();

                double[] ptScore = this.getMidFrame().getFrameScoreArray(); // score for single frame
                scores = new double[GlobalConstant.numberOfActions];

                for (int actionID = 0; actionID < GlobalConstant.numberOfActions; actionID++)
                {

                    double neighboursScoreSum = 0;
                    for (int i = 0; i < currentBuffer.Length; i++)
                    {
                        neighboursScoreSum += currentBuffer[i].getFrameScoreArray()[actionID]; // score for single frame
                    }
                    scores[actionID] = (neighboursScoreSum + (bufferSize - 2)*ptScore[actionID] ) / (2 * (bufferSize-1));
                }

            }
            return scores;
        }
    }
}
