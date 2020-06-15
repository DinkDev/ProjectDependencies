namespace ProjectDependenciesTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using ProjectDependencies.Model.SolutionAndProjectParsing;

    [TestClass]
    [UseReporter(typeof(BeyondCompareReporter))]
    public class SolutionParsingTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Learning_ByteDevDotNet_SolutionParsing_Test1()
        {
            var settings = new TestParserSettings();
            var solutionFile = @"TestFiles\\MerlinTransformSolution.txt";
            var solutionPath = Path.GetDirectoryName(Path.GetFullPath(solutionFile))
                               ?? throw new NullReferenceException(@"Path.GetDirectoryName(Path.GetFullPath(solutionFile)) result is null");

            var solution = DotNetSolution.Load(solutionFile);
            
            foreach (var solutionProject in solution.Projects)
            {
                var projectFile = Path.Combine(solutionPath, solutionProject.Path);

                TestContext.WriteLine("Project: {0}", projectFile);

                if (projectFile.EndsWith(settings.ProjectFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    var project = DotNetProject.Load(projectFile);
                    TestContext.WriteLine("Project References:");

                    foreach (var reference in project.References)
                    {
                        TestContext.WriteLine("    {0}", reference.ToString());
                    }

                    TestContext.WriteLine("Project Targets:");

                    foreach (var target in project.ProjectTargets)
                    {
                        TestContext.WriteLine("    {0}", target.ToString());
                    }

                    TestContext.WriteLine("Package References:");

                    foreach (var package in project.PackageReferences)
                    {
                        TestContext.WriteLine("    {0}", package.ToString());
                    }

                    TestContext.WriteLine("Project References:");

                    foreach (var projectReference in project.ProjectReferences)
                    {
                        TestContext.WriteLine("    {0}", projectReference.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void SolutionFileHelper_Test1()
        {
            var solutionFile = @"TestFiles\\MerlinTransformSolution.txt";
            var settings = new TestParserSettings();
            var parser = new SolutionParser(
                s => new DotNetSolution(s),
                x => new DotNetProject(x),
                settings);
            var sut = new SolutionFileHelper(settings, parser);

            var t = new Version();

            SolutionFileData actualSolution;
            try
            {
                actualSolution = sut.ReadSolutionFileAsync(solutionFile).Result;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Error {0} while reading file {1}", ex, solutionFile);
                throw;
            }

            var actualProjects = new List<ProjectFileData>();
            foreach (var projectFile in actualSolution.ProjectFiles)
            {
                try
                {
                    actualProjects.Add(sut.ReadProjectFileAsync(actualSolution, projectFile).Result);
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine("Error {0} while reading file {1}", ex, projectFile.ProjectPath);
                    throw;
                }
            }

            // TODO: split into 2 tests!
            var actual = new {Solution = actualSolution, Projects = actualProjects};

            var json = JsonConvert.SerializeObject(actual);
            Approvals.VerifyJson(json);
        }
    }
}
