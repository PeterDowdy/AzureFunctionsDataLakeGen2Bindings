using System.IO;
using System.Text;

namespace WebJobs.Extensions.DataLakeGen2.Client
{
    public class DataLakeFile
    {
        public DataLakeFile(string path, string content)
        {
            Path = path;
            Content = new MemoryStream(Encoding.UTF8.GetBytes(content));
        }
        public DataLakeFile(string path, byte[] content)
        {
            Path = path;
            Content = new MemoryStream(content);
        }
        public DataLakeFile(string path, Stream content)
        {
            Path = path;
            Content = content;
        }
        public string Path { get; private set; }
        public Stream Content { get; private set; }
    }
}
