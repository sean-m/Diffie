using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Diffie {
    public class DiffJob {
        public DiffJob() { }
        public dynamic Left { get; set; }
        public dynamic Right { get; set; }
        public bool ShowInitialValues { get; set; } = false;
        public bool DetailedOutput { get; set; } = false;
        public IEnumerable<string> Ignore { get; set; }
        public string Description { get => "Populate the Left and Right with objects to be diffed and POST to this endpoint. OutputMode options: [default] Symbol, Detailed."; }
    }
}
