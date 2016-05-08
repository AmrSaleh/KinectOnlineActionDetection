using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class FrameBuffer
    {
        protected Queue<Frame> buffer;
        protected int bufferSize;
        protected int scale;
        private Frame midFrameIndex;


        public FrameBuffer(int scale)
        {
            this.bufferSize = scale * 2 + 1;
            this.buffer = new Queue<Frame>();
            this.scale = scale;
        }

        public void addFrame(Frame frame)
        {
            if (buffer.Count == bufferSize) buffer.Dequeue();
            buffer.Enqueue(frame);
        }

        public Frame getMidFrame()
        {
            Frame[] currentBuffer = buffer.ToArray();
            int index = bufferSize / 2;
            return currentBuffer[index];
        }

        public int getBufferSize()
        {
            return bufferSize;
        }

        public int getScale()
        {
            return scale;
        }

        public Queue<Frame> getBuffer()
        {
            return buffer;
        }
    }
}
