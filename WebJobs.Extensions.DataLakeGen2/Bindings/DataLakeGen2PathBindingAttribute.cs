using Microsoft.Azure.WebJobs.Description;
using System;
using System.IO;

namespace WebJobs.Extensions.DataLakeGen2.Bindings
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class DataLakeGen2PathAttribute : Attribute
    {
        [AutoResolve]
        public string ActiveDirectoryApplicationId { get; set; }
        [AutoResolve]
        public string ActiveDirectoryApplicationSecret { get; set; }
        [AutoResolve]
        public string ActiveDirectoryTenantId { get; set; }
        [AutoResolve]
        public string AzureDataLakeKey { get; set; }
        [AutoResolve]
        public string AzureDataLakeSas { get; set; }
        [AutoResolve]
        public string Path { get; set; }
        internal AzureDataLakePath AzureDataLakePath => new AzureDataLakePath(Path);
        internal AuthorizationType GetAuthorizationType()
        {
            if (!string.IsNullOrEmpty(ActiveDirectoryApplicationId)
            && !string.IsNullOrEmpty(ActiveDirectoryApplicationSecret)
            && !string.IsNullOrEmpty(ActiveDirectoryTenantId)
            && string.IsNullOrEmpty(AzureDataLakeKey)
            && string.IsNullOrEmpty(AzureDataLakeSas))
                return AuthorizationType.Oauth;
            if (string.IsNullOrEmpty(ActiveDirectoryApplicationId)
            && string.IsNullOrEmpty(ActiveDirectoryApplicationSecret)
            && string.IsNullOrEmpty(ActiveDirectoryTenantId)
            && !string.IsNullOrEmpty(AzureDataLakeKey)
            && string.IsNullOrEmpty(AzureDataLakeSas))
                return AuthorizationType.SharedKey;
            if (string.IsNullOrEmpty(ActiveDirectoryApplicationId)
            && string.IsNullOrEmpty(ActiveDirectoryApplicationSecret)
            && string.IsNullOrEmpty(ActiveDirectoryTenantId)
            && string.IsNullOrEmpty(AzureDataLakeKey)
            && !string.IsNullOrEmpty(AzureDataLakeSas))
                return AuthorizationType.Sas;
            throw new Exception();
        }
    }
    internal enum AuthorizationType
    {
        Oauth,
        SharedKey,
        Sas
    }
}
