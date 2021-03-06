﻿namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AutoMapper;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using TypeConverters;

    public class SolutionParser
    {
        private readonly Func<string, DotNetSolution> _solutionReader;
        private readonly Func<XDocument, DotNetProject> _projectReader;
        private readonly IFileSettings _settings;
        private readonly MapperConfiguration _mapperConfig;

        public SolutionParser(
            Func<string, DotNetSolution> solutionReader,
            Func<XDocument, DotNetProject> projectReader,
            IFileSettings settings)
        {
            _solutionReader = solutionReader ?? throw new ArgumentNullException(nameof(solutionReader));
            _projectReader = projectReader ?? throw new ArgumentNullException(nameof(projectReader));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing<NullStringConverter>();
                cfg.CreateMap<string, VersionData>().ConvertUsing<VersionStringConverter>();

                cfg.CreateMap<Tuple<string, DotNetSolution>, SolutionFileData>()
                    .ForMember(dest => dest.Name,
                        opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.Item1)))
                    .ForMember(dest => dest.SolutionPath, opt => opt.MapFrom(src => Path.GetFullPath(src.Item1)))
                    .ForMember(dest => dest.VisualStudioVersion,
                        opt => opt.MapFrom(src => src.Item2.VisualStudioVersion))
                    .ForMember(dest => dest.ProjectFiles,
                        opt => opt.MapFrom(src => src.Item2.Projects.Where(p =>
                                p.Path.EndsWith(_settings.ProjectFileExtension, StringComparison.OrdinalIgnoreCase))
                            .ToList()))
                    .AfterMap((s, d) =>
                    {
                        var basePath = Path.GetDirectoryName(d.SolutionPath);
                        foreach (var projectFile in d.ProjectFiles)
                        {
                            projectFile.ProjectPath = Path.Combine(basePath, projectFile.ProjectPath);
                        }
                    });

                cfg.CreateMap<DotNetSolutionProject, ProjectFileData>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.ProjectPath, opt => opt.MapFrom(src => src.Path))
                    .ForMember(dest => dest.ProjectFileDataId, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<Tuple<SolutionFileData, ProjectFileData, DotNetProject>, ProjectFileData>()
                    //.ForMember(dest => dest.FileData, opt => opt.MapFrom(src => src.Item2))
                    .ForMember(dest => dest.SolutionProjectReferences,
                        opt => opt.ResolveUsing(src =>
                        {
                            return src.Item3.ProjectReferences.Select(r => new SolutionProjectReferenceData
                            {
                                Name = Path.GetFileNameWithoutExtension(r.FilePath),
                                SolutionName = src.Item1.Name,
                                ProjectPath = Path.Combine(Path.GetFullPath(src.Item1.SolutionPath), r.FilePath)
                            });
                        }))
                    .ForMember(dest => dest.LibraryReferences,
                        opt => opt.MapFrom(src => src.Item3.References.Select(r => r).ToList()));

                cfg.CreateMap<Reference, ProjectLibraryReferenceData>();

            });
        }

        public async Task<SolutionFileData> ParseSolution(string solutionPath, Stream solutionStream)
        {
            using (var reader = new StreamReader(solutionStream))
            {
                var solutionText = await reader.ReadToEndAsync();
                var solutionObject = await Task.Run(() => _solutionReader(solutionText))
                    .ConfigureAwait(false);

                var mapper = _mapperConfig.CreateMapper();

                return mapper.Map<SolutionFileData>(Tuple.Create(solutionPath, solutionObject));
            }
        }

        public async Task<ProjectFileData> ParseProject(ProjectFileData projectFile, SolutionFileData solution,
            FileStream projectStream)
        {
            using (var reader = new StreamReader(projectStream))
            {
                var projectDocument = XDocument.Load(reader);

                var projectObject = await Task.Run(() => _projectReader(projectDocument))
                    .ConfigureAwait(false);

                var mapper = _mapperConfig.CreateMapper();

                return mapper.Map<ProjectFileData>(Tuple.Create(solution, projectFile, projectObject));
            }
        }
    }
}
