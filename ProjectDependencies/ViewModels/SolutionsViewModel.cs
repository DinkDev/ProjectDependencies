namespace ProjectDependencies.ViewModels
{
    using System;
    using Caliburn.Micro;
    using Model;

    public class SolutionsViewModel : Conductor<IDependencyScreen>.Collection.OneActive, IDependencyScreen
    {
        private readonly SolutionFileHelper _fileHelper;

        public SolutionsViewModel(SolutionFileHelper fileHelper)
        {
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));

            DisplayName = @"Solutions View";
        }

        public BindableCollection<SolutionFileViewModel> Solutions { get; } = new BindableCollection<SolutionFileViewModel>();

        public void SearchForProjects()
        {
            Solutions.Add(new SolutionFileViewModel { Path = "ABC", IsSelected = false });
            Solutions.Add(new SolutionFileViewModel { Path = "XYZ", IsSelected = false });
        }
    }
}