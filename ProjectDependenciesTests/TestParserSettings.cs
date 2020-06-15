namespace ProjectDependenciesTests
{
    using ProjectDependencies.Model.SolutionAndProjectParsing;

    public class TestParserSettings : IFileSettings
    {
        public string ProjectFileExtension { get; } = @".xml";
        public string SolutionSearchWildcard { get; } = @"*.sln";
    }
}