using Azure.Storage.Files.DataLake;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebJobs.Extensions.DataLakeGen2.Bindings;

namespace Webjobs.Extensions.DataLakeGen2.Samples
{
    public class InputSample
    {
        [FunctionName("InputStream")]
        public IActionResult InputStream([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/stream")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
                ActiveDirectoryApplicationSecret = "%clientsecret%",
                ActiveDirectoryTenantId = "%tenantid%",
                Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input/stream")] Stream myfile,
            ILogger log)
        {
            using (var reader = new StreamReader(myfile))
            {
                return new OkObjectResult(reader.ReadToEnd());
            }
        }
        [FunctionName("InputString")]
        public IActionResult InputString([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/string")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
                ActiveDirectoryApplicationSecret = "%clientsecret%",
                ActiveDirectoryTenantId = "%tenantid%",
                Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input/string")] string myfile,
            ILogger log)
        {
            return new OkObjectResult(myfile);
        }
        [FunctionName("InputBytes")]
        public IActionResult InputBytes([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/bytes")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
            ActiveDirectoryApplicationSecret = "%clientsecret%",
            ActiveDirectoryTenantId = "%tenantid%",
            Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input/bytes")] byte[] myfile,
            ILogger log)
        {
            return new OkObjectResult(Encoding.UTF8.GetString(myfile));
        }
        [FunctionName("InputFile")]
        public IActionResult InputFile([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/file")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
            ActiveDirectoryApplicationSecret = "%clientsecret%",
            ActiveDirectoryTenantId = "%tenantid%",
            Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input/file")] DataLakeFileClient myfile,
            ILogger log)
        {
            using var reader = new StreamReader(myfile.Read().Value.Content);
            return new OkObjectResult(reader.ReadToEnd());
        }
        [FunctionName("InputFiles")]
        public async Task<IActionResult> InputFiles([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/files")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
                ActiveDirectoryApplicationSecret = "%clientsecret%",
                ActiveDirectoryTenantId = "%tenantid%",
                Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input")] IEnumerable<DataLakeFileClient> files,
            ILogger log)
        {
            var results = new List<string>();
            foreach (var f in files)
            {
                using var reader = new StreamReader(f.Read().Value.Content);
                results.Add(await reader.ReadToEndAsync());
            }
            return new OkObjectResult(results);
        }
        [FunctionName("InputDirectory")]
        public IActionResult InputDirectory([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/directory")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
                ActiveDirectoryApplicationSecret = "%clientsecret%",
                ActiveDirectoryTenantId = "%tenantid%",
                Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input"
            )] DataLakeDirectoryClient myfile,
            ILogger log)
        {
            return new OkObjectResult(myfile.Path);
        }
        [FunctionName("InputDateTime")]
        public IActionResult InputDateTime([HttpTrigger(AuthorizationLevel.Function, "get", Route = "input/dateTime")] HttpRequest req,
            [DataLakeGen2Path(
                ActiveDirectoryApplicationId = "%applicationid%",
                ActiveDirectoryApplicationSecret = "%clientsecret%",
                ActiveDirectoryTenantId = "%tenantid%",
                Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/input/{DateTime}")] string myfile,
            ILogger log)
        {
            return new OkObjectResult(myfile);
        }
    }
}
