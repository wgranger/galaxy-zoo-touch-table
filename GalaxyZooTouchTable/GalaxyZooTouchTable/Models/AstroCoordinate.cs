using System;

namespace GalaxyZooTouchTable.Models
{
    public class AstroCoordinate
    {
        public double RA { get; set; }
        public double DEC { get; set; }
        public double Redshift { get; set; }
        public Declination Declination { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public AstroCoordinate(double ra, double dec, double redshift)
        {
            //RA = ra;
            //DEC = dec;
            RA = 26.017046;
            DEC = -15.937469;
            Redshift = 11.9;

            ConvertDeclination(dec);
            FindCartesianCoordinates();
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
            X = Redshift * Math.Cos(ToDegrees(DEC)) * Math.Cos(ToDegrees(RA));
            Y = Redshift * Math.Cos(ToDegrees(DEC)) * Math.Sin(ToDegrees(RA));
            Z = Redshift * Math.Sin(ToDegrees(DEC));
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
