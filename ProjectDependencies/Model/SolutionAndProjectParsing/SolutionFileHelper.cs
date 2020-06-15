namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class SolutionFileHelper
    {
        private readonly IFileSettings _settings;
        private readonly SolutionParser _parser;

        public SolutionFileHelper(IFileSettings settings, SolutionParser parser)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public async Task<SolutionFileData> ReadSolutionFileAsync(string solutionFile)
        {
            return await Task.Run(() =>
            {
                using (var solutionStream = File.OpenRead(solutionFile))
                {
                    return _parser.ParseSolution(solutionFile, solutionStream).Result;
                }
            }).ConfigureAwait(false);
        }

        public async Task<ProjectFileData> ReadProjectFileAsync(SolutionFileData solution, ProjectFileData projectFile)
        {
            return await Task.Run(() =>
            {
                using (var projectStream = File.OpenRead(projectFile.ProjectPath))
                {
                    return _parser.ParseProject(projectFile, solution, projectStream).Result;
                }
            }).ConfigureAwait(false);
        }

        public async Task<string[]> SearchForSolutionsAsync(string searchFolder)
        {
            return await Task.Run(() =>
            {
                var rv = new List<string>();

                var pending = new Stack<string>();
                pending.Push(searchFolder);
                while (pending.Count != 0)
                {
                    var path = pending.Pop();
                    var next = new string[] { };
                    try
                    {
                        next = Directory.GetFiles(path, _settings.SolutionSearchWildcard);
                    }
                    catch
                    {
                        // ignored
                    }

                    foreach (var file in next)
                    {
                        rv.Add(file);
                    }

                    try
                    {
                        next = Directory.GetDirectories(path);
                        foreach (var subDirectory in next)
                        {
                            pending.Push(subDirectory);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return rv.ToArray();
            }).ConfigureAwait(false);
        }
    }
}
