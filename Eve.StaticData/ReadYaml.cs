using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;

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

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.GetConnectionString("evestaticdata"));
            log.Info("Cloud Storage Account created");
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            log.Info("Cloud blob client initialised");
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("evestaticdata");
            log.Info("Cloud blob container created");
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference("typeIDs.yaml");
            log.Info("typeIDs.yaml blob available");

            Task<string> yaml = null;
            var stream = new MemoryStream();
            yaml = (blob.DownloadTextAsync());
            yaml.Wait();

            //json = JObject.Parse(yaml.Result); Doesn't work :(

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            var result = deserializer.Deserialize<List<DTO.TypeID>>(yaml.Result);

            foreach(var item in result)
            {
                log.Info(item.Name[0].Item2);
            }

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("OK") };
        }
    }
}
