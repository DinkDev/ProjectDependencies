namespace ProjectDependencies.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Static class to access internal Microsoft.Build.Construction.SolutionParser via reflection.
    /// </summary>
    public class SolutionParserWrapper
    {

        //static SolutionParserWrapper()
        //{
        //    _solutionParserType =
        //        Type.GetType(
        //            "Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.8.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        //            false, false)
        //        ?? throw new TypeLoadException($"Unable to load type Microsoft.Build.Construction.SolutionParser");

        //    _solutionReaderProperty =
        //        _solutionParserType.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
        //    _projectsProperty =
        //        _solutionParserType.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
        //    _parseSolutionMethod =
        //        _solutionParserType.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);
        //}

        public List<ProjectInSolutionWrapper> Projects { get; }

        public SolutionParserWrapper(string solutionFileName)
        {
            //if (_solutionParserType == null)
            //{
            //    throw new InvalidOperationException(
            //        "Can not find type 'Microsoft.Build.Construction.SolutionParser' are you missing a assembly reference to 'Microsoft.Build.dll'?");
            //}

            //var solutionParser = _solutionParserType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            //    .First().Invoke(null);
            //using (var streamReader = new StreamReader(solutionFileName))
            //{
            //    _solutionReaderProperty.SetValue(solutionParser, streamReader, null);
            //    _parseSolutionMethod.Invoke(solutionParser, null);
            //}

            //var projects = new List<ProjectInSolutionWrapper>();
            //var array = (Array)_projectsProperty.GetValue(solutionParser, null);
            //for (int i = 0; i < array.Length; i++)
            //{
            //    projects.Add(new ProjectInSolutionWrapper(array.GetValue(i)));
            //}

            //Projects = projects;
        }
    }
}