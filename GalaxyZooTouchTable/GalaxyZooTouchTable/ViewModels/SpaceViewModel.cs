using GalaxyZooTouchTable.Lib;
using GalaxyZooTouchTable.Models;
using GalaxyZooTouchTable.Services;
using GalaxyZooTouchTable.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GalaxyZooTouchTable.ViewModels
{
    public class SpaceViewModel : ViewModelBase
    {
        private ILocalDBService _localDBService;
        public SpaceNavigation CurrentLocation { get; set; }
        public event Action<CardinalDirectionEnum> AnimateMovement = delegate { };
        CutoutService CutoutService = new CutoutService();

        public ICommand MoveViewNorth { get; private set; }
        public ICommand MoveViewEast { get; set; }
        public ICommand MoveViewSouth { get; private set; }
        public ICommand MoveViewWest { get; private set; }
        public ICommand HideError { get; private set; }

        private bool _showError = false;
        public bool ShowError
        {
            get => _showError;
            set => SetProperty(ref _showError, value);
        }

        private bool _canMoveMap = false;
        public bool CanMoveMap
        {
            get => _canMoveMap;
            set => SetProperty(ref _canMoveMap, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private List<TableSubject> _currentGalaxies = new List<TableSubject>();
        public List<TableSubject> CurrentGalaxies
        {
            get => _currentGalaxies;
            set => SetProperty(ref _currentGalaxies, value);
        }

        private BitmapImage _previousSpaceCutoutUrl;
        public BitmapImage PreviousSpaceCutoutUrl
        {
            get => _previousSpaceCutoutUrl;
            set => SetProperty(ref _previousSpaceCutoutUrl, value);
        }

        private BitmapImage _spaceCutoutUrl;
        public BitmapImage SpaceCutoutUrl
        {
            get => _spaceCutoutUrl;
            set
            {
                PreviousSpaceCutoutUrl = _spaceCutoutUrl;
                SetProperty(ref _spaceCutoutUrl, value);
            }
        }

        private PeripheralItems PeripheralItems { get; set; } = new PeripheralItems();

        public SpaceViewModel(ILocalDBService localDBService)
        {
            _localDBService = localDBService;

            CurrentLocation = new SpaceNavigation(_localDBService.GetRandomPoint());
            CurrentGalaxies = _localDBService.GetLocalSubjects(CurrentLocation);
            SetSpaceCutout();
            SetPeripheralItems();
            LoadCommands();
            Messenger.Default.Register<ClassificationRingNotifier>(this, OnGalaxyInteraction);
            Messenger.Default.Register<string>(this, OnShowError, "DatabaseError");
        }

        private void OnGalaxyInteraction(ClassificationRingNotifier RingNotifier)
        {
            foreach (TableSubject SpaceViewGalaxy in CurrentGalaxies)
            {
                if (RingNotifier.Status == RingNotifierStatus.IsLeaving)
                {
                    SpaceViewGalaxy.RemoveRing(RingNotifier.User);
                } else if (RingNotifier.SubjectId == SpaceViewGalaxy.Id)
                {
                    switch (RingNotifier.Status)
                    {
                        case RingNotifierStatus.IsCreating:
                            RemoveAllActiveRingsByUser(RingNotifier.User);
                            SpaceViewGalaxy.AddRing(RingNotifier.User);
                            break;
                        case RingNotifierStatus.IsSubmitting:
                            SpaceViewGalaxy.DimRing(RingNotifier.User);
                            break;
                        default:
                            SpaceViewGalaxy.RemoveRing(RingNotifier.User);
                            break;
                    }
                }
            }
        }

        void RemoveAllActiveRingsByUser(TableUser user)
        {
            TableSubject Galaxy = CurrentGalaxies.Find(x => x.IsWorkingWithUser(user));
            if (Galaxy != null) Galaxy.RemoveRing(user);
        }

        private void LoadCommands()
        {
            MoveViewNorth = new CustomCommand(OnMoveViewNorth, OnCanMoveMap);
            MoveViewEast = new CustomCommand(OnMoveViewEast, OnCanMoveMap);
            MoveViewSouth = new CustomCommand(OnMoveViewSouth, OnCanMoveMap);
            MoveViewWest = new CustomCommand(OnMoveViewWest, OnCanMoveMap);
            HideError = new CustomCommand(OnHideError);
        }

        private bool OnCanMoveMap(object obj) { return CanMoveMap; }

        private void OnHideError(object obj) { ShowError = false; }

        private void OnShowError(string message)
        {
            ShowError = true;
            ErrorMessage = message;
        }

        private void OnMoveViewWest(object obj)
        {
            SpaceCutoutUrl = PeripheralItems.Western.Cutout;
            CurrentGalaxies = PeripheralItems.Western.Galaxies;
            if (CurrentGalaxies.Count > 0) AnimateMovement(CardinalDirectionEnum.West);
            CurrentLocation = PeripheralItems.Western.Location;
            SetPeripheralItems();
        }

        private void OnMoveViewSouth(object obj)
        {
            SpaceCutoutUrl = PeripheralItems.Southern.Cutout;
            CurrentGalaxies = PeripheralItems.Southern.Galaxies;
            if (CurrentGalaxies.Count > 0) AnimateMovement(CardinalDirectionEnum.South);
            CurrentLocation = PeripheralItems.Southern.Location;
            SetPeripheralItems();
        }

        private void OnMoveViewEast(object obj)
        {
            SpaceCutoutUrl = PeripheralItems.Eastern.Cutout;
            CurrentGalaxies = PeripheralItems.Eastern.Galaxies;
            if (CurrentGalaxies.Count > 0) AnimateMovement(CardinalDirectionEnum.East);
            CurrentLocation = PeripheralItems.Eastern.Location;
            SetPeripheralItems();
        }

        private void OnMoveViewNorth(object obj)
        {
            SpaceCutoutUrl = PeripheralItems.Northern.Cutout;
            CurrentGalaxies = PeripheralItems.Northern.Galaxies;
            if (CurrentGalaxies.Count > 0) AnimateMovement(CardinalDirectionEnum.North);
            CurrentLocation = PeripheralItems.Northern.Location;
            SetPeripheralItems();
        }

        private async void SetSpaceCutout()
        {
            SpaceCutoutUrl = await CutoutService.GetSpaceCutout(CurrentLocation);
        }

        private async void SetPeripheralItems()
        {
            CanMoveMap = false;
            SpaceNavigation northern = new SpaceNavigation(CurrentLocation.NextNorthernPoint());
            SpaceNavigation southern = new SpaceNavigation(CurrentLocation.NextSouthernPoint());
            SpaceNavigation eastern = new SpaceNavigation(CurrentLocation.NextEasternPoint());
            SpaceNavigation western = new SpaceNavigation(CurrentLocation.NextWesternPoint());

            PeripheralItems.Northern = await GetPeripheralItem(northern);
            PeripheralItems.Southern = await GetPeripheralItem(southern);
            PeripheralItems.Eastern = await GetPeripheralItem(eastern);
            PeripheralItems.Western = await GetPeripheralItem(western);
            CanMoveMap = true;
        }

        private async Task<PeripheralItem> GetPeripheralItem(SpaceNavigation location)
        {
            PeripheralItem periphery = new PeripheralItem(location);
            periphery.Galaxies = _localDBService.GetLocalSubjects(location);

            if (periphery.Galaxies.Count == 0)
            {
                periphery.Location = new SpaceNavigation(_localDBService.FindNextAscendingDec(location.MaxDec));
                periphery.Galaxies = _localDBService.GetLocalSubjects(periphery.Location);
            }
            periphery.Cutout = await CutoutService.GetSpaceCutout(periphery.Location);
            return periphery;
        }
    }
}
