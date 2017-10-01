﻿using System;
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
         3. Change pythagorean theorem - done
         4. Change decay rate - done
         5. Update neighbors immediately - done
         6. convert song list to library/table - to be done by nica
         7. Read xml
         */

        int iterations = 20000;
        int currentIteration;
        double learningRate = 1;
        double newLearningRate;
        int radius = 6;
        int newRadius;
        int row = 6;
        int col = 6;
        int songIndex = 0;
        int bestMatchIndexX = 0;
        int bestMatchIndexY = 0;
        List<Song> songs;
        Neuron[,] map = null;
        Neuron bestMatchingUnit;
        Random rand;

        public SOM1(List<Song> songs)
        {
            GenerateRandomWeights();
            this.songs = songs;
            map = new Neuron[row, col];
            currentIteration = 1;
            rand = new Random();
            newLearningRate = learningRate;
            newRadius = radius;
            ExtractFeatures();
        }

        public void ExtractFeatures()
        {
            for(int i = 0; i < songs.Count; i++)
            {
                songs[i].ExtractFeatures();
            }
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
            for(int i = 1; i <= newRadius; i++)
            {
                for(int j = 1; j <= newRadius; j++)
                {
                    //Up
                    if(bestMatchingUnit.xPos - i >= 0)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Down
                    if(bestMatchingUnit.xPos + i < row)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Left
                    if(bestMatchingUnit.yPos - i >= 0)
                    {
                        map[bestMatchingUnit.xPos, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Right
                    if(bestMatchingUnit.yPos + i < col)
                    {
                        map[bestMatchingUnit.xPos, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Bottom-Right
                    if(bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos + j < col)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Upper-Left
                    if(bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos - j >= 0)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Upper-Right
                    if(bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos + j < col)
                    {
                        map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                    }
                    //Bottom-Left
                    if(bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos - j >= 0)
                    {
                        map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
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
            //I just set these to constants so that the system wont calculate for the decay and save computation time
            newLearningRate = 0.1;
            newRadius = 1;
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
            newRadius = (int) Math.Ceiling(value);
            if(newRadius < 1)
            {
                newRadius = 1;
            }

            Console.WriteLine("New neighborhood radius:" + newRadius);
        }

        public void DecayLearningRate()
        {
            newLearningRate = learningRate * ((double)(iterations - currentIteration) / (double)iterations);
            if (newLearningRate < 0.1)
            {
                newLearningRate = 0.1;
            }
            Debug.WriteLine("Learning Rate: " + newLearningRate);
        }

    }
}
