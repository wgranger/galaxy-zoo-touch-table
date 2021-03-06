﻿using GalaxyZooTouchTable.Models;
using GalaxyZooTouchTable.ViewModels;
using Xunit;

namespace GalaxyZooTouchTable.Tests.ViewModels
{
    public class CloseConfirmationViewModelTests
    {
        CloseConfirmationViewModel _viewModel;

        public CloseConfirmationViewModelTests()
        {
            _viewModel = new CloseConfirmationViewModel(new BlueUser(), ClassificationPanelViewModelTests.MockClassificationPanel());
        }

        [Fact]
        private void ShouldInitializeWithDefaultValues()
        {
            Assert.False(_viewModel.Intent);
            Assert.False(_viewModel.IsVisible);
        }

        [Fact]
        private void ShouldCheckIntent()
        {
            Assert.False(_viewModel.Intent);
            _viewModel.CheckIntent.Execute(null);
            Assert.True(_viewModel.Intent);
        }

        [Fact]
        private void ShouldResetIntentWhenClassifierClosed()
        {
            _viewModel.Intent = true;
            _viewModel.CheckIntent.Execute(null);
            Assert.False(_viewModel.Intent);
        }

        [Fact]
        private void ShouldOpenConfirmationModal()
        {
            _viewModel.ToggleCloseConfirmation.Execute(null);
            Assert.True(_viewModel.IsVisible);
        }

        [Fact]
        private void ShouldCloseConfirmationModal()
        {
            _viewModel.ToggleCloseConfirmation.Execute(null);
            _viewModel.ToggleCloseConfirmation.Execute(false);
            Assert.False(_viewModel.IsVisible);
        }
    }
}
