namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {FileData.Name}")]
    public class ProjectData
    {
        public int ProjectDataId { get; set; }

        public int FileDataId { get; set; }

        [ForeignKey(nameof(FileDataId))]
        public virtual ProjectFileData FileData { get; set; }

        public virtual ICollection<SolutionProjectReferenceData> SolutionProjectReferences { get; set; } = new List<SolutionProjectReferenceData>();
        public virtual ICollection<ProjectLibraryReferenceData> LibraryReferences { get; set; } = new List<ProjectLibraryReferenceData>();
    }
}