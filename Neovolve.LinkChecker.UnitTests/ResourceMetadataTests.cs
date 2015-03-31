using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neovolve.LinkChecker.UnitTests
{
    /// <summary>
    ///This is a test class for ResourceMetadataTests and is intended
    ///to contain all ResourceMetadataTests Unit Tests
    ///</summary>
    [TestClass]
    public class ResourceMetadataTests
    {
        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorTest()
        {
            Uri relativeLocation = new Uri("/somelocation", UriKind.RelativeOrAbsolute);
            Uri referrer = new Uri("/anotherlocation", UriKind.RelativeOrAbsolute); 
            
            ResourceMetadata target = new ResourceMetadata(relativeLocation, referrer);
            
            Assert.AreEqual(relativeLocation, target.RelativeLocation, "RelativeLocation returned an incorrect valuue");
            Assert.AreEqual(referrer, target.Referrer, "Referrer returned an incorrect value.");
        }

        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorAbsoluteRelativeLocationTest()
        {
            Uri relativeLocation = new Uri("http://www.test.com/somelocation", UriKind.RelativeOrAbsolute);
            Uri referrer = new Uri("/anotherlocation", UriKind.RelativeOrAbsolute);

            try
            {
                ResourceMetadata target = new ResourceMetadata(relativeLocation, referrer);

                Assert.IsNotNull(target, "Ignore, used to satisfy code analysis rules");

                Assert.Fail("ApplicationExpection was expected");
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorAbsoluteReferrerTest()
        {
            Uri relativeLocation = new Uri("/anotherlocation", UriKind.RelativeOrAbsolute);
            Uri referrer = new Uri("http://www.test.com/somelocation", UriKind.RelativeOrAbsolute);

            try
            {
                ResourceMetadata target = new ResourceMetadata(relativeLocation, referrer);

                Assert.IsNotNull(target, "Ignore, used to satisfy code analysis rules");

                Assert.Fail("ApplicationExpection was expected");
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorNullReferrerTest()
        {
            Uri relativeLocation = new Uri("/somelocation", UriKind.RelativeOrAbsolute);
            const Uri Referrer = null;

            ResourceMetadata target = new ResourceMetadata(relativeLocation, Referrer);

            Assert.AreEqual(relativeLocation, target.RelativeLocation, "RelativeLocation returned an incorrect valuue");
            Assert.AreEqual(Referrer, target.Referrer, "Referrer returned an incorrect value.");
        }

        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorNullRelativeLocationTest()
        {
            const Uri RelativeLocation = null;
            Uri referrer = new Uri("/anotherlocation", UriKind.RelativeOrAbsolute);

            ResourceMetadata target = new ResourceMetadata(RelativeLocation, referrer);

            Assert.AreEqual(RelativeLocation, target.RelativeLocation, "RelativeLocation returned an incorrect valuue");
            Assert.AreEqual(referrer, target.Referrer, "Referrer returned an incorrect value.");
        }

        /// <summary>
        ///A test for ResourceMetadata Constructor
        ///</summary>
        [TestMethod]
        public void ResourceMetadataConstructorNullRelativeLocationNullReferrerTest()
        {
            const Uri RelativeLocation = null;
            const Uri Referrer = null;

            ResourceMetadata target = new ResourceMetadata(RelativeLocation, Referrer);

            Assert.AreEqual(RelativeLocation, target.RelativeLocation, "RelativeLocation returned an incorrect valuue");
            Assert.AreEqual(Referrer, target.Referrer, "Referrer returned an incorrect value.");
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}