using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class Descriptor
    {
        private double[] desciptorArray;

        public Descriptor(double[] descriptorArray) {
            this.desciptorArray = descriptorArray;
        }

        public double[] getArray()
        {
           return this.desciptorArray;
        }

        public void printDescriptor()
        {
            Console.Write("\nDescriptor:\n");
            for (int i = 0; i < desciptorArray.Length; i++)
            {
                Console.Write(desciptorArray[i]+" ");
            }
            Console.Write("\n\n");
        }
    }
}
