﻿using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GalaxyZooTouchTable.Models
{
    public enum GalaxyType
    {
        Smooth,
        Features,
        Star
    }

    public class GalaxyExample
    {
        public BitmapImage ImageOne { get; set; }
        public BitmapImage ImageTwo { get; set; }
        public BitmapImage ImageThree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }

        public GalaxyExample(GalaxyType type)
        {
            switch (type)
            {
                case GalaxyType.Smooth:
                    ImageOne = new BitmapImage(new Uri("../Images/GalaxyExamples/smooth1.jpg", UriKind.Relative));
                    ImageTwo = new BitmapImage(new Uri("../Images/GalaxyExamples/smooth2.jpg", UriKind.Relative));
                    ImageThree = new BitmapImage(new Uri("../Images/GalaxyExamples/smooth3.jpg", UriKind.Relative));
                    Title = "Smooth";
                    Description = "Smooth galaxies fade gradually in all directions from the center";
                    Description2 = "There may be a small bright symmetric core.";
                    break;
                case GalaxyType.Features:
                    ImageOne = new BitmapImage(new Uri("../Images/GalaxyExamples/features1.jpeg", UriKind.Relative));
                    ImageTwo = new BitmapImage(new Uri("../Images/GalaxyExamples/features2.jpg", UriKind.Relative));
                    ImageThree = new BitmapImage(new Uri("../Images/GalaxyExamples/features3.jpg", UriKind.Relative));
                    Title = "Features or Disk";
                    Description = "Galaxies might have spiral arms, a bulge or bar, or any other interesting feature.";
                    Description2 = "Choose this option if you see anything unique about the galaxy";
                    break;
                case GalaxyType.Star:
                    ImageOne = new BitmapImage(new Uri("../Images/GalaxyExamples/star1.jpg", UriKind.Relative));
                    ImageTwo = new BitmapImage(new Uri("../Images/GalaxyExamples/star2.jpg", UriKind.Relative));
                    ImageThree = new BitmapImage(new Uri("../Images/GalaxyExamples/smooth3.jpg", UriKind.Relative));
                    Title = "Star or Artifact";
                    Description = "Choose \"Star or Artifact\" if there is no central galaxy to classify or if the artifact is so badly displayed that you can't ignore it and classify the galaxy with reasonable confidence.";
                    Description2 = "The telescopes taking our data were designed to look at faint galaxies, which means that bright, compact objects like stars sometimes look a bit strange.";
                    break;
            }
        }
    }
}