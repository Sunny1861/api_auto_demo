using System.Text.Json;
using System.Text;

namespace WebAPIDemo.Tests.Common.Base
{
    [TestClass]
    public class BaseTest
    {
        // Static property to store the base URL
        protected static HttpClient Client { get; private set; } = null!;

        public TestContext TestContext { get; set; } = null!;

        protected static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
         {
             PropertyNameCaseInsensitive = true
         };

        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            string baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:5005";
            Client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl) // Update if needed
            };
        }

        [TestInitialize]
        public void TestStart()
        {
            Console.WriteLine($"[START] Test: {TestContext.TestName} at {DateTime.Now}");
        }

        [TestCleanup]
        public void TestEnd()
        {
            // TODO: Take meanful inforation if test fail
            Console.WriteLine($"[END] Test: {TestContext.TestName} at {DateTime.Now}");
        }
        
        protected async Task<HttpResponseMessage> SafeSendAsync(Func<Task<HttpResponseMessage>> action, string operation)
        {
            try
            {
                var response = await action();
                Console.WriteLine($"[{operation}] {response.StatusCode}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{operation}] Exception: {ex.Message}");
                throw;
            }
        }

        protected StringContent AsStringContent(object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected async Task<T?> DeserializeJsonAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(str, JsonOptions);
        }

        //TODO: More common test setup will add
    }
}