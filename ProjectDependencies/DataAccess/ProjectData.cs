namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {FileData.Name}")]
    public class ProjectData
    {
        public ProjectFileData FileData { get; set; }
        public ProjectAssemblyData AssemblyInfo { get; set; }
        public List<string> ProjectReferencePaths { get; set; } = new List<string>();
        public List<ReferenceData> ReferencedLibraries { get; set; } = new List<ReferenceData>();
    }
}