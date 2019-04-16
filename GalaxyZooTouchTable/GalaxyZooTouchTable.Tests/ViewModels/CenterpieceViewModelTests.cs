﻿using GalaxyZooTouchTable.Lib;
using GalaxyZooTouchTable.ViewModels;
using Xunit;

namespace GalaxyZooTouchTable.Tests.ViewModels
{
    public class CenterpieceViewModelTests
    {
        private CenterpieceViewModel _viewModel;

        public CenterpieceViewModelTests()
        {
            _viewModel = new CenterpieceViewModel();
        }

        [Fact]
        private void ShouldInstantiateWithDefaultValues()
        {
            Assert.True(_viewModel.AllUsers.Count > 0);
            Assert.True(_viewModel.Dormant);
            Assert.False(_viewModel.CenterpieceIsFlipped);
        }

        [Fact]
        private void ShouldHideJoinMessageWithActiveUser()
        {
            Assert.True(_viewModel.Dormant);
            GlobalData.GetInstance().StarUser.Active = true;
            Assert.False(_viewModel.Dormant);
        }
    }
}
