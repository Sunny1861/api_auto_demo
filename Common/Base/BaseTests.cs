using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WebAPIDemo.Tests.Common.Base
{
    [TestClass]
    public class BaseTest
    {
        // Static property to store the base URL
        protected static string? BaseUrl;

        public TestContext TestContext { get; set; }

        // This method runs once before any test in the assembly
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Retrieve the base URL from an environment variable or set a default value
            BaseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:5089";

            // Log the base URL to verify (optional)
            Console.WriteLine($"Base URL for the tests is: {BaseUrl}");
        }

        [TestInitialize]
        public void TestStart()
        {
            Console.WriteLine($"[START] Test: {TestContext.TestName} at {DateTime.Now}");
        }

        [TestCleanup]
        public void TestEnd()
        {
            Console.WriteLine($"[END] Test: {TestContext.TestName} at {DateTime.Now}");
        }

        //TODO: More common test setup will add
    }
}