using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sample_Som
{
    class Driver
    {
        public static void Main()
        {
            
        }

        public void simulateFormula()
        {
            decimal value;
            int radius = 6;
            double learningRate = 1.0;
            int iterations = 20000;
            int currentIteration = 1;

            for (int i = 0; i < iterations; i++)
            {
                Debug.WriteLine(iterations);
                Debug.WriteLine(currentIteration);
                DecayLearningRate();
                currentIteration++;
            }

            void foo()
            {
                value = radius * ((decimal)(iterations - currentIteration) / (decimal)iterations);
                radius = (int)Math.Ceiling(value);
                if (radius < 1)
                {
                    radius = 1;
                }

                Debug.WriteLine("Value: " + value);
            }

            void DecayLearningRate()
            {
                learningRate = learningRate * ((double)(iterations - currentIteration) / (double)iterations);
                learningRate = Math.Round(learningRate, 1);
                if (learningRate < 0.1)
                {
                    learningRate = 0.1;
                }
                Debug.WriteLine("Learning Rate: " + learningRate);
            }
        }
        
    }
}
