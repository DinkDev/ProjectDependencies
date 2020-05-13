namespace ProjectDependencies.DataAccess
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}")]
    public class ProjectFileData
    {
        public long ProjectFileDataId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ProjectPath { get; set; } = string.Empty;
        public Guid Guid { get; set; } = Guid.Empty;
    }
}