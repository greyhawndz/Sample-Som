﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sample_Som
{
    class Song
    {
        public int SongID
        {
            get; set;
        }

        public String Title
        {
            get; set;
        }

        public String Artist
        {
            get; set;

        }

        public String Album
        {
            get; set;
        }

        public int TrackNumber
        {
            get; set;
        }

        public String Year
        {
            get; set;

        }

        public String Genre
        {
            get; set;
        }

        public String Path
        {
            get; set;
        }


        public String FeaturePath
        {
            get; set;
        }

        public List<double> Features
        {
            get; set;
        }

        public int SongIndex
        {
            get; set;
        }

        public int XPos
        {
            get; set;
        }

        public int YPos
        {
            get; set;
        }

        public double GetTotal()
        {

            double sum = 0;
            for (int i = 0; i < Features.Count; i++)
            {
                sum += Features[i];
            }

            return sum;
        }

        public void ExtractFeatures()
        {
            String line;
            StreamReader file = new StreamReader(FeaturePath);

            while ((line = file.ReadLine()) != null)
            {
                Features.Add(double.Parse(line));
            }

            file.Close();
        }

        public Song()
        {
            Features = new List<double>();
        }

    }
}
