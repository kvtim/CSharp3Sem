using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryForFiles
{
    class AllData
    {
        public PathsOptions PathsOptions { get; set; }
        public EncryptingOptions EncryptingOptions { get; set; }
        public CompressOptions CompressOptions { get; set; }

        public AllData()
        {
            this.PathsOptions = new PathsOptions();
            this.EncryptingOptions = new EncryptingOptions();
            this.CompressOptions = new CompressOptions();
        }
    }
    
    public class PathsOptions
    {
        [JsonPropertyName("SourceDirectory")]
        public string SourceDirectory { get; set; }
        [JsonPropertyName("TargetDirectory")]
        public string TargetDirectory { get; set; }
    }

    public class EncryptingOptions
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }
    }

    public class CompressOptions
    {
        [JsonPropertyName("CompressFormat")]
        public string CompressFormat { get; set; }
    }
}
