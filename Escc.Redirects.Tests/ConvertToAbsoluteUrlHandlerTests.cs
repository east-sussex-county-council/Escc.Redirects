using System;
using NUnit.Framework;

namespace Escc.Redirects.Tests
{
    [TestFixture]
    public class ConvertToAbsoluteUrlHandlerTests
    {
        [Test]
        public void RelativeUrlIsConverted()
        {
            var relative = new Uri("/folder/page1.html", UriKind.Relative);
            var baseUrl = new Uri("http://example.org");
            var redirect = new Redirect() {RequestedUrl = baseUrl, DestinationUrl = relative};

            var handler = new ConvertToAbsoluteUrlHandler();
            handler.HandleRedirect(redirect);

            Assert.AreEqual(new Uri("http://example.org/folder/page1.html"), redirect.DestinationUrl);
        }

        [Test]
        public void AbsoluteUrlIsNotConverted()
        {
            var original = new Uri("http://www.example.org/folder/page1.html");
            var baseUrl = new Uri("http://example.org");
            var redirect = new Redirect() { RequestedUrl = baseUrl, DestinationUrl = original };

            var handler = new ConvertToAbsoluteUrlHandler();
            handler.HandleRedirect(redirect);

            Assert.AreEqual(original, redirect.DestinationUrl);
        }
    }
}
