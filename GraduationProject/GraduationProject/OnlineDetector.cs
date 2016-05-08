using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace GraduationProject
{
    class OnlineDetector
    {
        private static OnlineDetector onlineDetector = null;

        private DescriptorBuffer descriptorBuffer;
        private ScoreBuffer scoreBuffer;
        private ActionDetector actionDetector;
        private double[] sliceScoreArray;
        //private double[] initialFrameScoreArray;
        private OnlineDetector()
        {
            descriptorBuffer = new DescriptorBuffer(GlobalConstant.scale);
            scoreBuffer = new ScoreBuffer(GlobalConstant.scoreScale);
            actionDetector = ActionDetector.getInstance();
        }

        public static OnlineDetector getInstance()
        {
            if (onlineDetector == null) onlineDetector = new OnlineDetector();
            return onlineDetector;
        }

 

        public int addFrameAndCheckAction(Frame newFrame)
        {
            // add new frame to the frameBuffer
            descriptorBuffer.addFrame(newFrame);

            // set descriptor for the middle frame 
            Frame frame = descriptorBuffer.checkAndSetFramesDescriptor();

            //No action
             
            if (frame == null)
            {
                
                //sliceScoreArray = new double[GlobalConstant.numberOfActions];
                //initialFrameScoreArray = new double[GlobalConstant.numberOfActions];
                //Frame dummyFrame = new Frame(-1);
                //scoreBuffer.addFrame(dummyFrame);
                return -1;
            }
            else
            {
                // add the frame contribution to the current state of the action detector to identify action
                scoreBuffer.addFrame(frame);  // 3 

                // initialFrameScoreArray length is 20 (as it is for each action type)
                //initialFrameScoreArray = scoreBuffer.getInitialFramesScore();
                
                sliceScoreArray = scoreBuffer.getFramesScore();
                if (sliceScoreArray == null) { 
                    return -1;
                    //sliceScoreArray = new double[GlobalConstant.scoreScale * 2 + 1];
                }
                
            }
            // detect action
            //returning action index for this frame
            return actionDetector.updateState(sliceScoreArray);
            
        }
    }
}
