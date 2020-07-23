# AzureFunctionsDataLakeGen2Bindings

## Usage

Authorization uses OAuth. ClientId, ClientSecret, TenantId required.

### Input binding

```
[DataLakeGen2Path(
  ActiveDirectoryApplicationId = "application id",
  ActiveDirectoryApplicationSecret = "application secret",
  ActiveDirectoryTenantId = "tenant id"
  Path (in the form of <filesystem>@<account>.dfs.core.windows.net/<path to file or directory>) input binding
)]
```

Input binding can be one of `Stream`, `string`, `byte[]`, `DataLakeFileClient`, `DataLakeFileClient[]`, `DataLakeDirectoryClient`. Input binding of type `DataLakeDirectoryClient` doesn't seem very useful, but I've included it for completeness.

### Output binding

```
[return: DataLakeGen2Path(
  ActiveDirectoryApplicationId = "application id",
  ActiveDirectoryApplicationSecret = "application secret",
  ActiveDirectoryTenantId = "tenant id"
  Path (in the form of <filesystem>@<account>.dfs.core.windows.net/<path to file or directory>) input binding
)]
```

Output bunding can be one of `string`, `byte[]`.

#### Sources, references, and related work:

https://github.com/Azure/azure-functions-datalake-extension
https://github.com/PeterDowdy/Adlg2Helper
https://microsoft.github.io/AzureTipsAndTricks/blog/tip247.html
https://github.com/Azure/azure-webjobs-sdk/wiki/Creating-custom-input-and-output-bindings