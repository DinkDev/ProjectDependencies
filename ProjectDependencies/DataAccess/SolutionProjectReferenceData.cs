namespace ProjectDependencies.DataAccess
{
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}")]
    public class SolutionProjectReferenceData
    {
        public string Name { get; set; } = string.Empty;
        public string SolutionName { get; set; } = string.Empty;

        // TODO: fix this up with the solutions path
        public string ProjectPath { get; set; } = string.Empty;
    }
}