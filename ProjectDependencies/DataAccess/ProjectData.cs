namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {FileData.Name}")]
    public class ProjectData
    {
        public ProjectFileData FileData { get; set; }
        public ProjectAssemblyData AssemblyInfo { get; set; }
        public List<SolutionProjectReferenceData> SolutionProjectReferences { get; set; } = new List<SolutionProjectReferenceData>();
        public List<LibraryReferenceData> LibraryReferences { get; set; } = new List<LibraryReferenceData>();
    }
}