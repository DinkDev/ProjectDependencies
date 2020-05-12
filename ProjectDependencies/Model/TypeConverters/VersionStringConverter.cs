namespace ProjectDependencies.Model.TypeConverters
{
    using System;
    using AutoMapper;

    public class VersionStringConverter : ITypeConverter<string, Version>
    {
       public Version Convert(string source, Version destination, ResolutionContext context)
       {
           return Version.TryParse(source, out var rv) ? rv : new Version();
       }
    }
}