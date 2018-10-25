using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace GalaxyZooTouchTable.Views
{
    /// <summary>
    /// Interaction logic for SpaceView.xaml
    /// </summary>
    public partial class SpaceView : UserControl
    {
        SolarSystem3D SolarSystem;

        public SpaceView()
        {
            InitializeComponent();

            view1.Camera.Position = new Point3D(0, -400, -500);
            view1.Camera.LookDirection = new Vector3D(0, 400, 500);
            SolarSystem = view1.Children[2] as SolarSystem3D;
            DataContext = SolarSystem;

            Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            SolarSystem.InitModel();
            SolarSystem.UpdateModel();
        }

    }
}
