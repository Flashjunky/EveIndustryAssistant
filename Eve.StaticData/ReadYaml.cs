using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using YamlDotNet.RepresentationModel;

namespace Eve.StaticData
{
    public static class ReadYaml
    {
        public static IConfiguration Configuration { get; set; }
        
        [FunctionName("GetTypeName")]
        public static async Task<HttpResponseMessage> GetTypeName([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{typeid}")] HttpRequestMessage req, int typeid, TraceWriter log)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json");
            Configuration = builder.Build();

            log.Info("Configuration built");
            log.Info("Connection string = " + Configuration.GetConnectionString("evestaticdata"));

            CheckYaml.ProcessFiles(Configuration, log);

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("OK") };
        }
    }
}
