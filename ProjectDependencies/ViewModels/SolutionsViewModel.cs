namespace ProjectDependencies.ViewModels
{
    using Caliburn.Micro;

    public class SolutionsViewModel : Conductor<IDependencyScreen>.Collection.OneActive, IDependencyScreen
    {
        public SolutionsViewModel()
        {
            DisplayName = @"Solutions View";
        }
    }
}