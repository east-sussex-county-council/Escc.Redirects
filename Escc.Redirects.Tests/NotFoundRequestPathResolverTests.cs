using System;
using NUnit.Framework;

namespace Escc.Redirects.Tests
{
    [TestFixture]
    public class NotFoundRequestPathResolverTests
    {
        [Test]
        public void AspxPathPassedByCustomErrors()
        {
            var requestUrl = new Uri("http://www.example.org/404page.aspx?aspxerrorpath=/broken/page.aspx");

            var resolver = new NotFoundRequestPathResolver();
            var originalUrl = resolver.NormaliseRequestedPath(requestUrl);

            Assert.AreEqual(new Uri("http://www.example.org/broken/page.aspx"), originalUrl);
        }

        [Test]
        public void IisPathPassedByHttpErrors()
        {
            var requestUrl = new Uri("http://www.example.org/404page.aspx?404;http://www.example.org/broken/page.aspx");

            var resolver = new NotFoundRequestPathResolver();
            var originalUrl = resolver.NormaliseRequestedPath(requestUrl);

            Assert.AreEqual(new Uri("http://www.example.org/broken/page.aspx"), originalUrl);
        }

        [TestCase("http://www.example.org/404page.aspx?404;http://www.example.org/", "http://www.example.org/")]
        [TestCase("https://www.example.org:1234/HttpStatus/NotFound?404;https://www.example.org:1234/", "https://www.example.org:1234/")]
        public void HomePageNotFoundByIis(string homeUrl, string expected)
        {
            var requestUrl = new Uri(homeUrl);

            var resolver = new NotFoundRequestPathResolver();
            var originalUrl = resolver.NormaliseRequestedPath(requestUrl);

            Assert.AreEqual(new Uri(expected), originalUrl);
        }

        [Test]
        public void NotFoundPageIgnoresItself()
        {
            var requestUrl = new Uri("http://www.example.org/404page.aspx?404;http://www.example.org/404page.aspx");

            var resolver = new NotFoundRequestPathResolver();
            var originalUrl = resolver.NormaliseRequestedPath(requestUrl);

            Assert.IsNull(originalUrl);
        }

        [Test]
        public void TrailingSlashIsNormalised()
        {
            var requestUrl1 = new Uri("http://www.example.org/404page.aspx?404;http://www.example.org/broken");
            var requestUrl2 = new Uri("http://www.example.org/404page.aspx?404;http://www.example.org/broken/");

            var resolver = new NotFoundRequestPathResolver();
            var originalUrl1 = resolver.NormaliseRequestedPath(requestUrl1);
            var originalUrl2 = resolver.NormaliseRequestedPath(requestUrl2);

            Assert.AreEqual(originalUrl1, originalUrl2);
        }
    }
}
