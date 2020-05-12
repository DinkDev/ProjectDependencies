namespace ProjectDependencies.Model
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AutoMapper;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using DataAccess;
    using TypeConverters;

    public class SolutionParser
    {
        private readonly Func<string, DotNetSolution> _solutionReader;
        private readonly Func<XDocument, DotNetProject> _projectReader;
        private readonly MapperConfiguration _mapperConfig;

        public SolutionParser(
            Func<string, DotNetSolution> solutionReader,
            Func<XDocument, DotNetProject> projectReader)
        {
            _solutionReader = solutionReader ?? throw new ArgumentNullException(nameof(solutionReader));
            _projectReader = projectReader ?? throw new ArgumentNullException(nameof(projectReader));
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing<NullStringConverter>();
                cfg.CreateMap<string, Version>().ConvertUsing<VersionStringConverter>();

                cfg.CreateMap<Tuple<string, DotNetSolution>, SolutionData>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.Item1)))
                    .ForMember(dest => dest.SolutionFullPath, opt => opt.MapFrom(src => Path.GetFullPath(src.Item1)))
                    .ForMember(dest => dest.VisualStudioVersion, opt => opt.MapFrom(src => Version.Parse(src.Item2.VisualStudioVersion)))
                    .ForMember(dest => dest.ProjectFiles, opt => opt.MapFrom(src => src.Item2.Projects.Where(p => p.Path.EndsWith(@".csproj", StringComparison.OrdinalIgnoreCase)).ToList()));

                cfg.CreateMap<DotNetSolutionProject, ProjectFileData>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
                    .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<Tuple<SolutionData, ProjectFileData, DotNetProject>, ProjectData>()
                    .ForMember(dest => dest.FileData, opt => opt.MapFrom(src => src.Item2))
                    .ForMember(dest => dest.AssemblyInfo, opt => opt.MapFrom(src => src.Item3.AssemblyInfo))
                    .ForMember(dest => dest.SolutionProjectReferences,
                        opt => opt.ResolveUsing(src =>
                        {
                            return src.Item3.ProjectReferences.Select(r => new SolutionProjectReferenceData
                            {
                                Name = Path.GetFileNameWithoutExtension(r.FilePath),
                                SolutionName = src.Item1.Name,
                                ProjectPath = Path.Combine(Path.GetFullPath(src.Item2.Path), r.FilePath)
                            });
                        }))
                    .ForMember(dest => dest.LibraryReferences,
                        opt => opt.MapFrom(src => src.Item3.References.Select(r => r).ToList()));

                cfg.CreateMap<AssemblyInfoProperties, ProjectAssemblyData>();

                cfg.CreateMap<Reference, LibraryReferenceData>();

            });
        }

        public async Task<SolutionData> ParseSolution(string solutionPath, Stream solutionStream)
        {
            using (var reader = new StreamReader(solutionStream))
            {
                var solutionText = await reader.ReadToEndAsync();
                var solutionObject = await Task.Run(() => _solutionReader(solutionText))
                    .ConfigureAwait(false);

                var mapper = _mapperConfig.CreateMapper();

                return mapper.Map<SolutionData>(Tuple.Create(solutionPath, solutionObject));
            }
        }

        public async Task<ProjectData> ParseProject(ProjectFileData projectFile, SolutionData solution, FileStream projectStream)
        {
            using (var reader = new StreamReader(projectStream))
            {
                var projectDocument = XDocument.Load(reader);

                var projectObject = await Task.Run(() => _projectReader(projectDocument))
                    .ConfigureAwait(false);

                var mapper = _mapperConfig.CreateMapper();

                return mapper.Map<ProjectData>(Tuple.Create(solution, projectFile, projectObject));
            }
        }
    }
}
