
using System.Runtime.CompilerServices;
using Microsoft.Azure.WebJobs.Extensions.DataLakeGen2;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebJobs.Extensions.DataLakeGen2.Models;

[assembly: WebJobsStartup(typeof(AzureDataLakeGen2WebJobsStartup))]
[assembly: InternalsVisibleTo("Webjobs.Extensions.DataLakeGen2.Tests")]
namespace Microsoft.Azure.WebJobs.Extensions.DataLakeGen2
{
    public class AzureDataLakeGen2WebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.AddDataLakeGen2Store();
        }
    }
}