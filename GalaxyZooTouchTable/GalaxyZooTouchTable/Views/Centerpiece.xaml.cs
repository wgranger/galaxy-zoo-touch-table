using GalaxyZooTouchTable.Models;
using HelixToolkit.Wpf;
using PanoptesNetClient;
using PanoptesNetClient.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public List<Subject> RawSubjects { get; set; } = new List<Subject>();
        public List<InteractiveSubject> VisibleSubjects { get; set; } = new List<InteractiveSubject>();

        public Centerpiece()
        {
            InitializeComponent();
            FetchSubjects();

            viewport.DefaultCamera = new PerspectiveCamera();
            viewport.DefaultCamera.Position = new Point3D(0, 0, 0);
            viewport.CameraMode = CameraMode.FixedPosition;

            viewport.Children.Add(new GridLinesVisual3D());
        }

        private void PlotItems()
        {
            foreach (var subject in VisibleSubjects)
            {
                Sphere = new SphereVisual3D();
                Sphere.Radius = 5;
                Sphere.Center = new Point3D(subject.X, subject.Y, subject.Z);
                Sphere.Fill = Brushes.Gray;
                viewport.Children.Add(Sphere);
            }
        }

        private async void FetchSubjects()
        {
            ApiClient client = new ApiClient();
            NameValueCollection query = new NameValueCollection
            {
                { "workflow_id", Config.WorkflowId },
                { "page_size", "10" }
            };
            RawSubjects = await client.Subjects.GetList("queued", query);
            GetCoordinates();
        }

        private void GetCoordinates()
        {
            if (RawSubjects.Count > 0)
            {
                foreach (Subject subject in RawSubjects)
                {
                    InteractiveSubject SubjectWithCoordinates = new InteractiveSubject(subject);
                    VisibleSubjects.Add(SubjectWithCoordinates);
                }
            }
            PlotItems();
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
