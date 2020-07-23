using Azure.Identity;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebJobs.Extensions.DataLakeGen2.Client;
using Azure.Storage.Files.DataLake;
using Azure.Storage;
using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Files.DataLake.Models;

namespace WebJobs.Extensions.DataLakeGen2.Bindings
{
    [Extension("DataLakeGen2StoreBinding")]
    public class DataLakeGen2ExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var fileRule = context.AddBindingRule<DataLakeGen2PathAttribute>();
            fileRule.BindToInput<DataLakeFileClient>(BuildFileFromAttribute);
            fileRule.BindToInput<DataLakeDirectoryClient>(BuildDirectoryFromAttribute);
            fileRule.BindToInput<IEnumerable<DataLakeFileClient>>(BuildFilesFromAttribute);
            fileRule.BindToInput<Stream>(BuildStreamFromAttribute);
            fileRule.BindToInput<string>(BuildStringFromAttribute);
            fileRule.BindToInput<byte[]>(BuildBytesFromAttribute);
            fileRule.AddConverter<string, DataLakeFile>(ConvertStringToDataLakeFile).AddConverter<byte[], DataLakeFile>(ConvertBytesToDataLakeGen2File);
            context.AddOpenConverter<OpenType.Poco, DataLakeFile>(ConvertPocoToDataLakeGen2File);
            fileRule.BindToCollector<DataLakeFile>(BuildCollector);
        }
        private DataLakeFileSystemClient BuildClient(DataLakeGen2PathAttribute arg)
        {
            return arg.GetAuthorizationType() switch
            {
                AuthorizationType.Oauth => new DataLakeServiceClient(new Uri($"https://{arg.AzureDataLakePath.Account}.blob.core.windows.net"),
                    new ClientSecretCredential(
                        arg.ActiveDirectoryTenantId,
                        arg.ActiveDirectoryApplicationId,
                        arg.ActiveDirectoryApplicationSecret)
                        ).GetFileSystemClient(arg.AzureDataLakePath.Filesystem),
                AuthorizationType.Sas => new DataLakeServiceClient(new Uri($"https://{arg.AzureDataLakePath.Account}.blob.core.windows.net?{arg.AzureDataLakeSas.Replace("?", "")}")).GetFileSystemClient(arg.AzureDataLakePath.Filesystem),
                AuthorizationType.SharedKey => new DataLakeServiceClient(new Uri($"https://{arg.AzureDataLakePath.Account}.blob.core.windows.net"), new StorageSharedKeyCredential(arg.AzureDataLakePath.Account, arg.AzureDataLakeKey)).GetFileSystemClient(arg.AzureDataLakePath.Filesystem),
                _ => throw new Exception()
            };
        }
        private Task<object> ConvertPocoToDataLakeGen2File(object src, Attribute attribute, ValueBindingContext context)
        {
            return Task.FromResult<object>(ConvertStringToDataLakeFile(JsonConvert.SerializeObject(src), (DataLakeGen2PathAttribute)attribute));
        }

        private DataLakeFile ConvertBytesToDataLakeGen2File(byte[] arg, DataLakeGen2PathAttribute attr)
        {
            return new DataLakeFile(attr.AzureDataLakePath.Path, arg);
        }

        private DataLakeFile ConvertStringToDataLakeFile(string arg, DataLakeGen2PathAttribute attr)
        {
            return new DataLakeFile(attr.AzureDataLakePath.Path, arg);
        }

        private DataLakeFileClient BuildFileFromAttribute(DataLakeGen2PathAttribute arg)
        {
            return BuildClient(arg).GetFileClient(arg.AzureDataLakePath.Path);
        }

        private IEnumerable<DataLakeFileClient> BuildFilesFromAttribute(DataLakeGen2PathAttribute arg)
        {
            var fsClient = BuildClient(arg);
            return fsClient
            .GetPaths(arg.AzureDataLakePath.Path).AsPages()
            .SelectMany(p => p.Values)
            .Where(p => !p.IsDirectory ?? false)
            .Select(p => fsClient.GetFileClient(p.Name));
        }

        private DataLakeDirectoryClient BuildDirectoryFromAttribute(DataLakeGen2PathAttribute arg)
        {
            return BuildClient(arg).GetDirectoryClient(arg.AzureDataLakePath.Path);
        }

        private string BuildStringFromAttribute(DataLakeGen2PathAttribute arg)
        {
            var client = BuildFileFromAttribute(arg);
            using var sr = new StreamReader(client.Read().GetRawResponse().ContentStream);
            return sr.ReadToEnd();
        }

        private byte[] BuildBytesFromAttribute(DataLakeGen2PathAttribute arg)
        {
            var client = BuildFileFromAttribute(arg);
            using var ms = new MemoryStream();
            client.Read().GetRawResponse().ContentStream.CopyTo(ms);
            return ms.ToArray();
        }

        private Stream BuildStreamFromAttribute(DataLakeGen2PathAttribute arg)
        {
            var client = BuildFileFromAttribute(arg);
            var ms = new MemoryStream();
            client.Read().Value.Content.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }
        private IAsyncCollector<DataLakeFile> BuildCollector(DataLakeGen2PathAttribute arg)
        {
            return new DataLakeGen2AsyncCollector(BuildClient(arg));
        }
    }
    internal class DataLakeGen2AsyncCollector : IAsyncCollector<DataLakeFile>
    {
        private readonly ConcurrentBag<DataLakeFile> _buffer = new ConcurrentBag<DataLakeFile>();
        private readonly DataLakeFileSystemClient _client;

        public DataLakeGen2AsyncCollector(DataLakeFileSystemClient client)
        {
            _client = client;
        }

        public Task AddAsync(DataLakeFile item, CancellationToken cancellationToken = default(CancellationToken))
        {
            _buffer.Add(item);
            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!_buffer.IsEmpty)
            {
                if (_buffer.TryTake(out var file))
                {
                    await _client.GetFileClient(file.Path).UploadAsync(file.Content, overwrite: true);
                }
            }
        }
    }
}
