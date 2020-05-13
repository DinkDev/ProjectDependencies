namespace ProjectDependencies.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using Caliburn.Micro;
    using Model;
    using Ookii.Dialogs.Wpf;

    public class SolutionsViewModel : Conductor<IDependencyScreen>.Collection.OneActive, IDependencyScreen
    {
        private readonly SolutionFileHelper _fileHelper;
        private readonly Func<VistaFolderBrowserDialog> _getFolderBrowser;
        private readonly BindableCollection<SolutionFileViewModel> _workingSolutions = new BindableCollection<SolutionFileViewModel>();

        public SolutionsViewModel(SolutionFileHelper fileHelper, Func<VistaFolderBrowserDialog> getFolderBrowser)
        {
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
            _getFolderBrowser = getFolderBrowser ?? throw new ArgumentNullException(nameof(getFolderBrowser));

            DisplayName = @"Solutions View";
            Solutions = CollectionViewSource.GetDefaultView(new List<SolutionFileViewModel>());
        }

        private ICollectionView _solutions;

        public ICollectionView Solutions
        {
            get => _solutions;
            set
            {
                if (value != _solutions)
                {
                    _solutions = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public async void SearchForProjects()
        {
            var dialog = _getFolderBrowser();
            dialog.Description = @"Please select a folder to search from.";
            dialog.UseDescriptionForTitle = true;
            dialog.SelectedPath = @"C:\";

            if (dialog.ShowDialog(this.Parent as Window) ?? false)
            {
                var searchFolder = dialog.SelectedPath;

                await _fileHelper.SearchForSolutionsAsync(searchFolder)
                    .ContinueWith(MergeSolutions)
                    .ConfigureAwait(false);
            }
        }

        private void MergeSolutions(Task<string[]> solutionTask)
        {
            var solutions = solutionTask.Result;

            Execute.OnUIThread(() =>
            {
                var currentSolutions = _workingSolutions.Select(s => s.Path).ToArray();
                var newSolutions = solutions.Where(s => !currentSolutions.Contains(s));

                _workingSolutions.AddRange(newSolutions.Select(n => new SolutionFileViewModel {Path = n}));

                Solutions = CollectionViewSource.GetDefaultView(_workingSolutions);
                Solutions.SortDescriptions.Add(new SortDescription("Path", ListSortDirection.Ascending));
                Solutions.SortDescriptions.Add(new SortDescription("IsSelected", ListSortDirection.Descending));
            });
        }
    }
}