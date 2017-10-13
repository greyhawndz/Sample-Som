using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using System.Diagnostics;

namespace Sample_Som
{
    class VanillaSOM
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
        int row;
        int col;
        int songIndex = 0;
        List<Song> songs;
        Neuron[,] map;
        Neuron bestMatchingUnit;
        Random rand;

        public VanillaSOM(List<Song> songs, int row, int col)
        {
            this.songs = songs;
            this.row = row;
            this.col = col;
            map = new Neuron[row, col];
            currentIteration = 1;
            rand = new Random();
            newLearningRate = learningRate;
            newRadius = radius;
            songIndex = rand.Next(0, songs.Count);
            Debug.WriteLine("In SOM");
            foreach (Song song in songs)
            {
                song.ExtractFeatures();
            }
            GenerateRandomWeights();
        }


        public void GenerateRandomWeights()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    map[i, j] = new Neuron();
                    map[i, j].Initialize();
                    map[i, j].xPos = i;
                    map[i, j].yPos = j;
                }
            }
        }

        public void SetBestMatchingUnit()
        {
            double value = -1;
            double temp = 0;

            songIndex = rand.Next(0, songs.Count);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    temp = map[i, j].GetDistance(songs[songIndex].Features);
                    if (value == -1)
                    {
                        value = temp;
                    }
                    // Console.WriteLine("Temp: " + temp);
                    // Console.WriteLine("Value: " + value);
                    if (temp <= value)
                    {
                        
                        Console.WriteLine("Title: " + songs[songIndex].Title);
                        Console.WriteLine("New Best Matching Unit");
                        Console.WriteLine("Best X: " + i + " Best Y: " + j);
                        value = temp;
                        bestMatchingUnit = map[i, j];
                    }
                    else
                    {
                        Console.WriteLine("Skipped");
                    }
                }
            }

        }

        public void FindKNearestNeighbor()
        {
            for (int i = 1; i <= newRadius; i++)
            {
                for (int j = 1; j <= newRadius; j++)
                {
                    if (bestMatchingUnit != null)
                    {
                        //Up
                        if (bestMatchingUnit.xPos - i >= 0)
                        {
                            map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Down
                        if (bestMatchingUnit.xPos + i < row)
                        {
                            map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Left
                        if (bestMatchingUnit.yPos - j >= 0)
                        {
                            map[bestMatchingUnit.xPos, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Right
                        if (bestMatchingUnit.yPos + j < col)
                        {
                            map[bestMatchingUnit.xPos, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Bottom-Right
                        if (bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos + j < col)
                        {
                            map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Upper-Left
                        if (bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos - j >= 0)
                        {
                            map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Upper-Right
                        if (bestMatchingUnit.xPos - i >= 0 && bestMatchingUnit.yPos + j < col)
                        {
                            map[bestMatchingUnit.xPos - i, bestMatchingUnit.yPos + j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                        //Bottom-Left
                        if (bestMatchingUnit.xPos + i < row && bestMatchingUnit.yPos - j >= 0)
                        {
                            map[bestMatchingUnit.xPos + i, bestMatchingUnit.yPos - j].AdjustWeights(songs[songIndex], newLearningRate);
                        }
                    }

                }
            }

        }



        public void Train()
        {
            double value = -1;
            double temp;

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
            for (int i = 0; i < iterations; i++)
            {
                SetBestMatchingUnit();
                FindKNearestNeighbor();
                currentIteration++;
            }

            //Add songs to neurons
            for (int i = 0; i < songs.Count; i++)
            {
                value = -1;
                for (int j = 0; j < row; j++)
                {
                    for (int x = 0; x < col; x++)
                    {
                        temp = map[j, x].GetDistance(songs[i].Features);
                        if (value == -1)
                        {
                            value = temp;
                        }

                        if (temp <= value)
                        {
                            Debug.WriteLine("Final Temp: " + temp);
                            Debug.WriteLine("Final Value: " + value);
                            Debug.WriteLine("Adding song to best matching unit " + songs[i].Title + " " + songs[i].Genre);
                            value = temp;
                            //bestMatchIndexX = j;
                            //bestMatchIndexY = x;
                            //map[bestMatchIndexX, bestMatchIndexY].AddSongToList(songs[i]);
                            //songs[i].XPos = bestMatchIndexX;
                            //songs[i].YPos = bestMatchIndexY;
                            bestMatchingUnit = map[j, x];
                        }
                    }

                }
                songs[i].XPos = bestMatchingUnit.xPos;
                songs[i].YPos = bestMatchingUnit.yPos;
            }
            for (int i = 0; i < songs.Count; i++)
            {
                Debug.WriteLine(songs[i].Title + " Genre: " + songs[i].Genre);
                Debug.WriteLine(songs[i].XPos + ", " + songs[i].YPos);
            }

            //Writer.write(songs);
        }

        public void DecayRadius()
        {
            decimal value;
            value = radius * ((decimal)(iterations - currentIteration) / (decimal)iterations);
            newRadius = (int)Math.Ceiling(value);
            if (newRadius < 1)
            {
                newRadius = 1;
            }

            //Console.WriteLine("New neighborhood radius:" + newRadius);
        }

        public void DecayLearningRate()
        {
            newLearningRate = learningRate * ((double)(iterations - currentIteration) / (double)iterations);
            if (newLearningRate < 0.1)
            {
                newLearningRate = 0.1;
            }
            //Debug.WriteLine("Learning Rate: " + newLearningRate);
        }
    }
}
