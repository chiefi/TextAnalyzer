using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextAnalyzer.Reader;

namespace TextAnalyzer.Tests
{
    [TestClass]
    public class DocumentReaderTest
    {
        [TestMethod]
        public void TestUrl()
        {
            var reader = new DocumentReader();

            Assert.AreEqual(true, reader.IsUrl("http://www.xyz.com/text.txt"), "Url is not classified correctly");
            Assert.AreEqual(true, reader.IsUrl("https://www.xyz.com/text.txt"), "Url is not classified correctly");
            Assert.AreEqual(true, reader.IsUrl("http://www.xyz.com/text.xyz"), "Url is not classified correctly");
            Assert.AreEqual(true, reader.IsUrl("http://www.xyz.com/texda/text.txt"), "Url is not classified correctly");
           
            Assert.AreEqual(false, reader.IsUrl("text.txt"), "Url is not classified correctly");
            Assert.AreEqual(false, reader.IsUrl("c:\text.txt"), "Url is not classified correctly");
            Assert.AreEqual(false, reader.IsUrl("d1212d12"), "Url is not classified correctly");
            Assert.AreEqual(false, reader.IsUrl("..\\text.txt"), "Url is not classified correctly");
            Assert.AreEqual(false, reader.IsUrl("../idg.txt"), "Url is not classified correctly");
        }
    }
}
