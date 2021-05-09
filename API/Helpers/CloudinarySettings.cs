using System.Collections.Generic;

namespace API.Helpers
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        
        public string ApiKey { get; set; }
        
        public string ApiSecret { get; set; }

        public Dictionary<string, object> Transformation { get; set; }
    }
}
