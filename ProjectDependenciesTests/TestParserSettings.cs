namespace ProjectDependenciesTests
{
    using ProjectDependencies.Model;

    public class TestParserSettings : IParserSettings
    {
        public string ProjectFileExtension { get; } = @".xml";
    }
}