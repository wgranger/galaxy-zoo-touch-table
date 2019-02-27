﻿using System;
using System.Collections.ObjectModel;
using GalaxyZooTouchTable.ViewModels;
using PanoptesNetClient.Models;

namespace GalaxyZooTouchTable.Models
{
    public class TableSubject : ViewModelBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double RightAscension { get; set; }
        public double Declination { get; set; }
        public string SubjectLocation { get; set; }
        private readonly double PlateScale = 1.5;
        private readonly double Offset = 0.03;
        public Subject Subject { get; set; }
        public ObservableCollection<GalaxyRing> Submissions { get; set; } = new ObservableCollection<GalaxyRing>();

        private int _tileOffset = -28;
        public int TileOffset
        {
            get => _tileOffset;
            set => SetProperty(ref _tileOffset, value);
        }

        public TableSubject(Subject subject, double TableRA = 0, double TableDEC = 0)
        {
            RightAscension = System.Convert.ToDouble(subject.Metadata.ra);
            Declination = System.Convert.ToDouble(subject.Metadata.dec);
            Subject = subject;
            SubjectLocation = subject.GetSubjectLocation();
            XYConvert(TableRA, TableDEC);

            Submissions.Add(new GalaxyRing());
        }

        private void XYConvert(double CenterRightAscension, double CenterDeclination)
        {
            int CutoutWidth = 1248; 
            int CutoutHeight = 432;
            const int ArcDegreeInSeconds = 3600;

            double DecRange = CutoutHeight * PlateScale / ArcDegreeInSeconds;
            double RaRange = System.Math.Abs(CutoutWidth * PlateScale / ArcDegreeInSeconds / System.Math.Cos(CenterDeclination));

            double StartY = ((CenterDeclination - Declination) / DecRange * CutoutHeight) + (CutoutHeight / 2);
            double StartX = ((CenterRightAscension - RightAscension) / (RaRange + Offset) * CutoutWidth) + (CutoutWidth / 2);

            Y = System.Convert.ToInt32(StartY);
            X = System.Convert.ToInt32(StartX);
        }

        public void DimRing(TableUser userClassifying)
        {
            foreach (GalaxyRing Ring in Submissions)
            {
                if (Ring.User == userClassifying)
                {
                    Ring.CurrentlyClassifying = false;
                }
            }
        }

        public void AddRing(TableUser user)
        {
            int NewIndex = Submissions.Count;
            GalaxyRing NewRing = new GalaxyRing(NewIndex, user);
            if (NewIndex > 0)
            {
                TileOffset -= 8;
            }
            Submissions.Add(NewRing);
        }

        private double ToRadians(double Degrees)
        {
            return (Degrees * System.Math.PI) / 180.0;
        }
    }
}
