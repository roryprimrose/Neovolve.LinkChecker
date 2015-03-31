using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neovolve.LinkChecker.UnitTests
{
    /// <summary>
    ///This is a test class for ResourceResolverTests and is intended
    ///to contain all ResourceResolverTests Unit Tests
    ///</summary>
    [TestClass]
    public class ResourceResolverTests
    {
        private const String TestBaseLocation = "http://localhost/BE/";

        private readonly Uri TestBaseUri = new Uri(TestBaseLocation);

        /// <summary>
        ///A test for DetermineRelativeLocation
        ///</summary>
        [TestMethod]
        public void DetermineRelativeLocationFragmentTest()
        {
            const String Expected = "somepage.aspx";
            const String RelativePath = Expected + "#someBookmark";

            RunDetermineRelativeLocationTest(RelativePath, Expected);
        }

        /// <summary>
        ///A test for DetermineRelativeLocation
        ///</summary>
        [TestMethod]
        public void DetermineRelativeLocationNoRelativePathTest()
        {
            String relativePath = String.Empty;

            RunDetermineRelativeLocationTest(relativePath, relativePath);
        }

        /// <summary>
        ///A test for DetermineRelativeLocation
        ///</summary>
        [TestMethod]
        public void DetermineRelativeLocationQueryStringAndFragmentTest()
        {
            const String Expected = "somepage.aspx?somevalue=123";
            const String RelativePath = Expected + "#someBookmark";

            RunDetermineRelativeLocationTest(RelativePath, Expected);
        }

        /// <summary>
        ///A test for DetermineRelativeLocation
        ///</summary>
        [TestMethod]
        public void DetermineRelativeLocationQueryStringTest()
        {
            const String Expected = "somepage.aspx?somevalue=123";

            RunDetermineRelativeLocationTest(Expected, Expected);
        }

        /// <summary>
        ///A test for DetermineRelativeLocation
        ///</summary>
        [TestMethod]
        public void DetermineRelativeLocationTest()
        {
            const String Expected = "somepage.aspx";

            RunDetermineRelativeLocationTest(Expected, Expected);
        }

        /// <summary>
        ///A test for GetRelatedResources
        ///</summary>
        [TestMethod]
        public void GetRelatedResourcesNullParentRelativeLocationTest()
        {
            RunGetRelatedResourcesTest(null);
        }

        /// <summary>
        ///A test for GetRelatedResources
        ///</summary>
        [TestMethod]
        public void GetRelatedResourcesTest()
        {
            Uri parentRelativeLocation = new Uri("someLocation/default.aspx", UriKind.RelativeOrAbsolute);

            RunGetRelatedResourcesTest(parentRelativeLocation);
        }

        /// <summary>
        ///A test for ResourceResolver Constructor
        ///</summary>
        [TestMethod]
        public void ResourceResolverConstructorNullResourceLoaderTest()
        {
            try
            {
                ResourceResolver target = new ResourceResolver(null);

                Assert.IsNotNull(target, "Ignore, used to satisfy code analysis");

                Assert.Fail("ArgumentNullException was expected");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("loader", ex.ParamName, "ParamName returned an incorrect value");
            }
        }

        /// <summary>
        ///A test for ResourceResolver Constructor
        ///</summary>
        [TestMethod]
        public void ResourceResolverConstructorTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);

            Assert.IsNotNull(target, "Ignore, used to satisfy code analysis");
        }

        [TestMethod]
        public void UpdateResourceMetadataNullMetadataTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            const Uri baseLocation = null;
            const ResourceMetadata Metadata = null;

            try
            {
                target.UpdateResourceMetadata(Metadata, baseLocation);

                Assert.Fail("Failed to throw ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("metadata", ex.ParamName, "ParamName returned an incorrect value");
            } 
        }

        /// <summary>
        ///A test for UpdateResourceMetadata
        ///</summary>
        [TestMethod]
        public void UpdateResourceMetadataNullBaseLocationTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            Uri relativeLocation = new Uri("somelocation.aspx", UriKind.Relative);
            const Uri baseLocation = null;
            ResourceMetadata metadata = new ResourceMetadata(relativeLocation, null);

            try
            {
                target.UpdateResourceMetadata(metadata, baseLocation);

                Assert.Fail("Failed to throw ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("baseLocation", ex.ParamName, "ParamName returned an incorrect value");
            }
        }

        /// <summary>
        ///A test for UpdateResourceMetadata
        ///</summary>
        [TestMethod]
        public void UpdateResourceMetadataNullRelativeLocationTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            const Uri relativeLocation = null;
            ResourceMetadata metadata = new ResourceMetadata(relativeLocation, null);

            loader.StateToReturn = new ResourceState();
            loader.StateToReturn.MimeType = Guid.NewGuid().ToString();
            loader.StateToReturn.ResponseCode = HttpStatusCode.UseProxy;

            target.UpdateResourceMetadata(metadata, TestBaseUri);

            Assert.AreEqual(loader.StateToReturn.MimeType, metadata.MimeType, "MimeType returned an incorrect value");
            Assert.AreEqual(
                loader.StateToReturn.ResponseCode, metadata.ResponseCode, "ResponseCode returned an incorrect value");
        }

        /// <summary>
        ///A test for UpdateResourceMetadata
        ///</summary>
        [TestMethod]
        public void UpdateResourceMetadataTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            Uri relativeLocation = new Uri("testLocation/default.aspx", UriKind.Relative);
            ResourceMetadata metadata = new ResourceMetadata(relativeLocation, null);

            loader.StateToReturn = new ResourceState();
            loader.StateToReturn.MimeType = Guid.NewGuid().ToString();
            loader.StateToReturn.ResponseCode = HttpStatusCode.UseProxy;

            target.UpdateResourceMetadata(metadata, TestBaseUri);

            Assert.AreEqual(loader.StateToReturn.MimeType, metadata.MimeType, "MimeType returned an incorrect value");
            Assert.AreEqual(
                loader.StateToReturn.ResponseCode, metadata.ResponseCode, "ResponseCode returned an incorrect value");
        }

        /// <summary>
        ///A test for UpdateResourceMetadata
        ///</summary>
        [TestMethod]
        public void UpdateResourceMetadataRelativeBaseLocationTest()
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            Uri relativeLocation = new Uri("somelocation.aspx", UriKind.Relative);
            ResourceMetadata metadata = new ResourceMetadata(relativeLocation, null);

            try
            {
                target.UpdateResourceMetadata(metadata, relativeLocation);

                Assert.Fail("Failed to throw ArgumentNullException");
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #region Helper Methods

        private void RunDetermineRelativeLocationTest(String TestValue, String Expected)
        {
            Uri actual = ResourceResolver_Accessor.DetermineRelativeLocation(TestValue, TestBaseUri);

            Assert.AreEqual(Expected, actual.ToString(), "DetermineRelativeLocation returned an incorrect value");
        }

        /// <summary>
        /// Runs the get related resources test.
        /// </summary>
        /// <param name="parentRelativeLocation">The parent relative location.</param>
        private void RunGetRelatedResourcesTest(Uri parentRelativeLocation)
        {
            ResourceLoaderStub loader = new ResourceLoaderStub();
            ResourceResolver target = new ResourceResolver(loader);
            ResourceMetadata parentMetadata = new ResourceMetadata(parentRelativeLocation, null);

            loader.ContentToReturn =
                @"
                some content

                Testing with single quotes
                <a href=""http://localhost/BE/"">here</a>
                <a href=""random.aspx"">here</a>
                <a href=""http://www.google.com/unrelated"">here</a>
                <a href=""http://localhost/BE/random.aspx?content=different"">here</a>
                <a href=""http://localhost/BE/random.aspx#bookmark"">here</a>
                <a href=""mailto:some@testing.com"">here</a>
                <a href=""javascript: return true;"">here</a>
                <img src=""http://localhost/BE/test.jpg"" />
                <img src=""testing.jpg"" />
                <img src=""http://www.google.com/unrelated.jpg"" />

                Testing with double quotes
                <a href=""http://localhost/BE/"">here</a>
                <a href=""random.aspx"">here</a>
                <a href=""http://www.google.com/unrelated"">here</a>
                <a href=""http://localhost/BE/random.aspx?content=different"">here</a>
                <a href=""http://localhost/BE/random.aspx#bookmark"">here</a>
                <a href=""mailto:some@testing.com"">here</a>
                <a href=""javascript: return true;"">here</a>
                <img src=""http://localhost/BE/test.jpg"" />
                <img src=""testing.jpg"" />
                <img src=""http://www.google.com/unrelated.jpg"" />           
            ";

            Collection<ResourceMetadata> actual = target.GetRelatedResources(parentMetadata, TestBaseUri);

            Assert.IsNotNull(actual, "GetRelatedResources returned an incorrect value");
            Assert.AreEqual(5, actual.Count, "Count of items is incorrect");
        }

        #endregion

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