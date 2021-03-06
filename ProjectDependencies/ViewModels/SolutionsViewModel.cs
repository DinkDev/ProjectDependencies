﻿namespace ProjectDependencies.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using Caliburn.Micro;
    using DataAccess;
    using Model.SolutionAndProjectParsing;
    using Ookii.Dialogs.Wpf;
    using SolutionData = DataAccess.SolutionData;

    public sealed class SolutionsViewModel : Conductor<IDependencyScreen>.Collection.OneActive, IDependencyScreen
    {
        private readonly SolutionFileHelper _fileHelper;
        //private readonly Func<ProjectDependencyContext> _getDbContext;
        private readonly Func<VistaFolderBrowserDialog> _getFolderBrowser;
        private readonly BindableCollection<SolutionFileViewModel> _workingSolutions = new BindableCollection<SolutionFileViewModel>();

        //public SolutionsViewModel(SolutionFileHelper fileHelper, Func<ProjectDependencyContext> getDbContext, Func<VistaFolderBrowserDialog> getFolderBrowser)
        public SolutionsViewModel(SolutionFileHelper fileHelper, Func<VistaFolderBrowserDialog> getFolderBrowser)
        {
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
            //_getDbContext = getDbContext ?? throw new ArgumentNullException(nameof(getDbContext));
            _getFolderBrowser = getFolderBrowser ?? throw new ArgumentNullException(nameof(getFolderBrowser));

            DisplayName = @"Solutions View";

            //LoadSolutionsFromDb();
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
            dialog.Reset();
            dialog.Description = @"Please select a folder to search from.";
            dialog.UseDescriptionForTitle = true;
            //dialog.SelectedPath = @"C:\";
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (dialog.ShowDialog(Parent as Window) ?? false)
            {
                var searchFolder = dialog.SelectedPath;

                await _fileHelper.SearchForSolutionsAsync(searchFolder)
                    .ContinueWith(MergeSolutions)
                    .ConfigureAwait(false);
            }
        }

        public async void SyncSelectedProjects()
        {
            var addedSolutionFiles = _workingSolutions.Where(s => s.IsNew).Select(s => s.SolutionPath);
            var deletedSolutions = _workingSolutions.Where(s => s.IsDeleted).Select(s => s.Wrapped);

            //var context = _getDbContext();

            // 1 - Delete the deleted solutions
            try
            {
                //foreach (var solutionData in deletedSolutions.Select(d =>
                //        context.Solutions.FirstOrDefault(s => s.SolutionDataId == d.SolutionDataId))
                //    .Where(s => s != null))
                //{
                //    context.Solutions.Remove(solutionData);
                //}

                //context.Solutions.RemoveRange(deletedSolutions);
            }
            finally
            {
                // TODO: report the number of edits returned 
                //await context.SaveChangesAsync();
            }

            // 2 - read in the new solutions (and Project files)
            var newSolutions = await Task.Run(() =>
            {
                var rv = new List<SolutionFileData>();

                foreach (var newSolutionFile in addedSolutionFiles)
                {
                    rv.Add(_fileHelper.ReadSolutionFileAsync(newSolutionFile).Result);
                }

                return rv;
            }).ConfigureAwait(false);

            // Now - look at ProjectFileData

            //// 3 - get the current list of all ProjectFileData items, any any new - delete any discarded
            //var currentProjectFiles = context.ProjectFiles.ToDictionary(k => k.ProjectFileDataId, v => v);
            //var newProjectFiles = newSolutions.SelectMany(s => s.ProjectFiles).ToList();

            //foreach (var newProjectFile in newProjectFiles)
            //{

            //}

            ////LoadSolutionsFromDb();
        }

        private void MergeSolutions(Task<Tuple<string, uint>[]> solutionTask)
        {
            var solutions = solutionTask.Result;

            Execute.OnUIThread(() =>
            {
                var currentSolutions = _workingSolutions.Select(s => s.SolutionPath).ToArray();
                var newSolutions = solutions.Where(s => !currentSolutions.Contains(s.Item1));

                _workingSolutions.AddRange(newSolutions.Select(n => new SolutionFileViewModel(n.Item1, n.Item2)));

                Solutions = CollectionViewSource.GetDefaultView(_workingSolutions);
                Solutions.SortDescriptions.Add(new SortDescription("Path", ListSortDirection.Ascending));
            });
        }

        //private void LoadSolutionsFromDb()
        //{
        //    // TODO: async this, also need to add a UI wait notification
        //    foreach (var solution in _getDbContext().Solutions)
        //    {
        //        _workingSolutions.Add(new SolutionFileViewModel(solution));
        //    }

        //    Solutions = CollectionViewSource.GetDefaultView(_workingSolutions);
        //    Solutions.SortDescriptions.Add(new SortDescription("Path", ListSortDirection.Ascending));
        //}
    }
}