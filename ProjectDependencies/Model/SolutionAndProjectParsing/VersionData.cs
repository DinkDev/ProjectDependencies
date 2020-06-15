namespace ProjectDependencies.Model.SolutionAndProjectParsing
{
    using System;

    public class VersionData
    {
        public static bool TryParse(string source, out VersionData outValue)
        {
            outValue = null;
            var rv = Version.TryParse(source, out var version);
            if (rv)
            {
                outValue = new VersionData(version.Major, version.Minor, version.Build, version.Revision);
            }

            return rv;
        }

        public VersionData()
        {
        }

        public VersionData(int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; } = 1;
        public int Revision { get; set; } = 1;

        public Version ToVersion()
        {
            return new Version(Major, Minor, Build, Revision);
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }
    }
}