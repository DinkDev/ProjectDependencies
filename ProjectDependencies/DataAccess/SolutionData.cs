namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}, Version = {VisualStudioVersion}")]
    public class SolutionData
    {
        public int SolutionDataId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string SolutionPath { get; set; } = string.Empty;
        public virtual VersionData VisualStudioVersion { get; set; } = new VersionData(0, 0, 0, 0);
        public virtual ICollection<ProjectFileData> ProjectFiles { get; set; } = new List<ProjectFileData>();
        public virtual ICollection<ProjectData> Projects { get; set; } = new List<ProjectData>();
    }
}
