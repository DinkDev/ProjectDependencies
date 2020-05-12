namespace ProjectDependencies.DataAccess
{
    using System.Diagnostics;

    [DebuggerDisplay("Title = {Title}, Version = {Version}")]
    public class ProjectAssemblyData
    {
        public string Company { get; set; }
        public string Configuration { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string FileVersion { get; set; }
        public string InformationalVersion { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string NeutralLanguage { get; set; }
    }
}
