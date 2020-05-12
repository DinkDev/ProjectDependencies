namespace ProjectDependencies.Model
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    [DebuggerDisplay("{ProjectName}, {RelativePath}, {ProjectGuid}")]
    public class ProjectInSolutionWrapper
    {
        private static readonly PropertyInfo _projectNameProperty;
        private static readonly PropertyInfo _relativePathProperty;
        private static readonly PropertyInfo _projectGuidProperty;
        private static readonly PropertyInfo _projectTypeProperty;

        static ProjectInSolutionWrapper()
        {
            var projectInSolutionType = Type.GetType(
                                            "Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.8.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                                            false, false)
                                        ?? throw new TypeLoadException(
                                            $"Unable to load type Microsoft.Build.Construction.ProjectInSolution");

            _projectNameProperty =
                projectInSolutionType.GetProperty("ProjectName", BindingFlags.NonPublic | BindingFlags.Instance);
            _relativePathProperty =
                projectInSolutionType.GetProperty("RelativePath", BindingFlags.NonPublic | BindingFlags.Instance);
            _projectGuidProperty =
                projectInSolutionType.GetProperty("ProjectGuid", BindingFlags.NonPublic | BindingFlags.Instance);
            _projectTypeProperty =
                projectInSolutionType.GetProperty("ProjectType", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public string ProjectName { get; }
        public string RelativePath { get; }
        public string ProjectGuid { get; }
        public string ProjectType { get; }

        public ProjectInSolutionWrapper(object solutionProject)
        {
            ProjectName = _projectNameProperty.GetValue(solutionProject, null) as string;
            RelativePath = _relativePathProperty.GetValue(solutionProject, null) as string;
            ProjectGuid = _projectGuidProperty.GetValue(solutionProject, null) as string;
            ProjectType = _projectTypeProperty.GetValue(solutionProject, null).ToString();
        }
    }
}
