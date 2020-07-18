using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WebJobs.Extensions.DataLakeGen2.Bindings
{
    public class AzureDataLakePath
    {
        internal AzureDataLakePath(string dataLakePath)
        {
            try
            {
                if (string.IsNullOrEmpty(dataLakePath)) throw new ArgumentNullException(nameof(dataLakePath));
                var segments = dataLakePath.Split("@");
                if (segments.Length != 2) throw new ArgumentException("Malformed path provided.", nameof(dataLakePath));
                var filesystem = segments[0];
                segments = segments[1].Split(".");
                if (segments.Length < 2) throw new ArgumentException("Malformed path provided.", nameof(dataLakePath));
                var account = segments[0];
                var path = segments.Last().Substring(segments.Last().IndexOf('/'));
                Account = account;
                Filesystem = filesystem;
                Path = path.StartsWith('/') ? path.Substring(1) : path;
            }
            catch
            {
                throw new ArgumentException("Malformed path provided.", nameof(dataLakePath));
            }
        }
        public string Account { get; private set; }
        public string Filesystem { get; private set; }
        public string Path { get; private set; }
    }

}
