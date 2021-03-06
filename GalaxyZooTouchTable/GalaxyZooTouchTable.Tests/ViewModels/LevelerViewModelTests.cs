﻿using GalaxyZooTouchTable.Lib;
using GalaxyZooTouchTable.ViewModels;
using Xunit;

namespace GalaxyZooTouchTable.Tests.ViewModels
{
    public class LevelerViewModelTests
    {
        private LevelerViewModel _viewModel;

        public LevelerViewModelTests()
        {
            var User = GlobalData.GetInstance().BlueUser;
            _viewModel = new LevelerViewModel(User, ClassificationPanelViewModelTests.MockClassificationPanel());
        }

        [Fact]
        public void ShouldInitializeWithDefaultValues()
        {
            var ExpectedUser = GlobalData.GetInstance().BlueUser;
            Assert.Equal(5, _viewModel.ClassificationsUntilUpgrade);
            Assert.Equal(0, _viewModel.ClassificationsThisSession);
            Assert.Equal(ExpectedUser, _viewModel.User);
            Assert.Equal("One", _viewModel.ClassificationLevel);
            Assert.False(_viewModel.IsOpen);
        }

        [Fact]
        public void ShouldIncrementCount()
        {
            _viewModel.OnIncrementCount();
            Assert.Equal(1, _viewModel.ClassificationsThisSession);
        }

        [Fact]
        public void ShouldCloseLeveler()
        {
            _viewModel.IsOpen = true;
            _viewModel.CloseLeveler();
            Assert.False(_viewModel.IsOpen);
        }

        [Fact]
        public void ShouldToggleLeveler()
        {
            Assert.False(_viewModel.IsOpen);
            _viewModel.ToggleLeveler.Execute(null);
            Assert.True(_viewModel.IsOpen);
        }

        [Fact]
        public void ShouldLevelUpAccordingly()
        {
            Assert.Equal("One", _viewModel.ClassificationLevel);

            _viewModel.ClassificationsThisSession = 5;
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.Equal("Two", _viewModel.ClassificationLevel);

            _viewModel.ClassificationsThisSession = 10;
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.Equal("Three", _viewModel.ClassificationLevel);

            _viewModel.ClassificationsThisSession = 15;
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.Equal("Four", _viewModel.ClassificationLevel);

            _viewModel.ClassificationsThisSession = 20;
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.Equal("Five", _viewModel.ClassificationLevel);

            _viewModel.ClassificationsThisSession = 25;
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.Equal("Master", _viewModel.ClassificationLevel);
        }

        [Fact]
        public void ShouldAnimateLevelUp()
        {
            bool LevelUpAnimationFired = false;
            _viewModel.LevelUpAnimation += delegate { LevelUpAnimationFired = true; };
            _viewModel.ClassificationsUntilUpgrade = 0;
            Assert.True(_viewModel.IsOpen);
            Assert.True(LevelUpAnimationFired);
        }

        [Fact]
        public void ShouldResetClassificationCounts()
        {
            _viewModel.OnIncrementCount();
            Assert.Equal(1, _viewModel.ClassificationsThisSession);
            _viewModel.Reset();
            Assert.Equal(0, _viewModel.ClassificationsThisSession);
        }
    }
}
