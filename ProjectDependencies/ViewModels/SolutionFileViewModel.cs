namespace ProjectDependencies.ViewModels
{
    using System.ComponentModel;
    using Caliburn.Micro;

    public class SolutionFileViewModel : PropertyChangedBase
    {
        private string _path;
        private bool _isSelected;

        public string Path
        {
            get => _path;
            set
            {
                if (value != _path)
                {
                    _path = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyOfPropertyChange(() => IsSelected);
                }
            }
        }
    }
}