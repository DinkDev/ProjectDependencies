﻿namespace ProjectDependencies.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        [Key]
        public Guid? ProjectFileDataId { get; set; } = null;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ProjectPath { get; set; } = string.Empty;

        /// <remarks>
        /// This is to facilitate a many to many relationship from SolutionData
        /// </remarks>
        public virtual ICollection<SolutionData> Solutions { get; set; } = new List<SolutionData>();
    }
}