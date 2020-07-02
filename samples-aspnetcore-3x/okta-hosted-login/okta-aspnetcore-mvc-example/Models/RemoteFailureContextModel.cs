using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace okta_aspnetcore_mvc_example.Models
{
    public class RemoteFailureContextModel
    {
        public RemoteFailureContextModel(RemoteFailureContext context)
        {
            this.Failure = context?.Failure.Message;
        }
        
        public string Failure { get; set; }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        public static RemoteFailureContextModel FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RemoteFailureContextModel>(json);
        }
    }
}