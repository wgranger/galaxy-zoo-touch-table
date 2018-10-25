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
            viewport.DefaultCamera.Position = new Point3D(0.5, 0.5, 0.5);
            viewport.DefaultCamera.LookDirection = new Vector3D(-0.5, -0.5, -0.5);
            viewport.DefaultCamera.UpDirection = new Vector3D(0, 0, 1);

            viewport.Children.Add(new GridLinesVisual3D());

            SphereVisual3D sphere = new SphereVisual3D();
            sphere.Radius = 2;
            sphere.Center = new Point3D(2, 0, 2);
            sphere.Fill = Brushes.Red;
            viewport.Children.Add(sphere);
        }
    }
}
