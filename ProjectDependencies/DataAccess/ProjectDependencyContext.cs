namespace ProjectDependencies.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class ProjectDependencyContext : DbContext
    {
        //public ProjectDependencyContext(IDataAccessSettings settings) : base(settings.DataAccessConnection)
        //{
        //}

        //public DbSet<SolutionData> Solutions { get; set; }
        //public DbSet<ProjectData> Projects { get; set; }
        //public DbSet<ProjectFileData> ProjectFiles { get; set; }
        //public DbSet<SolutionProjectReferenceData> SolutionProjectReferences { get; set; }
        //public DbSet<ProjectLibraryReferenceData> ProjectLibraryReferences { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        //    modelBuilder.ComplexType<VersionData>();

        //    modelBuilder.Entity<SolutionData>()
        //        .HasMany<ProjectFileData>(s => s.ProjectFiles)
        //        .WithMany(p => p.Solutions)
        //        .Map(cs =>
        //        {
        //            cs.MapLeftKey(nameof(SolutionData.SolutionDataId));
        //            cs.MapRightKey(nameof(ProjectFileData.ProjectFileDataId));
        //            cs.ToTable(@"SolutionDataProjectFileData");
        //        });

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
