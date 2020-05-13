namespace ProjectDependencies.ViewModels
{
    using System;
    using Autofac;
    using Caliburn.Micro;

    public class ShellViewModel : Conductor<IDependencyScreen>, IShell
    {
        private readonly ILifetimeScope _container;

        public ShellViewModel(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void SelectSolutionsView()
        {
            ActivateItem(GetWindow<SolutionsViewModel>());
        }
        
        public override void ActivateItem(IDependencyScreen item)
        {
            base.ActivateItem(item);
            if (item != null)
            {
                SetDisplayName(item.DisplayName);
            }
        }

        private T GetWindow<T>() where T : IDependencyScreen
        {
            return _container.Resolve<T>();
        }

        private void SetDisplayName(string testDisplayName)
        {
            DisplayName = $"Project Dependencies - {testDisplayName}";
        }
    }
}