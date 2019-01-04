﻿using GalaxyZooTouchTable.Lib;
using GalaxyZooTouchTable.Models;
using GalaxyZooTouchTable.Utility;
using System.ComponentModel;
using System.Windows.Input;

namespace GalaxyZooTouchTable.ViewModels
{
    public class LevelerViewModel : INotifyPropertyChanged
    {
        public TableUser User { get; set; }
        public ICommand ToggleLeveler { get; private set; }
        const string MAX_LEVEL = "Five";

        private int _classificationsUntilUpgrade { get; set; } = 5;
        public int ClassificationsUntilUpgrade
        {
            get { return _classificationsUntilUpgrade; }
            set
            {
                if (value <= 0)
                {
                    value = 5;
                    LevelUp();
                }
                _classificationsUntilUpgrade = value;
                OnPropertyRaised("ClassificationsUntilUpgrade");
            }
        }

        private int _classificationsThisSession { get; set; } = 0;
        public int ClassificationsThisSession
        {
            get { return _classificationsThisSession; }
            set
            {
                ClassificationsUntilUpgrade--;
                _classificationsThisSession = value;
                OnPropertyRaised("ClassificationsThisSession");
            }
        }

        private string _classificationLevel { get; set; } = "One";
        public string ClassificationLevel
        {
            get { return _classificationLevel; }
            set
            {
                _classificationLevel = value;
                OnPropertyRaised("ClassificationLevel");
            }
        }

        private bool _isOpen = false;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyRaised("IsOpen");
            }
        }

        public LevelerViewModel(TableUser user = null)
        {
            User = user;
            LoadCommands();

            if (user != null)
            {
                Messenger.Default.Register<int>(this, OnClassificationReceived, $"{user.Name}_IncrementCount");
            }
        }

        private void OnClassificationReceived(int TotalClassifications)
        {
            ClassificationsThisSession = TotalClassifications;
        }

        public void LoadCommands()
        {
            ToggleLeveler = new CustomCommand(SlideLeveler);
        }

        private void SlideLeveler(object sender)
        {
            IsOpen = !IsOpen;
        }

        private void LevelUp()
        {
            if (ClassificationLevel == MAX_LEVEL)
            {
                return;
            }
            switch (ClassificationsThisSession)
            {
                case int n when (n <= 6):
                    ClassificationLevel = "Two";
                    break;
                case int n when (n <= 12):
                    ClassificationLevel = "Three";
                    break;
                case int n when (n <= 18):
                    ClassificationLevel = "Four";
                    break;
                case int n when (n <= 24):
                    ClassificationLevel = MAX_LEVEL;
                    break;
                default:
                    ClassificationLevel = "One";
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
