namespace ProjectDependencies.Model
{
    public interface IFileSettings
    {
        /// <summary>
        /// Usually ".csproj"
        /// </summary>
        string ProjectFileExtension { get; }

        /// <summary>
        /// Usually "*.sln"
        /// </summary>
        string SolutionSearchWildcard { get; }
    }
}