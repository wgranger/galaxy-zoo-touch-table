﻿using GalaxyZooTouchTable.Lib;
using GalaxyZooTouchTable.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;

namespace GalaxyZooTouchTable.ViewModels
{
    public class CenterpieceViewModel : ViewModelBase
    {
        public ObservableCollection<TableUser> AllUsers { get; set; } = new ObservableCollection<TableUser>();

        private bool _showJoinMessage = true;
        public bool ShowJoinMessage
        {
            get => _showJoinMessage;
            set => SetProperty(ref _showJoinMessage, value);
        }

        private bool _flipCenterpiece;
        public bool FlipCenterpiece
        {
            get => _flipCenterpiece;
            set => SetProperty(ref _flipCenterpiece, value);
        }

        public CenterpieceViewModel()
        {
            CreateTimer();

            AllUsers.CollectionChanged += AllUsersCollectionChanged;

            AllUsers.Add(GlobalData.GetInstance().PersonUser);
            AllUsers.Add(GlobalData.GetInstance().LightUser);
            AllUsers.Add(GlobalData.GetInstance().StarUser);
            AllUsers.Add(GlobalData.GetInstance().HeartUser);
            AllUsers.Add(GlobalData.GetInstance().FaceUser);
            AllUsers.Add(GlobalData.GetInstance().EarthUser);
        }

        private void AllUsersCollectionChanged(object sender, NotifyCollectionChangedEventArgs changedEventArgs)
        {

            if (changedEventArgs.NewItems != null)
            {
                foreach (object item in changedEventArgs.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (changedEventArgs.OldItems != null)
            {
                foreach (object item in changedEventArgs.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs changedEventArgs)
        {
            foreach (TableUser user in AllUsers)
            {
                if (user.Active)
                {
                    ShowJoinMessage = false;
                    return;
                }
            }
            ShowJoinMessage = true;
        }

        private void CreateTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new System.EventHandler(OnFlipCenterpiece);
            dispatcherTimer.Interval = new System.TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }

        private void OnFlipCenterpiece(object sender, System.EventArgs e)
        {
            FlipCenterpiece = !FlipCenterpiece;
        }
    }
}
