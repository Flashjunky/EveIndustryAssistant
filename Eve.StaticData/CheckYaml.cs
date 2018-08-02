using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Host;
using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;
using Eve.StaticData.Interfaces;
using Eve.StaticData.DTO;
using YamlDotNet.Core.Tokens;

namespace Eve.StaticData
{
    public static class CheckYaml
    {
        private static CloudStorageAccount storageAccount;
        private static CloudBlobClient cloudBlobClient;
        private static CloudBlobContainer cloudBlobContainer;
        private static DocumentClient documentClient;

        private const string documentDatabase = @"eveindustryassistant";

        public static void ProcessFiles(IConfiguration config, TraceWriter log)
        {
            storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("evestaticdata"));
            log.Info("Cloud Storage Account created");
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
            log.Info("Cloud blob client initialised");
            cloudBlobContainer = cloudBlobClient.GetContainerReference("evestaticdata");
            log.Info("Cloud blob container created");
            CloudBlockBlob typeIds = cloudBlobContainer.GetBlockBlobReference("typeIDs.yaml");
            log.Info("typeIDs.yaml blob reference created");
            CloudBlockBlob blueprints = cloudBlobContainer.GetBlockBlobReference("blueprints.yaml");
            log.Info("blueprints.yaml blob reference created");

            try
            {
                var exists = typeIds.ExistsAsync().Result;
                if (exists) ProcessTypeId(typeIds, log);
                exists = blueprints.ExistsAsync().Result;
                if (exists) ProcessBlueprints(blueprints, log);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
        }

        private static void ProcessBlueprints(CloudBlockBlob blueprints, TraceWriter log)
        {
            documentClient = new DocumentClient(
                new Uri("https://localhost:8081"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            return;
        }

        private static async void ProcessTypeId(CloudBlockBlob typeIds, TraceWriter log)
        {
            documentClient = new DocumentClient(
                new Uri("https://localhost:8081"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            Task<string> blobStringData = null;
            blobStringData = (typeIds.DownloadTextAsync());
            blobStringData.Wait();

            var input = new StringReader(blobStringData.Result);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            List<EveType> types = new List<EveType>();

            foreach (var entry in mapping.Children)
            {
                var typeId = ((YamlScalarNode)entry.Key).Value;
                var basePrice = (YamlScalarNode)mapping.Children[new YamlScalarNode("items")];
            }

            //await CreateDocumentAsync(documentClient, new iDocumentWithId { Id = 1 });
        }

        private static async Task CreateDocumentAsync<T>(DocumentClient client, T o) where T : iDocumentWithId
        {
            try
            {
                await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(documentDatabase, "types", o.Id.ToString()));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(documentDatabase, "types"), o);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
