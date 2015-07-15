using System;
using NUnit.Framework;

namespace Escc.Redirects.Tests
{
    [TestFixture]
    public class PreserveQueryStringHandlerTests
    {
        [Test]
        public void NoQueryStringDoesNothing()
        {
            var url = new Uri("http://www.example.org/page1.html");
            var redirect = new Redirect() {RequestedUrl = url, DestinationUrl = url};

            var handler = new PreserveQueryStringHandler();
            redirect = handler.HandleRedirect(redirect);

            Assert.AreEqual(url, redirect.DestinationUrl);
        }

        [Test]
        public void OriginalQueryStringPreservedIfNoDestinationQueryString()
        {
            var redirect = new Redirect() { RequestedUrl =  new Uri("http://www.example.org/page1.html?test=test"), DestinationUrl =  new Uri("http://www.example.org/page2.html") };

            var handler = new PreserveQueryStringHandler();
            redirect = handler.HandleRedirect(redirect);

            Assert.AreEqual(new Uri("http://www.example.org/page2.html?test=test"), redirect.DestinationUrl);
        }


        [Test]
        public void DestinationQueryStringPreservedIfPresent()
        {
            var redirect = new Redirect() { RequestedUrl = new Uri("http://www.example.org/page1.html"), DestinationUrl = new Uri("http://www.example.org/page2.html?test=test") };

            var handler = new PreserveQueryStringHandler();
            redirect = handler.HandleRedirect(redirect);

            Assert.AreEqual(new Uri("http://www.example.org/page2.html?test=test"), redirect.DestinationUrl);
        }
    }
}
