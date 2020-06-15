namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {" + nameof(Name) + "}")]
    public class ProjectFileData
    {
        /// <summary>
        /// This is the GUID found in the solution Project records.
        /// </summary>
        /// <remarks>
        /// This defaults to a generated GUID, to be overwritten by the projects.
        /// </remarks>
        public Guid? ProjectFileDataId { get; set; } = null;

        public string Name { get; set; } = string.Empty;

        public string ProjectPath { get; set; } = string.Empty;
        public virtual ICollection<SolutionProjectReferenceData> SolutionProjectReferences { get; set; } = new List<SolutionProjectReferenceData>();
        public virtual List<ProjectLibraryReferenceData> LibraryReferences { get; set; } = new List<ProjectLibraryReferenceData>();

    }
}