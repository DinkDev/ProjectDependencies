namespace ProjectDependencies.ViewModels
{
    using Caliburn.Micro;
    using DataAccess;

    public class SolutionFileViewModel : PropertyChangedBase
    {
        private readonly string _solutionPath;
        private bool _isSelected;

        /// <summary>
        /// Load current object and set as selected
        /// </summary>
        /// <param name="wrapped"></param>
        public SolutionFileViewModel(SolutionData wrapped)
        {
            Wrapped = wrapped;
            IsSelected = true;
        }

        /// <summary>
        /// Load new solution file name (not selected)
        /// </summary>
        /// <param name="solutionPath"></param>
        public SolutionFileViewModel(string solutionPath)
        {
            _solutionPath = solutionPath;
        }

        public string SolutionPath => _solutionPath ?? Wrapped?.SolutionPath ?? @"Error";

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyOfPropertyChange(() => IsSelected);
                    NotifyOfPropertyChange(() => IsNew);
                    NotifyOfPropertyChange(() => IsDeleted);
                }
            }
        }
        
        public SolutionData Wrapped { get; }

        public bool IsNew => _solutionPath != null && IsSelected;
        public bool IsDeleted => Wrapped != null && !IsSelected;
    }
}