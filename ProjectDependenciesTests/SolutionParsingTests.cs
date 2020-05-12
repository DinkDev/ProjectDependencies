﻿namespace ProjectDependenciesTests
{
    using System;
    using System.IO;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using ProjectDependencies.DataAccess;
    using ProjectDependencies.Model;

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
        public void SolutionParser_Test1()
        {
            var solutionFile = @"TestFiles\\MerlinTransformSolution.txt";
            var settings = new TestParserSettings();
            var sut = new SolutionParser(
                s => new DotNetSolution(s),
                x => new DotNetProject(x),
                settings);

            SolutionData actual;

            try
            {
                using (var solutionStream = File.OpenRead(solutionFile))
                {
                    actual = sut.ParseSolution(solutionFile, solutionStream).Result;
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Error {0} while reading file {1}", ex, solutionFile);
                throw;
            }

            foreach (var projectFile in actual.ProjectFiles)
            {
                try
                {
                    using (var projectStream = File.OpenRead(projectFile.ProjectPath))
                    {
                        actual.Projects.Add(sut.ParseProject(projectFile, actual, projectStream).Result);
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine("Error {0} while reading file {1}", ex, projectFile.ProjectPath);
                    throw;
                }
            }

            var json = JsonConvert.SerializeObject(actual);
            Approvals.VerifyJson(json);
        }
    }
}
