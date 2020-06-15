namespace ProjectDependencies.Model.TypeConverters
{
    using AutoMapper;
    using SolutionAndProjectParsing;

    public class VersionStringConverter : ITypeConverter<string, VersionData>
    {
       public VersionData Convert(string source, VersionData destination, ResolutionContext context)
       {
           return VersionData.TryParse(source, out var rv) ? rv : new VersionData();
       }
    }
}