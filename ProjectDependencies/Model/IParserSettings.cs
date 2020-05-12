namespace ProjectDependencies.Model
{
    public interface IParserSettings
    {
        /// <summary>
        /// Usually ".csproj"
        /// </summary>
        string ProjectFileExtension { get; }
    }
}