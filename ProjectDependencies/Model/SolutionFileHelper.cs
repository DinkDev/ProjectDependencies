namespace ProjectDependencies.Model
{
    using System;
    using System.IO;
    using DataAccess;

    public class SolutionFileHelper
    {
        private readonly SolutionParser _parser;

        public SolutionFileHelper(SolutionParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public SolutionData ReadSolutionFile(string solutionFile)
        {
            using (var solutionStream = File.OpenRead(solutionFile))
            {
                return _parser.ParseSolution(solutionFile, solutionStream).Result;
            }
        }

        public ProjectData ReadProjectFile(SolutionData solution, ProjectFileData projectFile)
        {
            using (var projectStream = File.OpenRead(projectFile.ProjectPath))
            {
                return _parser.ParseProject(projectFile, solution, projectStream).Result;
            }
        }
    }
}
