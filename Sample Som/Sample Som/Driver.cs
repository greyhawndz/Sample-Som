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
            SongReader reader = new SongReader();
            reader.ReadXML("data_Rock.i3dmods");
            reader.ReadXML("data_Blues.i3dmods");
            reader.ReadXML("data_Country.i3dmods");
            reader.ReadXML("data_Reggae.i3dmods");

            VanillaSOM som = new VanillaSOM(reader.songs, 4, 4);
            som.Train();
            //SOM1 structuredSOM = new SOM1(reader.songs);
            //structuredSOM.Train();
            //simulateFormula();
        }

        public static void simulateFormula()
        {
            double value;
            int radius = 6;
            double learningRate = 1.0;
            int iterations = 20000;
            int currentIteration = 1;

            for (int i = 0; i < iterations; i++)
            {
                if(currentIteration % 100 == 0)
                {
                    Debug.WriteLine(currentIteration);
                }
                DecayLearningRate();
                foo();
                currentIteration++;
            }

            void foo()
            {
                value = 6 * ((double)(iterations - currentIteration) / (double)iterations);
                radius = (int)Math.Ceiling(value);
                if (radius < 1)
                {
                    radius = 1;
                }

                //Debug.WriteLine("Value: " + value);
                if (currentIteration % 100 == 0)
                {

                    Debug.WriteLine("Radius: " + radius);
                }
            }

            void DecayLearningRate()
            {
                learningRate = 1.0 * ((double)(iterations - currentIteration) / (double)iterations);
                if (learningRate < 0.1)
                {
                    learningRate = 0.1;
                }
                // Debug.WriteLine("Learning Rate: " + learningRate);
                if (currentIteration % 100 == 0)
                {
                    
                    Debug.WriteLine("Learning Rate: " + learningRate);
                }
            }
        }
        
    }
}
