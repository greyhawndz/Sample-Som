using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using System.Diagnostics;


namespace Sample_Som
{
    class SOM1
    {
        //TO DO:
        /*
         1. Change distance formula to average - done
         2. Change radius to 6 - done
         3. Change pythagorean theorem
         4. Change decay rate - done
         5. Update neighbors immediately
         6. convert song list to library
         */

        int iterations = 20000;
        int currentIteration;
        double learningRate = 1;
        int radius = 6;
        int row = 6;
        int col = 6;
        int songIndex = 0;
        int bestMatchIndexX = 0;
        int bestMatchIndexY = 0;
        double time;
        List<Song> songs;
        Neuron[,] map = null;
        Neuron bestMatchingUnit;
        List<Neuron> neighbors;
        Random rand;

        public SOM1(List<Song> songs)
        {
            GenerateRandomWeights();
            this.songs = songs;
            map = new Neuron[row, col];
            currentIteration = 1;
            radius = Math.Max(row, col) / 2;
            time = iterations / Math.Log10(radius);
            neighbors = new List<Neuron>();
            rand = new Random();
        }

        public void GenerateRandomWeights()
        {
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < col; j++)
                {
                    Neuron neuron = new Neuron();
                    neuron.Initialize();
                    neuron.xPos = i;
                    neuron.yPos = j;

                    //Set genre tags
                    if(i >= 0 && i < row/2 && j >= 0 && j < col/2)
                    {
                        neuron.tag = "Rock";
                    }
                    else if(i >= 0 && i < row/2 && j >= col/2 && j < col)
                    {
                        neuron.tag = "Reggae";
                    }
                    else if (i >= row/2 && i < row && j >= 0 && j < col/2)
                    {
                        neuron.tag = "Country";
                    }
                    else if (i >= row/2 && i < row && j >= col/2 && j < col)
                    {
                        neuron.tag = "Blues";
                    }
                }
            }
        }

        public void SetBestMatchingUnit()
        {
            double value;
            double temp;
            
            bestMatchIndexX = 0;
            bestMatchIndexY = 0;
            songIndex = rand.Next(0, songs.Count);
            value = map[0, 0].GetDistance(songs[songIndex].Features);
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < col; j++)
                {
                    temp = map[i,j].GetDistance(songs[songIndex].Features);
                    Console.WriteLine("Temp: " + temp);
                    Console.WriteLine("Value: " + value);
                    if (temp < value && songs[songIndex].Genre == map[i,j].tag)
                    {
                        Console.WriteLine("Genre Tag: " + map[i, j].tag);
                        Console.WriteLine("New Best Matching Unit");
                        value = temp;
                        bestMatchingUnit = map[i,j];
                    }
                }
            } 

        }

        public void FindKNearestNeighbor()
        {
            for(int i = 1; i <= radius; i++)
            {
                for(int j = 1; j <= radius; j++)
                {
                    //Up
                    if(bestMatchingUnit.xPos - i >= 0)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Down
                    if(bestMatchingUnit.xPos + i < row)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Left
                    if(bestMatchingUnit.yPos - i >= 0)
                    {
                        map[bestMatchingUnit.xPos, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Right
                    if(bestMatchingUnit.yPos + i < col)
                    {
                        map[bestMatchingUnit.xPos, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Bottom-Right
                    if(bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos + j < col)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Upper-Left
                    if(bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos - j >= 0)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Upper-Right
                    if(bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos + j < col)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], learningRate);
                    }
                    //Bottom-Left
                    if(bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos - j >= 0)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], learningRate);
                    }
                }
            }
            
        }

        

        public void Train()
        {
            double value;
            double temp;

            bestMatchIndexX = 0;
            bestMatchIndexY = 0;
            value = map[0, 0].GetDistance(songs[songIndex].Features);
            //Global Ordering
            for (int i = 0; i < iterations; i++)
            {
                SetBestMatchingUnit();
                FindKNearestNeighbor();
                DecayRadius();
                DecayLearningRate();
                currentIteration++;
            }
            learningRate = 0.1;
            currentIteration = 1;
            //Fine Adjustment
            for(int i = 0; i < iterations; i++)
            {
                SetBestMatchingUnit();
                FindKNearestNeighbor();
                currentIteration++;
            }

            //Add songs to neurons
            for(int i = 0; i < songs.Count; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        temp = map[j, x].GetDistance(songs[i].Features);
                        Console.WriteLine("Temp: " + temp);
                        Console.WriteLine("Value: " + value);
                        if (temp < value)
                        {
                            Console.WriteLine("New Best Matching Unit");
                            value = temp;
                            bestMatchIndexX = j;
                            bestMatchIndexY = x;
                        }
                    }
                    map[bestMatchIndexX, bestMatchIndexY].AddSongToList(songs[i]);
                }
            }
        }

        public void DecayRadius()
        {
            decimal value;
            value = radius * ((decimal)(iterations - currentIteration) / (decimal)iterations);
            radius = (int) Math.Ceiling(value);
            if(radius < 1)
            {
                radius = 1;
            }

            Console.WriteLine("New neighborhood radius:" + radius);
        }

        public void DecayLearningRate()
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
