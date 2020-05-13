namespace ProjectDependencies.DataAccess
{
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}")]
    public class SolutionProjectReferenceData
    {
        public int SolutionProjectReferenceDataId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string SolutionName { get; set; } = string.Empty;
        public string ProjectPath { get; set; } = string.Empty;
    }
}