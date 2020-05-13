namespace ProjectDependencies.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Name = {Name}, Version = {Version}")]
    public class ProjectLibraryReferenceData
    {
        public long ProjectLibraryReferenceDataId { get; set; }


        /// <summary>
        /// Assembly name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Assembly version number. Typically in: (Major).(Minor).(Patch) format. 
        /// </summary>
        public virtual VersionData Version { get; set; } = new VersionData(0, 0, 0, 0);

        /// <summary>
        /// Assembly culture.
        /// </summary>
        public string Culture { get; set; } = string.Empty;

        /// <summary>
        /// Assembly public key.
        /// </summary>
        public string PublicKeyToken { get; set; } = string.Empty;

        /// <summary>
        /// Assembly processor architecture. Typically one of "MSIL," "X86," or "AMD64".
        /// </summary>
        public string ProcessorArchitecture { get; set; } = string.Empty;

        /// <summary>
        /// Relative or absolute path of the assembly.
        /// </summary>
        public string HintPath { get; set; } = string.Empty;

        /// <summary>
        /// Any aliases for the reference.
        /// </summary>
        public virtual ICollection<string> Aliases { get; set; } = new List<string>();

        /// <summary>
        /// Specifies whether the reference should be copied to the output folder.
        /// </summary>
        /// <remarks>
        /// This attribute matches the Copy Local property of the reference that's in the Visual Studio IDE.
        /// </remarks>
        public bool Private { get; set; } = false;
    }
}
