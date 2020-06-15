namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}, Version = {VisualStudioVersion}")]
    public class SolutionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SolutionDataId { get; set; } = int.MinValue;

        public string Name { get; set; } = string.Empty;
        public string SolutionPath { get; set; } = string.Empty;
        //public virtual VersionData VisualStudioVersion { get; set; } = new VersionData(0, 0, 0, 0);

        public virtual ICollection<ProjectFileData> ProjectFiles { get; set; } = new List<ProjectFileData>();
    }
}
