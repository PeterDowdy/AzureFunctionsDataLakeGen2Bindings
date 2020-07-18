using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using WebJobs.Extensions.DataLakeGen2.Bindings;

namespace Webjobs.Extensions.DataLakeGen2.Samples
{
    public class Outputsample
    {
        [return: DataLakeGen2Path(
            ActiveDirectoryApplicationId = "%applicationid%",
            ActiveDirectoryApplicationSecret = "%clientsecret%",
            ActiveDirectoryTenantId = "%tenantid%",
            Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/output/string"
            )]
        [FunctionName("OutputString")]
        public string OutputString([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "output/string")] HttpRequest req,
            ILogger log)
        {
            using var sr = new StreamReader(req.Body);
            return sr.ReadToEnd();
        }
        [return: DataLakeGen2Path(
            ActiveDirectoryApplicationId = "%applicationid%",
            ActiveDirectoryApplicationSecret = "%clientsecret%",
            ActiveDirectoryTenantId = "%tenantid%",
            Path = "functionbindingtest@%fqdn%.dfs.core.windows.net/output/bytearray"
            )]
        [FunctionName("OutputBytes")]
        public async Task<byte[]> OutputBytes([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "output/bytes")] HttpRequest req,
            ILogger log)
        {
            using var ms = new MemoryStream();
            await req.Body.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
