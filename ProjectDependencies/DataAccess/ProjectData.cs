namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {FileData.Name}")]
    public class ProjectData
    {
        public long ProjectDataId { get; set; }

        public virtual ProjectFileData FileData { get; set; }
        public virtual ICollection<SolutionProjectReferenceData> SolutionProjectReferences { get; set; } = new List<SolutionProjectReferenceData>();
        public virtual ICollection<ProjectLibraryReferenceData> LibraryReferences { get; set; } = new List<ProjectLibraryReferenceData>();
    }
}