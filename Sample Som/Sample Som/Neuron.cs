using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sample_Som
{
    class Neuron
    {
        public int xPos { get; set; }
        public int yPos { get; set; }
        public List<double> weights { get; set; }
        public String tag { get; set; }

        private Random rand;
        private int featureNum;
        private List<Song> songs;
        public Neuron()
        {
            featureNum = 68;
            weights = new List<double>();
            rand = new Random();
            songs = new List<Song>();
        }

        public void Initialize()
        {
            for(int i=0; i < featureNum; i++)
            {
                double randomWeight = rand.NextDouble();
                Console.WriteLine("Random Weight: " +randomWeight);
                weights.Add(randomWeight);
            }
        }

        public double GetDistance(List<double> inputVector)
        {
            double distance = 0;
            int featureCount = 0;
            for(int i = 0; i < featureNum; i++)
            { 
                if(weights[i] != -1)
                {
                    distance += Math.Pow((weights[i] - inputVector[i]), 2);
                    featureCount++;
                }
                
            }

            return distance / featureCount;
        }

        public void AdjustWeights(Song song, double learningRate)
        {
            for(int i = 0; i < weights.Count; i++)
            {
                double oldWeight = weights[i];
                double songWeight = song.Features[i];
                weights[i] = oldWeight + (learningRate * (songWeight - oldWeight));
            }
            Debug.WriteLine("Weights updated");
        }

        public void AddSongToList(Song song)
        {
            songs.Add(song);
        }
    }
}
