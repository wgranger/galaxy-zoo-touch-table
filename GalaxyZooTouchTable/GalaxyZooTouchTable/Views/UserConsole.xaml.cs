using GalaxyZooTouchTable.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GalaxyZooTouchTable
{
    /// <summary>
    /// Interaction logic for UserConsole.xaml
    /// </summary>
    public partial class UserConsole : UserControl
    {
        ClassificationPanelViewModel ViewModel { get; set; }

        public UserConsole()
        {
            InitializeComponent();
        }

        void UserConsole_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = DataContext as ClassificationPanelViewModel;
            ViewModel.EmitAuraAnimation += OnEmitAuraAnimation;
        }

        private void OnEmitAuraAnimation()
        {
            DoubleAnimation animateIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            DoubleAnimation animateOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            animateOut.BeginTime = TimeSpan.FromSeconds(1);

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTargetName(animateIn, Aura.Name);
            Storyboard.SetTargetName(animateOut, Aura.Name);

            Storyboard.SetTargetProperty(animateIn,
                new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(animateOut,
                new PropertyPath(OpacityProperty));

            storyboard.Children.Add(animateIn);
            storyboard.Children.Add(animateOut);

            storyboard.Begin(this);
        }
    }
}
