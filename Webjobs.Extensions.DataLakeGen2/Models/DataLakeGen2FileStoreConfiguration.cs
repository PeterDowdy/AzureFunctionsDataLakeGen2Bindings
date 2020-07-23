using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;
using WebJobs.Extensions.DataLakeGen2.Bindings;

namespace WebJobs.Extensions.DataLakeGen2.Models
{

    public static class DataLakeGen2FileStoreConfiguration
    {
        public static IWebJobsBuilder AddDataLakeGen2Store(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.AddExtension<DataLakeGen2ExtensionConfigProvider>();
            return builder;
        }
    }

}
