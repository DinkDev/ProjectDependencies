namespace ProjectDependencies.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {SolutionName}, Version = {VisualStudioVersion}")]
    public class SolutionData
    {
        public string SolutionName { get; set; } = string.Empty;
        public string SolutionFullPath { get; set; } = string.Empty;
        public Version VisualStudioVersion { get; set; } = new Version(0, 0, 0, 0);
        public List<ProjectFileData> ProjectFiles { get; set; } = new List<ProjectFileData>();
        public List<ProjectData> Projects { get; set; } = new List<ProjectData>();
    }
}
