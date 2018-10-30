using GalaxyZooTouchTable.Models;
using HelixToolkit.Wpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GalaxyZooTouchTable
{
    /// <summary>
    /// Interaction logic for Centerpiece.xaml
    /// </summary>
    public partial class Centerpiece : UserControl
    {
        public SphereVisual3D Sphere { get; set; }
        private Dictionary<SphereVisual3D, string> Models = new Dictionary<SphereVisual3D, string>();
        private List<Model3D> hitResultsList = new List<Model3D>();

        public Centerpiece()
        {
            InitializeComponent();

            viewport.DefaultCamera = new PerspectiveCamera();
            viewport.DefaultCamera.Position = new Point3D(0, 0, 0);
            viewport.CameraMode = CameraMode.FixedPosition;

            viewport.Children.Add(new GridLinesVisual3D());

            var test = new AstroCoordinate(200.00726, 3.084285, 0.1469969);

            Sphere = new SphereVisual3D();
            Sphere.Radius = 0.25;
            Sphere.Center = new Point3D(test.X, test.Y, test.Z);
            Sphere.Fill = Brushes.Gray;
            viewport.Children.Add(Sphere);

            Models.Add(Sphere, "FirstSphere");
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            Point mouse_pos = e.GetTouchPoint(sender as IInputElement).Position;
            HitTestResult result = VisualTreeHelper.HitTest(viewport, mouse_pos);
            RayMeshGeometry3DHitTestResult mesh_result = result as RayMeshGeometry3DHitTestResult;

            if (mesh_result != null)
            {
                var TouchedItem = mesh_result.VisualHit;
                bool AppropriateType = TouchedItem.GetType() == typeof(SphereVisual3D);

                if (AppropriateType)
                {
                    var ItemTouched = TouchedItem as SphereVisual3D;
                    ItemTouched.Fill = Brushes.Green;
                }
            }
        }
    }
}
