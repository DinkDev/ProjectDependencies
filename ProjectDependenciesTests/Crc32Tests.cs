namespace ProjectDependenciesTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ProjectDependencies.Model;

    [TestClass]
    public class Crc32Tests
    {
        [TestMethod]
        public void Crc32_Test1()
        {
            var sut = new Crc32();

            var buffer = new[]
            {
                (byte)1,
                (byte)2,
                (byte)3,
                (byte)4,
                (byte)5, 
                (byte)6
            };

            var result = sut.BytesToUint(sut.ComputeHash(buffer));

            Assert.AreEqual(2180413220U, result);
        }
    }
}
