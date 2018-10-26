using GalaxyZooTouchTable.Models;
using HelixToolkit.Wpf;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GalaxyZooTouchTable
{
    /// <summary>
    /// Interaction logic for Centerpiece.xaml
    /// </summary>
    public partial class Centerpiece : UserControl
    {
        public Centerpiece()
        {
            InitializeComponent();
            //viewport.Camera.Position = new Point3D(0, 0, 0);
            //view1.Camera.LookDirection = new Vector3D(0, 0, 0);

            viewport.DefaultCamera = new PerspectiveCamera();
            viewport.DefaultCamera.Position = new Point3D(0, 0, 0);
            viewport.CameraMode = CameraMode.FixedPosition;
            //viewport.DefaultCamera.LookDirection = new Vector3D(-0.5, -0.5, -0.5);
            //viewport.DefaultCamera.UpDirection = new Vector3D(0, 0, 1);

            viewport.Children.Add(new GridLinesVisual3D());

            var test = new AstroCoordinate(200.00726, 3.084285, 0.1469969);

            SphereVisual3D sphere = new SphereVisual3D();
            sphere.Radius = 0.25;
            sphere.Center = new Point3D(test.X, test.Y, test.Z);
            sphere.Fill = Brushes.Gray;
            viewport.Children.Add(sphere);
        }
    }
}
