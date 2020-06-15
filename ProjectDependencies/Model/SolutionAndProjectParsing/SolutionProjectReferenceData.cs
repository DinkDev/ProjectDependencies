namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}")]
    public class SolutionProjectReferenceData
    {
        public string Name { get; set; } = string.Empty;
        public string SolutionName { get; set; } = string.Empty;
        public string ProjectPath { get; set; } = string.Empty;
    }
}