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
        private readonly Crc32 _crc;

        public SolutionFileHelper(IFileSettings settings, SolutionParser parser, Crc32 crc)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _crc = crc;
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

        public async Task<Tuple<string,uint>[]> SearchForSolutionsAsync(string searchFolder)
        {
            return await Task.Run(() =>
            {
                var rv = new List<Tuple<string, uint>>();

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
                        var crc = CalcCrc(file);


                        rv.Add(Tuple.Create(file, crc));
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

        private uint CalcCrc(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return _crc.BytesToUint(_crc.ComputeHash(stream));
            }
        }
    }
}
