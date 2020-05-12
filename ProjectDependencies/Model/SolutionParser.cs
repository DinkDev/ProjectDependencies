namespace ProjectDependencies.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AutoMapper;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using DataAccess;

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
                cfg.CreateMap<Tuple<string, DotNetSolution>, SolutionData>()
                    .ForMember(dest => dest.SolutionName, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.Item1)))
                    .ForMember(dest => dest.SolutionFullPath, opt => opt.MapFrom(src => Path.GetFullPath(src.Item1)))
                    .ForMember(dest => dest.VisualStudioVersion, opt => opt.MapFrom(src => Version.Parse(src.Item2.VisualStudioVersion)))
                    .ForMember(dest => dest.ProjectFiles, opt => opt.MapFrom(src => src.Item2.Projects.Where(p => p.Path.EndsWith(@".csproj", StringComparison.OrdinalIgnoreCase)).ToList()));

                cfg.CreateMap<DotNetSolutionProject, ProjectFileData>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
                    .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<Tuple<ProjectFileData, DotNetProject>, ProjectData>()
                    .ForMember(dest => dest.FileData, opt => opt.MapFrom(src => src.Item1))
                    .ForMember(dest => dest.AssemblyInfo, opt => opt.MapFrom(src => src.Item2.AssemblyInfo))
                    .ForMember(dest => dest.ProjectReferencePaths,
                        opt => opt.MapFrom(src => src.Item2.ProjectReferences.Select(r => r.FilePath).ToList()))

                    //;

                    .ForMember(dest => dest.ReferencedLibraries,
                        opt => opt.MapFrom(src => src.Item2.References.Select(r => r).ToList()));

                cfg.CreateMap<AssemblyInfoProperties, ProjectAssemblyData>();

                cfg.CreateMap<Reference, ReferenceData>()
                    .ForMember(dest => dest.Aliases, opt => opt.MapFrom(src => new List<string>(src.Aliases)))
                    .ForMember(dest => dest.Version, opt => opt.MapFrom(src => Version.Parse(src.Version)));
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

        public async Task<ProjectData> ParseProject(ProjectFileData fileData, FileStream projectStream)
        {
            using (var reader = new StreamReader(projectStream))
            {
                var projectDocument = XDocument.Load(reader);

                var projectObject = await Task.Run(() => _projectReader(projectDocument))
                    .ConfigureAwait(false);

                var mapper = _mapperConfig.CreateMapper();

                return mapper.Map<ProjectData>(Tuple.Create(fileData, projectObject));
            }
        }
    }
}
