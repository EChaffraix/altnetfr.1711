using System;
using common;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using service.Controllers;

namespace service.tests
{
[TestFixture]
    public class PingControllerTester
    {
        [Test]
        public void TheControllerMustReturnsItsVersion(){
            var controller = new PingController();
            var result = controller.Version();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            
            var content = result as OkObjectResult;
            var item = content.Value as VersionMessage;
            Assert.That(item, Is.Not.Null);
            Assert.That(item.Version, Is.EqualTo("0.1"));
        }
    }
}
