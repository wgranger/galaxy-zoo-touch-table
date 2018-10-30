using Newtonsoft.Json.Linq;
using PanoptesNetClient.Models;
using System;

namespace GalaxyZooTouchTable.Models
{
    public class InteractiveSubject
    {
        public Subject Subject { get; set; }
        public double RA { get; set; }
        public double DEC { get; set; }
        public double Redshift { get; set; }
        public Declination Declination { get; set; }
        public double MPC { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public InteractiveSubject(Subject subject)
        {
            //RA = ra;
            //DEC = dec;
            Subject = subject;
            //RA = 26.017046;
            //DEC = -15.937469;
            //Redshift = 11.9;

            FindMetadata(subject);
            //ConvertDeclination(dec);
            FindCartesianCoordinates();
        }

        private void FindMetadata(Subject subject)
        {
            RA = subject.Metadata["!ra"];
            DEC = subject.Metadata["!dec"];
            Redshift = subject.Metadata["!redshift"];
            MpcFromRedshift();
        }

        private void MpcFromRedshift()
        {
            var SpeedOfLight = 3 * Math.Pow(10, 5);
            var HubbleConstant = 65;
            var Velocity = (Redshift) * SpeedOfLight;
            MPC = Velocity / HubbleConstant;
        }

        public void ConvertDeclination(double dec)
        {
            bool IsNegative = dec < 0;
            dec = Math.Abs(dec);

            var degree = (int)Math.Round(dec);
            var decM = (int)Math.Abs(Math.Round((dec - degree) * 60));
            var decS = (int)Math.Round((Math.Abs((dec - degree) * 60) - decM) * 60);

            var test = (Math.Abs(degree) + (decM / 60) + (decS / 3600));
            Declination = new Declination(IsNegative, degree, decM, decS);
        }

        public double ToDegrees(double angle)
        {
            return angle * (Math.PI / 180.0);
        }

        public void FindCartesianCoordinates()
        {
            X = MPC * Math.Cos(ToDegrees(DEC)) * Math.Cos(ToDegrees(RA));
            Y = MPC * Math.Cos(ToDegrees(DEC)) * Math.Sin(ToDegrees(RA));
            Z = MPC * Math.Sin(ToDegrees(DEC));
        }
    }

    public class Declination
    {
        public int Degrees { get; set; }
        public int ArcMin { get; set; }
        public int ArcSec { get; set; }

        public Declination(bool IsNeg, int degrees, int arcmin, int arcsec)
        {
            Degrees = IsNeg ? degrees * -1 : degrees;
            ArcMin = arcmin;
            ArcSec = arcsec;
        }
    }
}
