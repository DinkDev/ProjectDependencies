namespace ProjectDependenciesTests
{
    using ProjectDependencies.Model;

    public class TestParserSettings : IFileSettings
    {
        public string ProjectFileExtension { get; } = @".xml";
        public string SolutionSearchWildcard { get; } = @"*.sln";
    }
}