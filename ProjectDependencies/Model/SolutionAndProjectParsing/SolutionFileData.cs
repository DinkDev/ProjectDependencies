namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}, Version = {VisualStudioVersion}")]
    public class SolutionFileData
    {
        public string Name { get; set; } = string.Empty;
        public string SolutionPath { get; set; } = string.Empty;
        public virtual VersionData VisualStudioVersion { get; set; } = new VersionData(0, 0, 0, 0);

        public virtual List<ProjectFileData> ProjectFiles { get; set; } = new List<ProjectFileData>();
    }
}