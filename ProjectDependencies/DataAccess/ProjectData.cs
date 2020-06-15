namespace ProjectDependencies.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {FileData.Name}")]
    public class ProjectData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectDataId { get; set; }

        public Guid? FileDataId { get; set; }

        [ForeignKey(nameof(FileDataId))]
        public virtual ProjectFileData FileData { get; set; }

        public virtual ICollection<SolutionProjectReferenceData> SolutionProjectReferences { get; set; } = new List<SolutionProjectReferenceData>();
        public virtual ICollection<ProjectLibraryReferenceData> LibraryReferences { get; set; } = new List<ProjectLibraryReferenceData>();
    }
}