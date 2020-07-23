using System;
using WebJobs.Extensions.DataLakeGen2.Bindings;
using Xunit;

namespace Webjobs.Extensions.DataLakeGen2.Tests.Bindings
{
    public class AzureDataLakePathTests
    {
        [Theory]
        [InlineData("filesystem@fqdn.dfs.core.windows.net/somepath", true)]
        [InlineData("@fqdn.dfs.core.windows.net/somepath", false)]
        [InlineData("filesystem@/somepath", false)]
        [InlineData("filesystem@fqdn.dfs.core.windows.net", false)]
        public void PathValidityTests(string path, bool valid)
        {
            try
            {
                new AzureDataLakePath(path);
                if (!valid) throw new Exception();
            }
            catch
            {
                if (valid) throw;
            }
        }
    }
}
