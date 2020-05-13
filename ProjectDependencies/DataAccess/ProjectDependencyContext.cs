namespace ProjectDependencies.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class ProjectDependencyContext : DbContext
    {
        public ProjectDependencyContext() : base(nameof(ProjectDependencyContext))
        {
        }

        public DbSet<SolutionData> Solutions { get; set; }
        public DbSet<ProjectData> Projects { get; set; }
        public DbSet<ProjectFileData> ProjectFiles { get; set; }
        public DbSet<SolutionProjectReferenceData> SolutionProjectReferences { get; set; }
        public DbSet<ProjectLibraryReferenceData> ProjectLibraryReferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.ComplexType<VersionData>();
        }
    }
}
